﻿using System.Collections;
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
                Text.text = "Click the ball to keep it off the ground\n\nReady?";
                HelperPlatform.SetActive(true);
            } else if (secondsRemaining > 0) {
                HelperPlatform.SetActive(false);
                Text.text = "" + secondsRemaining;
            } else if (secondsRemaining <= 0) {
                Text.text = "You Win!";
                isComplete = true;
                StartCoroutine("returnToMenuAfterDelay");
            }

            secondsRemaining--;
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
            StartCoroutine("returnToMenuAfterDelay");
        }

        IEnumerator returnToMenuAfterDelay() {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}