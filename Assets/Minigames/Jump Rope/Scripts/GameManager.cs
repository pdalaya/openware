using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace JumpRope {
    public class GameManager : MonoBehaviour {

        public MinigameCompletionHandler MinigameCompletionHandler;
        public GameObject Roof;
        public GameObject JumpRope;
        public GameObject Player;
        public Vector3 Gravity;
        public TextMeshProUGUI Text;

        Jump jump;
        bool isCompleted = false;
        int secondsRemaining = 8;

        private void Awake() {
            Physics.gravity = Gravity;
            StartCoroutine("StartGameFlow");
            jump = Player.GetComponent<Jump>();
        }

        private void Update() {
            if (isCompleted) {
                jump.DoJump();
            }
        }

        public void HandleLose() {
            if (isCompleted) {
                return;
            }

            Roof.SetActive(false);
            isCompleted = true;

            var rb = Player.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            rb.AddExplosionForce(1000f, new Vector3(Random.Range(3f,20f), Random.Range(3f, 20f), Random.Range(3f, 20f)), 100f);

            Text.text = "You lose!";
            StartCoroutine("loseCallback");
        }

        IEnumerator loseCallback() {
            yield return new WaitForSeconds(2);
            MinigameCompletionHandler.LoseCallback.Invoke();
        }

        public void HandleWin() {
            if (isCompleted) {
                return;
            }

            Roof.SetActive(false);
            isCompleted = true;

            Text.text = "You win!";
            StartCoroutine("winCallback");
        }

        IEnumerator winCallback() {
            yield return new WaitForSeconds(2);
            MinigameCompletionHandler.WinCallback.Invoke();
        }

        IEnumerator StartGameFlow() {
            Player.SetActive(false);
            Text.text = "Ready? Press space to jump.";
            yield return new WaitForSeconds(1.3f);

            Player.SetActive(true);
            Text.text = "Go!";

            InvokeRepeating("countdown", 1f, 1f);
        }

        void countdown() {
            if (isCompleted) {
                return;
            }

            Text.text = secondsRemaining + " seconds remaining";

            if (secondsRemaining == 0) {
                HandleWin();
            } else {
                secondsRemaining -= 1;
            }
        }

        IEnumerator returnToMenuAfterDelay() {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("MainMenu");
        }

    }
}