using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; } = false;
    
    // #################################################### -> BANHAMMER SETTINGS <- ##########################################################################
    public bool BanHammerWebhookEnabled { get; set; } = true;
    public string BanHammerWebhookUrl { get; set; } = "https://discord.com/api/webhooks/1386757994519199784/_nH8UILmbIdqF6iMqS0fy5nY7z_BM_1zmLqUPBOhTY8ouvDYn24tbkXrw5i-1w4d7EbH";
    
    // Basic embed settings
    public List<string> Titles { get; set; } = new List<string>
    {
        "🔨 Ban Hammer Strikes!",
        "⚖️ Justice Has Been Served",
        "🚫 Player Banned", 
        "🛡️ Server Protection Activated",
        "💀 Another One Bites the Dust",
        "🚨 Security Alert",
        "⛔ Access Denied"
    };
    
    public int EmbedColor { get; set; } = 16711680; // Red color (0xFF0000)
    public string FooterText { get; set; } = "⚖️ Justice served at";
    public string PermanentBanText { get; set; } = "PERMANENT";
    public string DurationFormat { get; set; } = "{0:F0} minutes";
    public double PermanentBanThresholdDays { get; set; } = 365.0;
    
   
    public string PlayerFieldName { get; set; } = "Player";
    public string AdminFieldName { get; set; } = "Admin";
    public string ReasonFieldName { get; set; } = "Reason";
    public string DurationFieldName { get; set; } = "Duration";
    
    public bool PlayerFieldInline { get; set; } = true;
    public bool AdminFieldInline { get; set; } = true;
    public bool ReasonFieldInline { get; set; } = false;
    public bool DurationFieldInline { get; set; } = false;
    

    [Description("Show SteamID of banned player and admin")]
    public bool ShowSteamId { get; set; } = false;
    
    public string PlayerSteamIdFieldName { get; set; } = "Player SteamID";
    public string AdminSteamIdFieldName { get; set; } = "Admin SteamID";
    public bool PlayerSteamIdFieldInline { get; set; } = true;
    public bool AdminSteamIdFieldInline { get; set; } = true;
    

    [Description("Show IP addresses of banned player and admin (SECURITY RISK!)")]
    public bool ShowIpAddress { get; set; } = false;
    
    public string PlayerIpFieldName { get; set; } = "Player IP";
    public string AdminIpFieldName { get; set; } = "Admin IP";
    public bool PlayerIpFieldInline { get; set; } = true;
    public bool AdminIpFieldInline { get; set; } = true;
    

    [Description("List of GIF URLs to randomly display with ban notifications")]
    public List<string> BanGifs { get; set; } = new List<string>
    {
        "https://media1.giphy.com/media/v1.Y2lkPTc5MGI3NjExbm9vODE0bTNpOTZ2c3hsaXd6NjVodHoxb2FwMHpsemNqM201aTc4YSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/GIuQfHtQO8QrgAlhqv/giphy.gif",
        "https://media2.giphy.com/media/v1.Y2lkPTc5MGI3NjExMzlldTlseTUzN2hsaGZwYTA2MzBwM2VpbWhzdmRwcnBhZG51ZWY0NSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/7vYY9kKV7hO10c0Reb/giphy.gif",
        "https://media3.giphy.com/media/v1.Y2lkPTc5MGI3NjExNzlweDFzamt3eHp6emJyZzBhamg2bzVmZWU4dnNmem5rZ3Blc3NrbyZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/fe4dDMD2cAU5RfEaCU/giphy.gif",
        "https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExbTYya291YWI2NGdpcGt3azk2YW54OWt4aXZpdXpwY2FxMjB5eTRuZCZlcD12MV9naWZzX3NlYXJjaCZjdD1n/ObXgWWGHzMlVe/giphy.gif",
        "https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExbTYya291YWI2NGdpcGt3azk2YW54OWt4aXZpdXpwY2FxMjB5eTRuZCZlcD12MV9naWZzX3NlYXJjaCZjdD1n/xT0GqiRi9X1i9XPVSg/giphy.gif",
        "https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExaG9pdHRjZjhodDloNDhkdnk0djhkOTY1bDVuc2NvZHBmeXpxMGpvNyZlcD12MV9naWZzX3NlYXJjaCZjdD1n/aOy7yK8uBB8XLvchGP/giphy.gif",
        "https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExdjdjeDY2a3N4a3JvYzBsc2xiejU1anZnaXpqa241MHYwMDJ0Z29nbyZlcD12MV9naWZzX3NlYXJjaCZjdD1n/KCe9K3t7lNs58UvobV/giphy.gif"
    };
    
    // #################################################### -> BANHAMMER SETTINGS END <- ##########################################################################
}