using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace FallyBird {
    public class FallyBirdController : MonoBehaviour {

        // TODO: set the value that ends up working nicely as the initial value here
        public float JumpPower;

        public TextMeshProUGUI ResultText;
        public TextMeshProUGUI SecondsText;

        bool isComplete = false;

        Rigidbody2D rb;
        int secondsRemaining = 10;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            ResultText.gameObject.SetActive(false);

            SecondsText.text = "" + secondsRemaining;
            InvokeRepeating("countdown", 1f, 1f);
        }

        void Update() {
            if (isComplete) {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
        }

        void countdown() {
            if (isComplete) {
                return;
            }

            secondsRemaining--;
            SecondsText.text = "" + secondsRemaining;

            if (secondsRemaining <= 0) {
                handleWin();
            }
        }

        public void Jump() {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * JumpPower);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (isComplete) {
                return;
            }

            handleLose();
        }

        void handleLose() {
            if (isComplete) {
                return;
            }

            GetComponent<SpriteRenderer>().enabled = false;
            rb.Sleep();
            GetComponent<CircleCollider2D>().enabled = false;

            ResultText.gameObject.SetActive(true);
            ResultText.text = "You Lose!";

            isComplete = true;
            StartCoroutine("returnToMenuAfterDelay");
        }

        void handleWin() {
            if (isComplete) {
                return;
            }

            GetComponent<SpriteRenderer>().enabled = false;
            rb.Sleep();
            GetComponent<CircleCollider2D>().enabled = false;

            ResultText.gameObject.SetActive(true);
            ResultText.text = "You Win!";

            isComplete = true;
            StartCoroutine("returnToMenuAfterDelay");
        }

        IEnumerator returnToMenuAfterDelay() {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}