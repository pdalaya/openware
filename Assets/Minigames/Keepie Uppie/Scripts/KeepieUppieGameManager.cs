using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace KeepieUppie {
    public class KeepieUppieGameManager : MonoBehaviour {
        public float ClickForce;
        public GameObject HelperPlatform;
        public GameObject SoccerBall;
        public Vector3 Gravity;
        public TextMeshProUGUI Text;
        public MinigameCompletionHandler MinigameCompletionHandler;

        int secondsRemaining = 10;
        bool isComplete = false;

        private void Awake() {
            Physics.gravity = Gravity;
            InvokeRepeating("countdown", 0f, 1f);
        }

        void countdown() {
            if (isComplete) {
                return;
            }

            if (secondsRemaining >= 8) {
                Text.text = "Click the ball to keep it off the ground.\nReady?";
                HelperPlatform.SetActive(true);
            } else if (secondsRemaining > 0) {
                HelperPlatform.SetActive(false);
                Text.text = "" + secondsRemaining;
            } else if (secondsRemaining <= 0) {
                Text.text = "You Win!";
                isComplete = true;
                StartCoroutine("winCallback");
            }

            secondsRemaining--;
        }

        IEnumerator winCallback() {
            yield return new WaitForSeconds(2);
            MinigameCompletionHandler.WinCallback.Invoke();
        }

        IEnumerator loseCallback() {
            yield return new WaitForSeconds(2);
            MinigameCompletionHandler.LoseCallback.Invoke();
        }

        public void DidClickOnSoccerBall() {
            Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 force = ((SoccerBall.transform.position - clickWorldPos) * ClickForce) + (Vector3.up * 200f);
            SoccerBall.GetComponent<Rigidbody2D>().AddForce(force);
        }

        public void BallDidHitGround() {
            if (isComplete) {
                return;
            }

            isComplete = true;
            Text.text = "You Lose!";
            StartCoroutine("loseCallback");
        }
    }
}