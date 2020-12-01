#Start form scene 0

To force the editor to launch the game from scene 0 (just like a standalone build would), go to Edit menu and select Always Start From Scene 0.

You can always unselect it anytime you want if you need to launch a specific scene.

Relevant source file:
_PlayFromFirstScene.cs_


#Battle Tester
The Battle Tester is a tool meant to bypass the player selection when you want to play a battle, and to be able to play with as many players as you want without needing the proper amount of controllers. The main goal is to be efficient when working on a specific game mode and be able to iterate on said mode without losing time in other parts of the game. This tool isn't affected by the Always Start From Scene 0 setting.

To access it, go to Tools, then Battle Tester. Here is the screen you will see:
 Here are the options available to customize the test:
*	Nb players: number of characters spawned (they are technically all playable, see below)
*	Use keyboard: All spawned characters are playable, a controller is assigned to them even if there aren't enough controllers plugged. This option determine if one of these character is playable with the keyboard. Should probably be true must of the time.
*	Game mode: rules used for the test
*	Spawn Blocks: If this option is toggled (which it is by default), destructible blocks are spawned as normal. Unchecking the box empties the level of such blocks, meaning it's easier to reach any point of the map. Indestructible walls are still here.
Once all the settings are correct, press Launch Battle. During play, this button turns into Stop Battle and can be used to exit Play mode.

In the future, this tool should be able to  edit all the details of a game state (player positions, tiles contents, etc...) and to save/load it from a Scriptable Object. This will be useful for debug and balancing.

Relevant source files:
_BattleTestTool.cs_
_TestBattleInitializer.cs_