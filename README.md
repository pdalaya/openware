# openware
- Wario Ware inspired open source minigames
- [Download on itch.io](https://aidanwaite.itch.io/openware)

# Vision
- A collection of simple minigames that are fun to make and fun to play
- Players should be able to learn and beat minigames in roughly 10 seconds
- Make sure the player can see the result of the minigame. Before calling a `MinigameCompletionHandler` callback, clearly show that the minigame has been won or lost.

# Adding a minigame
1. Install Unity 2019.4.12f1 LTS.
2. Fork the repo and clone your fork. You may need ssh auth setup for this to work.
3. Open project.
4. Set the `Game` resolution to 800x800.
5. Open the `SuperManager` scene and hit play. This will give you an idea of everything fits together.
6. Create a new folder in Minigames.
7. Put your scene and all related files in your new folder.
8. Add your new scene to Build Settings.
9. Use a minigame-specific namespace for all of your scripts.
10. Make the minigame!
11. Check out another minigame scene and see how `Minigame Completion Handler` is used. You'll need to invoke the win and lose callbacks in the same way in your new minigame.
12. Open SuperManager.cs and search for the comments `// Add new minigame here`
13. Make a pull request with your new minigame from your fork back to the original repo and tag `Aidan Waite` as a reviewer.

# Art and sound assets
Art and sound assets can only be used if we've got the appropriate license. For this reason, making original art is encouraged. If you source art or sound from somewhere please link it here after you check that the license permits its use. Note "original" here means it was created specifically for Openware and thus it uses Openware's license.