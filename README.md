OriginLauncher v0.5


Changelog (0.5)

First release; Current feature set:

	*Manages Origin silently in the background
	
	*Automatic detection of games in users Origin library
	
	*Silent mode can be run from Steam. With proper configuration,
	 app automatically safely closes Origin when user quits game
	 
Planned features:

	*Configuration of game EXE path in GUI (manual setup outlined below)
	
	*Configuration of silent mode operation in GUI
	
	*Launch flag to override Slient mode setting
	

Setup:

 1: Put OriginGameLauncher.exe, OriginGameLauncher.exe.config, and ManagedOrigin.dll in a safe place.
	(such as the installation folder of the game you wish to use it with)
	
 2:	Run OriginGameLauncher.exe. Choose your game from the list and hit launch.
	If the game launches properly, write down the name of the game's executable (i.e Titanfall.exe)
	
 3:	Close your game and Origin.
 
 4: Open OriginGameLauncher.exe.config in a text editor. Change the 'Silent' value to 'True'.
	Paste you're games executable file name into the 'GameProcessExe' key.
