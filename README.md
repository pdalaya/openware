# openware
- Wario Ware inspired open source minigames
- [Download on itch.io](https://aidanwaite.itch.io/openware)

# Vision
- A collection of simple minigames that are fun to make and fun to play
- Players should be able to learn and beat minigames in roughly 10 seconds

# Adding a minigame
1. Install Unity 2019.4.12f1 LTS.
2. Open project.
3. Create a new folder in Minigames.
4. Put your scene and all related files in your new folder.
5. Add your new scene to Build Settings
6. Use a minigame-specific namespace for all of your scripts.
7. Make the minigame!
8. Check out another minigame scene and see how `Minigame Completion Handler` is used. You'll need to invoke the win and lose callbacks in the same way in your new minigame.
9. Open the `Minigame Menu` scene and add a button for your new minigame.
10. Open SuperManager.cs and search for the comments `// Add new minigame here`. Each place that comment appears you need to make a change for your new minigame. 
11. Make a pull request with your new minigame and tag `Aidan Waite` as a reviewer.
12. Pat yourself on the back!

# Notes

- Make sure the player can see the result of the minigame. Before calling a `MinigameCompletionHandler` callback, clearly show that the minigame has been won or lost.

# Art and sound assets
Art and sound assets can only be used if we've got the appropriate license. For this reason, making original art is encouraged. If you source art or sound from somewhere please link it here after you check that the license permits its use. Note "original" here means it was created specifically for Openware and thus it uses Openware's license.