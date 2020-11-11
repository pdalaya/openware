using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Relax {
    public class RelaxGameManager : MonoBehaviour {

        public Transform ExplosionCenter;
        public TextMeshProUGUI FeedbackText;
        public TextMeshProUGUI CountdownText;
        public Transform BodyParts;
        public Color LoseColor;

        Rigidbody2D[] bodyPartsRB;
        bool isComplete = false;
        int secondsRemaining = 6;

        private void Awake() {
            FeedbackText.text = "Relax";

            bodyPartsRB = BodyParts.transform.GetComponentsInChildren<Rigidbody2D>();

            foreach (Rigidbody2D bodyPart in bodyPartsRB) {
                bodyPart.gravityScale = 0;
            }

            InvokeRepeating("countdown", 0f, 1f);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Lose();
            }
        }

        void countdown() {
            if (isComplete) {
                return;
            }

            secondsRemaining--;
            CountdownText.text = "" + secondsRemaining;

            if (secondsRemaining <= 0) {
                win();
            }
        }

        void win() {
            if (isComplete) {
                return;
            }

            isComplete = true;
            FeedbackText.text = "You Win!";

            StartCoroutine("returnToMenuAfterDelay");
        }

        public void Lose() {
            if (isComplete) {
                return;
            }

            isComplete = true;

            Camera.main.backgroundColor = LoseColor;
            
            FeedbackText.text = "You Lose";

            foreach ( Rigidbody2D bodyPart in bodyPartsRB ) {
                Vector3 v = (bodyPart.gameObject.transform.position - ExplosionCenter.position) * Random.Range(800f, 2000f);
                bodyPart.AddForce(v);
                bodyPart.AddTorque(Random.Range(50f, 300f));
            }

            StartCoroutine("returnToMenuAfterDelay");
        }

        IEnumerator returnToMenuAfterDelay() {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
