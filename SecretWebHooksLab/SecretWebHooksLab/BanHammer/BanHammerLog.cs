using Exiled.Events.EventArgs.Player;
using System;
using System.IO;
using System.Threading.Tasks;
using Exiled.API.Features;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;

namespace SecretWebHooksLab.WebHooks
{
    public class BanHammerLog
    {
        private readonly string _logDirectory;
        private static readonly object _fileLock = new object();
        
        public BanHammerLog()
        {
         
            _logDirectory = Path.Combine(Paths.Configs, "BanLogs");
            
      
            if (!Directory.Exists(_logDirectory))
            {
                try
                {
                    Directory.CreateDirectory(_logDirectory);
                    Log.Info($"[BanHammer] Created BanLogs directory: {_logDirectory}");
                }
                catch (Exception ex)
                {
                    Log.Error($"[BanHammer] Failed to create BanLogs directory: {ex.Message}");
                }
            }
        }
        
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.Banned += OnPlayerBanned;
        }
        
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Banned -= OnPlayerBanned;
        }

        private void OnPlayerBanned(BannedEventArgs ev)
        {
            Task.Run(async () =>
            {
                try
                {
                    await LogBanAsync(ev);
                }
                catch (Exception ex)
                {
                    Log.Error($"[BanHammer] Critical error in logging task: {ex}");
                }
            });
        }

        private async Task LogBanAsync(BannedEventArgs ev)
        {
            try
            {
                var config = SecretWebHooksLab.Instance?.Config;
                
                if (config == null)
                {
                    Log.Error("[BanHammer] Config is null!");
                    return;
                }

                if (ev?.Details == null)
                {
                    Log.Error("[BanHammer] Ban details are null!");
                    return;
                }

           
                var banData = PrepareBanData(ev);
                await SaveToJsonLogAsync(banData, config);
                await SaveToCsvLogAsync(banData, config);
                
                if (config.Debug)
                {
                    Log.Debug($"[BanHammer] Ban logged successfully for player {banData.PlayerName}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[BanHammer] Error logging ban: {ex}");
            }
        }

        private BanLogEntry PrepareBanData(BannedEventArgs ev)
        {
            long issuanceTicks = ev.Details.IssuanceTime;
            long expiresTicks = ev.Details.Expires;
            DateTime issuanceTime = new DateTime(issuanceTicks, DateTimeKind.Utc);
            DateTime expiresTime = new DateTime(expiresTicks, DateTimeKind.Utc);
            TimeSpan duration = expiresTime - issuanceTime;

            var banData = new BanLogEntry
            {
                Timestamp = DateTime.UtcNow,
                IssuanceTime = issuanceTime,
                ExpiresTime = expiresTime,
                DurationMinutes = (long)duration.TotalMinutes,
                PlayerName = ev.Target?.Nickname ?? "Unknown Player",
                PlayerSteamId = ev.Target?.UserId ?? "Unknown",
                PlayerIp = ev.Target?.IPAddress ?? "Unknown",
                AdminName = ev.Player?.Nickname ?? "Unknown Admin",
                AdminSteamId = ev.Player?.UserId ?? "Unknown",
                AdminIp = ev.Player?.IPAddress ?? "Unknown",
                Reason = ev.Details.Reason ?? "No reason provided",
                IsPermanent = duration.TotalDays >= 365
            };

            return banData;
        }

        private async Task SaveToJsonLogAsync(BanLogEntry banData, dynamic config)
        {
            try
            {
                string fileName = $"bans_{banData.Timestamp:yyyy-MM}.json";
                string filePath = Path.Combine(_logDirectory, fileName);
                
                List<BanLogEntry> existingBans = new List<BanLogEntry>();
                
                lock (_fileLock)
                {
                  
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            string existingJson = File.ReadAllText(filePath);
                            existingBans = JsonConvert.DeserializeObject<List<BanLogEntry>>(existingJson) ?? new List<BanLogEntry>();
                        }
                        catch (JsonException jsonEx)
                        {
                            Log.Error($"[BanHammer] Error reading existing ban log: {jsonEx.Message}");
                         
                            string backupPath = $"{filePath}.backup_{DateTime.Now:yyyyMMdd_HHmmss}";
                            File.Move(filePath, backupPath);
                            Log.Warn($"[BanHammer] Corrupted file backed up to: {backupPath}");
                        }
                    }
                    
         
                    existingBans.Add(banData);
                    
                    string json = JsonConvert.SerializeObject(existingBans, Formatting.Indented);
                    File.WriteAllText(filePath, json);
                }
                
                if (SecretWebHooksLab.Instance.Config.Debug)
                {
                    Log.Debug($"[BanHammer] Ban saved to JSON: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[BanHammer] Error saving to JSON log: {ex}");
            }
        }

        private async Task SaveToCsvLogAsync(BanLogEntry banData, dynamic config)
        {
            try
            {
          
                string fileName = $"bans_{banData.Timestamp:yyyy-MM}.csv";
                string filePath = Path.Combine(_logDirectory, fileName);
                
                lock (_fileLock)
                {
                    bool fileExists = File.Exists(filePath);
                    
                    using (var writer = new StreamWriter(filePath, append: true))
                    {
                   
                        if (!fileExists)
                        {
                            writer.WriteLine("Timestamp,IssuanceTime,ExpiresTime,DurationMinutes,PlayerName,PlayerSteamId,PlayerIp,AdminName,AdminSteamId,AdminIp,Reason,IsPermanent");
                        }
                        
                 
                        writer.WriteLine($"{banData.Timestamp:yyyy-MM-dd HH:mm:ss}," +
                                       $"{banData.IssuanceTime:yyyy-MM-dd HH:mm:ss}," +
                                       $"{banData.ExpiresTime:yyyy-MM-dd HH:mm:ss}," +
                                       $"{banData.DurationMinutes}," +
                                       $"\"{EscapeCsvField(banData.PlayerName)}\"," +
                                       $"\"{EscapeCsvField(banData.PlayerSteamId)}\"," +
                                       $"\"{EscapeCsvField(banData.PlayerIp)}\"," +
                                       $"\"{EscapeCsvField(banData.AdminName)}\"," +
                                       $"\"{EscapeCsvField(banData.AdminSteamId)}\"," +
                                       $"\"{EscapeCsvField(banData.AdminIp)}\"," +
                                       $"\"{EscapeCsvField(banData.Reason)}\"," +
                                       $"{banData.IsPermanent}");
                    }
                }
                
                if (SecretWebHooksLab.Instance.Config.Debug)
                {
                    Log.Debug($"[BanHammer] Ban saved to CSV: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[BanHammer] Error saving to CSV log: {ex}");
            }
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;
                
         
            return field.Replace("\"", "\"\"");
        }

        public async Task<BanStatistics> GetBanStatisticsAsync(DateTime fromDate, DateTime toDate)
        {
            var statistics = new BanStatistics();
            
            try
            {
             
                for (DateTime date = fromDate; date <= toDate; date = date.AddMonths(1))
                {
                    string fileName = $"bans_{date:yyyy-MM}.json";
                    string filePath = Path.Combine(_logDirectory, fileName);
                    
                    if (File.Exists(filePath))
                    {
                        lock (_fileLock)
                        {
                            try
                            {
                                string json = File.ReadAllText(filePath);
                                var bans = JsonConvert.DeserializeObject<List<BanLogEntry>>(json) ?? new List<BanLogEntry>();
                                
                                foreach (var ban in bans)
                                {
                                    if (ban.Timestamp >= fromDate && ban.Timestamp <= toDate)
                                    {
                                        statistics.TotalBans++;
                                        
                                        if (ban.IsPermanent)
                                            statistics.PermanentBans++;
                                        else
                                            statistics.TemporaryBans++;
                                        
                                        if (!statistics.AdminStats.ContainsKey(ban.AdminName))
                                            statistics.AdminStats[ban.AdminName] = 0;
                                        statistics.AdminStats[ban.AdminName]++;
                                    }
                                }
                            }
                            catch (JsonException jsonEx)
                            {
                                Log.Error($"[BanHammer] Error reading statistics from {filePath}: {jsonEx.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[BanHammer] Error calculating ban statistics: {ex}");
            }
            
            return statistics;
        }
    }

 
    public class BanLogEntry
    {
        public DateTime Timestamp { get; set; }
        public DateTime IssuanceTime { get; set; }
        public DateTime ExpiresTime { get; set; }
        public long DurationMinutes { get; set; }
        public string PlayerName { get; set; }
        public string PlayerSteamId { get; set; }
        public string PlayerIp { get; set; }
        public string AdminName { get; set; }
        public string AdminSteamId { get; set; }
        public string AdminIp { get; set; }
        public string Reason { get; set; }
        public bool IsPermanent { get; set; }
    }

    
    public class BanStatistics
    {
        public int TotalBans { get; set; }
        public int PermanentBans { get; set; }
        public int TemporaryBans { get; set; }
        public Dictionary<string, int> AdminStats { get; set; } = new Dictionary<string, int>();
    }
}