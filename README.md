Clone of Jumping Jack for ZX Spectrum

BUILD INFORMATION
Built with Unity 2018.2.2f1 Personal(64bit) on Windows 10.

CONTROLS
Player Controls:
[Left Arrow] - Move Left
[Right Arrow] - Move Right
[Up Arrow] - Move Up

Game Controls:
[Escape] - Exit game
[Space] - Restart game

Cheats:
[G] - God Mode
Press [G] in the introduction screen to set God Mode.
God Mode allows you to move without reacting to holes or hazards.
With God Mode you can quickly test if the whole game is there.

[A] - Level selection
Press [A] in the introduction scene to activate.
A display of the current level will appear at the top of the screen.
Continue pressing [A] to get to the desired level, in range (1..20).
The game will not automatically start after activating this, so you need to press [Enter] when you are satisfied with your selection.
With this cheat, you can test a specific level.

These cheats can be activated separately or together.

In case you want to reset, select a different cheat combination or don't want to use cheats, just press [Space] to restart the game.
Project Layout

Information about some more important files
/Animations/ - Contains all animations
/Audio/ - "DoTween" plug-in folder
/Fonts/ - Imported fonts
/Prefabs/ - Prefabs, not very important since most things are contained in the scenes
/Resources/ - for "DoTween" plug-in
/Scenes/ - All scenes
  EndScene - Scene displayed when you lose or complete all 20 levels
  GameScene - Scene where the game happens
  IntroScene - First scene loaded on game start/restart
  NextLevelScene - Scene displayed in between game levels
/Scripts/ - All code
  Behaviours - Small self-containing behaviours that are usually used by a controller
  Controllers - Code for controlling the game
  Settings - Code for scriptable objects that are used as settings
  Utils - Place for enums and generic utilities
/Settings/ - Scriptable objects that contain game settings
/Textures/ - All textures
