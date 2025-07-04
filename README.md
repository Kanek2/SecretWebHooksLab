# SecretWebHooksLab 🔨

 Discord webhook plugin for SCP: Secret Laboratory servers using the EXILED framework. This plugin currently provides advanced ban notifications with detailed logging capabilities.

## Features

- 🔨 **Ban Hammer Webhook**: Real-time Discord notifications for player bans
- 📊 **Detailed Ban Logging**: JSON and CSV log files for ban tracking
- 🎨 **Customizable Embeds**: Configurable Discord embed appearance
- 🔄 **Random Elements**: Random titles and GIFs for varied notifications


## Installation

1. Download the latest release from the [Releases](../../releases) page
2. Place the `.dll` file in your EXILED plugins folder
3. Restart your server to generate the configuration file
4. Configure the plugin in your EXILED config file

## Features in Detail

### Ban Hammer Webhook
- Sends instant Discord notifications when players are banned
- Includes player info, admin info, reason, and duration
- Supports random titles and GIFs for variety
- Configurable embed colors and styling

### Ban Logging System
- **JSON Logs**: Structured data for programmatic access
- **CSV Logs**: Spreadsheet-compatible format for analysis
- **Monthly Files**: Organized by year-month for easy management

### Error Handling
- Comprehensive error catching and logging
- Graceful degradation when Discord is unavailable
- File corruption protection with automatic backups
- Timeout handling for webhook requests

## File Structure

```
EXILED/Configs/
├── BanLogs/
│   ├── bans_2024-01.json
│   ├── bans_2024-01.csv
│   ├── bans_2024-02.json
│   └── bans_2024-02.csv
└── config.yml
```

## Troubleshooting

### Common Issues

1. **Webhook not sending**
   - Check if the webhook URL is valid
   - Verify Discord webhook permissions
   - Enable debug mode to see detailed error messages

2. **Logs not saving**
   - Check server permissions for the BanLogs directory
   - Verify disk space availability
   - Review server logs for file system errors


## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License.

## Support

- Create an issue on GitHub for bug reports
- Send a message on EXILED Discord plugin thread

## Changelog

### Version 0.2.0
- Initial release
- Ban hammer webhook functionality
- JSON and CSV logging
- Statistics tracking
- Configurable embeds and fields
