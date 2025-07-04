using System;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using SecretWebHooksLab.WebHooks;
using YamlDotNet.Core;
using System.Text;
using Version = System.Version;

namespace SecretWebHooksLab
{
    public class SecretWebHooksLab : Plugin<Config>
    {
        public override string Name => "SecretWebHooksLab";
        public override string Author => "Kanekuu";
        public override Version Version => new Version(0, 2, 0);
        
        private BanHammerWebhook banHammerWebhook;
        private BanHammerLog banHammerLog;
        public static SecretWebHooksLab Instance { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
            
       
            if (Config.BanHammerWebhookEnabled)
            {
                banHammerWebhook = new BanHammerWebhook();
                banHammerLog = new BanHammerLog();
                banHammerLog.RegisterEvents();
                banHammerWebhook.RegisterEvents();
            }
         
            base.OnEnabled();
            
            
            DisplayPluginInfo();
        }

        public override void OnDisabled()
        {
         
            if (banHammerWebhook != null)
            {
                banHammerWebhook.UnregisterEvents();
                banHammerWebhook = null;
            }
            
            Instance = null;
            base.OnDisabled();
            
            Log.Info($"┌─────────────────────────────────────────────┐");
            Log.Info($"│ {Name} v{Version} has been disabled │");
            Log.Info($"└─────────────────────────────────────────────┘");
        }

        private void DisplayPluginInfo()
        {
            var sb = new StringBuilder();
            var enabledWebhooks = GetEnabledWebhooksCount();
            
            sb.AppendLine("┌─────────────────────────────────────────────────────────────────┐");
            sb.AppendLine($"│                    {Name} v{Version}                     │");
            sb.AppendLine($"│                        by {Author}                               │");
            sb.AppendLine("├─────────────────────────────────────────────────────────────────┤");
            sb.AppendLine($"│ Status:    ENABLED                                              │");
            sb.AppendLine($"│ Active Webhooks: {enabledWebhooks}/1                                            │");
            sb.AppendLine("├─────────────────────────────────────────────────────────────────┤");
            
          
            sb.AppendLine("│  WEBHOOK MODULES:                                               │");
            sb.AppendLine("├─────────────────────────────────────────────────────────────────┤");
            
            DisplayWebhookModule(sb, "Ban Hammer", Config.BanHammerWebhookEnabled, 
                GetMaskedUrl(Config.BanHammerWebhookUrl), "Player ban notifications");
            
        
   
            
            sb.AppendLine("└─────────────────────────────────────────────────────────────────┘");
            
      
            var lines = sb.ToString().Split('\n');
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    Log.Info(line.TrimEnd());
            }
        }

        private void DisplayWebhookModule(StringBuilder sb, string moduleName, bool isEnabled, 
            string webhookUrl, string description)
        {
            var status = isEnabled ? "🟢 ENABLED " : "🔴 DISABLED";
            var urlDisplay = isEnabled && !string.IsNullOrEmpty(webhookUrl) 
                ? webhookUrl 
                : "Not configured";
            
            sb.AppendLine($" • {moduleName,-20} {status,-10}                      ");
            sb.AppendLine($"   Description: {description,-41} ");
            sb.AppendLine($"   Webhook URL: {urlDisplay,-41} ");
            sb.AppendLine("│                                                                 │");
        }

        private int GetEnabledWebhooksCount()
        {
            int count = 0;
            
            if (Config.BanHammerWebhookEnabled && !string.IsNullOrEmpty(Config.BanHammerWebhookUrl))
                count++;
            
         
            
            return count;
        }

        private string GetMaskedUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "Not configured";
            
      
            if (url.Length > 30)
            {
                return url.Substring(0, 20) + "..." + url.Substring(url.Length - 10);
            }
            
            return url.Substring(0, Math.Min(url.Length / 2, 15)) + 
                   new string('*', Math.Max(url.Length - 15, 3));
        }
    }
}