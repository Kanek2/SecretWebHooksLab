using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Exiled.API.Features;

namespace SecretWebHooksLab.WebHooks
{
    public class BanHammerWebhook
    {
        private readonly HttpClient _httpClient;
        private static readonly Random _random = new();
        
        public BanHammerWebhook()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }
        
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.Banned += OnPlayerBanned;
        }
        
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Banned -= OnPlayerBanned;
        }

        public void OnPlayerBanned(BannedEventArgs ev)
        {
         
            Task.Run(async () =>
            {
                try
                {
                    await SendDiscordWebhookAsync(ev);
                }
                catch (Exception ex)
                {
                    Log.Error($"[BanHammer] Critical error in webhook task: {ex}");
                }
            });
        }

        private async Task SendDiscordWebhookAsync(BannedEventArgs ev)
        {
            try
            {
                var config = SecretWebHooksLab.Instance?.Config;
                
                if (config == null)
                {
                    Log.Error("[BanHammer] Config is null!");
                    return;
                }
                
                if (string.IsNullOrEmpty(config.BanHammerWebhookUrl))
                {
                    if (config.Debug)
                        Log.Debug("[BanHammer] Webhook URL not configured, skipping.");
                    return;
                }

                if (ev?.Details == null)
                {
                    Log.Error("[BanHammer] Ban details are null!");
                    return;
                }

                long issuanceTicks = ev.Details.IssuanceTime;
                long expiresTicks = ev.Details.Expires;
                DateTime issuanceTime = new DateTime(issuanceTicks, DateTimeKind.Utc);
                DateTime expiresTime = new DateTime(expiresTicks, DateTimeKind.Utc);
                TimeSpan duration = expiresTime - issuanceTime;
                
                
                string durationText;
                try
                {
                    durationText = duration.TotalDays >= config.PermanentBanThresholdDays 
                        ? config.PermanentBanText ?? "PERMANENT"
                        : string.Format(config.DurationFormat ?? "{0:F0} minutes", duration.TotalMinutes);
                }
                catch (Exception ex)
                {
                    Log.Error($"[BanHammer] Error formatting duration: {ex.Message}");
                    durationText = "Unknown duration";
                }

             
                string selectedTitle;
                try
                {
                    var titles = config.Titles;
                    if (titles != null && titles.Count > 0)
                    {
                        selectedTitle = titles[_random.Next(titles.Count)];
                    }
                    else
                    {
                        selectedTitle = "Player Banned";
                        if (config.Debug)
                            Log.Debug("[BanHammer] No titles configured, using default.");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"[BanHammer] Error selecting title: {ex.Message}");
                    selectedTitle = "Player Banned";
                }

               
                string selectedGif = null;
                try
                {
                    var gifs = config.BanGifs;
                    if (gifs != null && gifs.Count > 0)
                    {
                        selectedGif = gifs[_random.Next(gifs.Count)];
                    }
                    else if (config.Debug)
                    {
                        Log.Debug("[BanHammer] No GIFs configured, embed will have no image.");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"[BanHammer] Error selecting GIF: {ex.Message}");
                }

            
                string playerName = ev.Target?.Nickname ?? "Unknown Player";
                string adminName = ev.Player?.Nickname ?? "Unknown Admin";
                string reason = ev.Details.Reason ?? "No reason provided";

              
                var fields = new List<object>
                {
                    new { 
                        name = config.PlayerFieldName ?? "Player", 
                        value = playerName, 
                        inline = config.PlayerFieldInline 
                    },
                    new { 
                        name = config.AdminFieldName ?? "Admin", 
                        value = adminName, 
                        inline = config.AdminFieldInline 
                    },
                    new { 
                        name = config.ReasonFieldName ?? "Reason", 
                        value = reason, 
                        inline = config.ReasonFieldInline 
                    },
                    new { 
                        name = config.DurationFieldName ?? "Duration", 
                        value = durationText, 
                        inline = config.DurationFieldInline 
                    }
                };

                // Add SteamID fields if enabled
                if (config.ShowSteamId)
                {
                    string playerSteamId = ev.Target?.UserId ?? "Unknown";
                    string adminSteamId = ev.Player?.UserId ?? "Unknown";
                    
                    fields.Add(new {
                        name = config.PlayerSteamIdFieldName ?? "Player SteamID",
                        value = playerSteamId,
                        inline = config.PlayerSteamIdFieldInline
                    });
                    
                    fields.Add(new {
                        name = config.AdminSteamIdFieldName ?? "Admin SteamID", 
                        value = adminSteamId,
                        inline = config.AdminSteamIdFieldInline
                    });
                }

               
                if (config.ShowIpAddress)
                {
                    string playerIp = ev.Target?.IPAddress ?? "Unknown";
                    string adminIp = ev.Player?.IPAddress ?? "Unknown";
                    
                    fields.Add(new {
                        name = config.PlayerIpFieldName ?? "Player IP",
                        value = playerIp,
                        inline = config.PlayerIpFieldInline
                    });
                    
                    fields.Add(new {
                        name = config.AdminIpFieldName ?? "Admin IP",
                        value = adminIp, 
                        inline = config.AdminIpFieldInline
                    });
                }

                var webhook = new
                {
                    embeds = new[]
                    {
                        new
                        {
                            title = selectedTitle,
                            color = config.EmbedColor,
                            fields = fields.ToArray(),
                            image = !string.IsNullOrEmpty(selectedGif) ? new { url = selectedGif } : null,
                            timestamp = DateTime.UtcNow,
                            footer = new { text = config.FooterText ?? "Ban notification" }
                        }
                    }
                };

                var json = JsonConvert.SerializeObject(webhook);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
              
                
                var response = await _httpClient.PostAsync(config.BanHammerWebhookUrl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    if (config.Debug)
                    {
                        Log.Debug($"[BanHammer] Webhook sent successfully for player {playerName}");
                    }
                }
                else
                {
                    Log.Warn($"[BanHammer] Webhook failed with status: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                Log.Error($"[BanHammer] HTTP error sending webhook: {httpEx.Message}");
            }
            catch (TaskCanceledException tcEx)
            {
                Log.Error($"[BanHammer] Webhook request timed out: {tcEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                Log.Error($"[BanHammer] JSON serialization error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Log.Error($"[BanHammer] Unexpected error sending webhook: {ex}");
            }
        }

     
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
