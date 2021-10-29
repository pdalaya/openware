using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Menu {
    public class MenuSetup : MonoBehaviour {
        public GameObject ButtonPrefab;
        public GridLayoutGroup ButtonContainer;

        private void Awake() {
            restoreDefaults();
            populateButtons();
        }

        void populateButtons() {
            List<string> sceneNames = Utilities.MinigameScenes();

            foreach (string name in sceneNames) {
                GameObject button = Instantiate(ButtonPrefab);
                button.transform.SetParent(ButtonContainer.transform);
                button.name = name;
                button.GetComponentInChildren<TextMeshProUGUI>().text = name;
            }
        }

        void restoreDefaults() {
            Cursor.visible = true;
            Physics.gravity = new Vector3(0f, -9.8f, 0f);
        }
    }
}