using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallyBird {
    public class PipeSpawner : MonoBehaviour {
        public GameObject PipeColumnPrefab;
        public GameObject PipesContainer;

        private void Awake() {
            float minY = 1.2f;
            float maxY = 3.96f;

            for (int i = 0; i < 50; i++) {
                float y = Random.Range(minY, maxY);
                GameObject newPipe = Instantiate(PipeColumnPrefab, Vector3.zero, Quaternion.identity);
                newPipe.transform.SetParent(PipesContainer.transform);
                newPipe.transform.localPosition = new Vector3(i * 5f, y, 0f);
            }

        }
    }
}