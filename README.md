# SecretWebHooksLab 🔨

**Version:** 0.1.0 (Beta)  
**Author:** Kanekuu  
**Framework:** EXILED API  

A comprehensive Discord webhook notification system for SCP: Secret Laboratory servers. Get real-time notifications about important server events directly in your Discord channels.

## ⚠️ **BETA VERSION** ⚠️

This is a beta release. While functional, it may contain bugs or undergo significant changes. Use in production at your own risk.

## 📋 Features

### Current Modules

#### 🔨 **Ban Hammer Webhook**
- **Real-time ban notifications** sent to Discord
- **Customizable embed messages** with random titles and GIFs
- **Detailed ban information** including player, admin, reason, and duration
- **Optional sensitive data** (SteamID, IP addresses) with security warnings
- **Permanent vs temporary ban detection**
- **Configurable visual styling** (colors, footer text, field layouts)

### 🚀 **Planned Features**
- Player join/leave notifications
- Round start/end events
- Admin action logs
- Server status monitoring
- And more...

## 📦 Installation

1. **Download** the latest release from the [Releases](../../releases) page
2. **Extract** the plugin files to your EXILED plugins folder:
   ```
   %appdata%\EXILED\Plugins\
   ```
3. **Restart** your server to generate the configuration file
4. **Configure** the plugin (see Configuration section below)

## ⚙️ Configuration

The plugin generates a configuration file at: `%appdata%\EXILED\Configs\Plugins\secret_web_hooks_lab`

### Basic Setup

```yaml
secret_web_hooks_lab:
  is_enabled: true
  debug: false
  
  # Discord Webhook URL (REQUIRED)
  ban_hammer_webhook_url: "https://discord.com/api/webhooks/YOUR_WEBHOOK_URL_HERE"
  ban_hammer_webhook_enabled: true
```

## 🎯 Discord Webhook Setup

1. **Open Discord** and go to your server
2. **Navigate** to the channel where you want notifications
3. **Click** the gear icon (Channel Settings)
4. **Go to** "Integrations" → "Webhooks"
5. **Click** "Create Webhook"
6. **Copy** the webhook URL
7. **Paste** it into your plugin configuration

## 🛡️ Security Considerations

- **Webhook URLs** are sensitive - keep them private
- **IP address logging** is disabled by default for privacy
- **SteamID logging** is disabled by default but safer than IPs
- **Use HTTPS webhooks** only (Discord provides these by default)

## 🐛 Troubleshooting

### Common Issues

**Webhook not sending:**
- Check if the webhook URL is correct
- Verify the webhook channel still exists
- Enable `debug: true` in config for detailed logs

**Plugin not loading:**
- Ensure EXILED is properly installed
- Check server console for error messages
- Verify all dependencies are present

**Configuration not working:**
- Restart server after config changes
- Check YAML syntax (indentation matters!)
- Look for typos in configuration keys

### Debug Mode

Enable debug logging by setting `debug: true` in your configuration:

```yaml
secret_web_hooks_lab:
  debug: true
```

This will provide detailed information about webhook operations in your server console.

## 📄 Dependencies

- **EXILED API** (Latest version recommended)
- **Newtonsoft.Json** (Included with EXILED)
- **YamlDotNet** (Included with EXILED)

## 🤝 Contributing

This is a beta release. If you encounter bugs or have feature requests:

1. **Check existing issues** before creating new ones
2. **Provide detailed bug reports** with server logs
3. **Test thoroughly** before submitting pull requests
4. **Follow the existing code style**


## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## ⚠️ Disclaimer

This software is provided "as is" without warranty of any kind. Use at your own risk. The author is not responsible for any damages or issues that may arise from using this plugin.

---

**Made with ❤️ by Kanekuu**

*For support, bug reports, or feature requests, please use the GitHub Issues page.*
