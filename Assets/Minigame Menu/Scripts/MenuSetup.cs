using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu {
    public class MenuSetup : MonoBehaviour {
        public Canvas Canvas;
        public GameObject OvalPrefab;
        public List<Sprite> Ovals;

        private void Awake() {
            restoreDefaults();

            for (int x = 0; x < 10; x++) {
                GameObject newOval = Instantiate(OvalPrefab);
                int ind = Random.Range(0, Ovals.Count);
                newOval.GetComponent<Image>().sprite = Ovals[ind];
                newOval.transform.SetParent(Canvas.transform);
                newOval.transform.SetSiblingIndex(1);
                newOval.name = "Background Oval";
            }
        }

        void restoreDefaults() {
            Cursor.visible = true;
            Physics.gravity = new Vector3(0f, -9.8f, 0f);
        }
    }
}