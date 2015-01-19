# Shimabutts
A nice group of apps for fansubbing and scanlating.

## Irc bot

### Project

Contains classes which handle looking after projects, members and their statuses.

### Commands

Commands which are created via the CommandTranslator and returned can be processed. The way of making the bot react to things.

### Root

ShimabuttsSettings is settings. You need to change the App.config to set her up.

ShimabuttsRedis is where all the redis commands are. You need to have redis as your backend for this bot currently.

ShimabuttsBot is library magic.

CommandTranslator is turning irc messages into commands.

## API

### ToDo