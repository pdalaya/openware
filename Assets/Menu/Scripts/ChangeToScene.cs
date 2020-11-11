using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu {
    public class ChangeToScene : MonoBehaviour {
        public string SceneName;

        public void ChangeScene() {
            SceneManager.LoadScene(SceneName);
        }
    }
}
