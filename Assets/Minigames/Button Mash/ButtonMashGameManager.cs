using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace ButtonMash {
    public class ButtonMashGameManager : MonoBehaviour {

        public TextMeshProUGUI Text;
        public GameObject BarMask;
        public MinigameCompletionHandler MinigameCompletionHandler;

        bool isComplete = false;

        Vector3 barStart = new Vector3(0f, 2.659f, 0f);
        Vector3 barEnd = new Vector3(7.39f, 2.659f, 0f);

        float tapCount = 0;
        float tapGoal = 20;
        int secondsRemaining = 9;

        private void Awake() {
            updateBar();
            Text.text = "Fill up the meter!";
            InvokeRepeating("countdown", 2f, 1f);
        }

        void countdown() {
            if (isComplete) {
                return;
            }

            secondsRemaining--;
            Text.text = "" + secondsRemaining;

            if (secondsRemaining <= 0) {
                isComplete = true;
                Text.text = "You lose";
                StartCoroutine("loseCallback");
            }
        }

        IEnumerator loseCallback() {
            yield return new WaitForSeconds(2);
            MinigameCompletionHandler.LoseCallback.Invoke();
        }

        public void DidTapButton() {
            if (isComplete) {
                return;
            }

            tapCount++;
            updateBar();
        }

        void updateBar() {
            float percent = Mathf.Min(tapCount / tapGoal, 1f);
            BarMask.transform.position = Vector3.Lerp(barStart, barEnd, percent);

            if (percent == 1) {
                isComplete = true;
                Text.text = "You win!";
                StartCoroutine("winCallback");
            }
        }

        IEnumerator winCallback() {
            yield return new WaitForSeconds(2);
            MinigameCompletionHandler.WinCallback.Invoke();
        }
    }
}

