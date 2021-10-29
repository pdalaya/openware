using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Utilities {
    public static List<string> MinigameScenes() {
        List<string> sceneNames = new List<string>();
        // Skip `SuperManager`, `Main Menu`, `Minigame Menu`, and `Score and lives`
        for (int x = 4; x < SceneManager.sceneCountInBuildSettings; x++) {
            string name = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(x));
            sceneNames.Add(name);
        }
        sceneNames.Sort();
        return sceneNames;
    }
}