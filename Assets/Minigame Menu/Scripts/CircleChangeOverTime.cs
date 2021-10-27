using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu {
    public class CircleChangeOverTime : MonoBehaviour {
        Vector3 fromScale;
        Vector3 toScale;
        float baseSize = 7f;
        float lerpTime;
        float currentLerpTime;
        RectTransform rt;

        private void Awake() {
            rt = GetComponent<RectTransform>();

            lerpTime = Random.Range(8f, 20f);

            float small = 0.35f;
            float large = 1.4f;

            fromScale = new Vector3(Random.Range(baseSize * small, baseSize * large), Random.Range(baseSize * small, baseSize * large), 1f);
            toScale = new Vector3(Random.Range(baseSize * small, baseSize * large), Random.Range(baseSize * small, baseSize * large), 1f);
            currentLerpTime = 0f;

            rt.anchoredPosition = new Vector2(Random.Range(0f, 800f), Random.Range(0f, 800f));
        }

        void Update() {
            currentLerpTime += Time.deltaTime;

            if (currentLerpTime > lerpTime) {
                Vector3 tmpFrom = fromScale;
                fromScale = toScale;
                toScale = tmpFrom;

                currentLerpTime = 0;
            }

            float percent = currentLerpTime / lerpTime;
            rt.localScale = Vector3.Lerp(fromScale, toScale, percent);
        }
    }
}