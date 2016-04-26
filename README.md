# OriginLauncher v1.0

## Why?
You might be wondering what good this is. After all, you can just add Origin.exe, or your game's executable file directly to Steam as a non-Steam game.

I did exaxtly those things for a few years and while it "works", I wouldn't call either experience ideal. In both cases, you have to manually manage Origin. When you're done playing a game, you need to quit Origin before the Steam overlay hook is released and usable for another game.

The goal of OriginGameLauncher is to manage Origin for you. When configured properly, OGL monitors your game and safely exits Origin when you're done, freeing up the Steam hook for your next game. OGL also starts the Origin client minimized, so you don't even have to think about it.

These features are especially useful for users of Steam Big Picture mode and Steam Link/In Home Streaming.

## Setup
1. Extract to a safe location (such as the installation folder of the game you wish to use it with)
2. Run OGLConfigurator.exe
3. Configure your game. You can test your configuration with the "Test" button
4. After saving your configuration, run OGLRunner.exe. You can also add OGLRunner.exe to your Steam library.

## Notes
- To reset your config, just delete OGLRunner.exe.config. A new config file will be created upon running the configurator.
- Compatibility with Battlefield games (BF3, BF4, and BF Hardline) is slightly iffy. I have no plans to improve the user expereience here. Users wanting to run these games through steam should check out Battlelogium (https://github.com/Battlelogium/Battlelogium/releases), as it properly integrates with the Battlelog web launcher.


## Changelog:
### 1.0
- GUI re-write. Moved GUI to separate configurator app.
- Removed "Silent" option. No longer necessary now that the game runner and configuration GUI are separated
	
**Planned features:**
- Warning on selection of certain Battlefield titles. Notify users to use Battlelogium instead.
- More Game EXE detection options (Auto-detect, or choose from a list of Origin child processes after launching)

### 0.5
**First release; Current feature set:**
- Manages Origin silently in the background
- Automatic detection of games in users Origin library
- Silent mode can be run from Steam. With proper configuration, app automatically safely closes Origin when user quits game

**Planned features:**
- Configuration of game EXE path in GUI (manual setup outlined below)
- Configuration of silent mode operation in GUI
- Launch flag to override Slient mode setting
