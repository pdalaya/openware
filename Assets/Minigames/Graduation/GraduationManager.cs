using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Graduation {
    public class GraduationManager : MonoBehaviour {
        public Animator Graduate;
        public RectTransform Diploma;
        public Animator DiplomaAnimator;
        public RectTransform CatchPoint;

        public GameObject WinImage;
        public GameObject WinText;

        public GameObject LoseImage;
        public GameObject LoseText;

        MinigameResult result = MinigameResult.Ongoing;

        Vector3 lastDiplomaPos;
        int diplomaNotMovingCount = 0;

        private void Awake() {
            WinImage.SetActive(false);
            WinText.SetActive(false);
            LoseImage.SetActive(false);
            LoseText.SetActive(false);
        }

        void Update() {
            // Don't let the player do anything once they've lost
            if (result == MinigameResult.Loss) {
                return;
            }

            // If the player does nothing they lose
            if (lastDiplomaPos == Diploma.position && result == MinigameResult.Ongoing) {
                diplomaNotMovingCount++;

                if (diplomaNotMovingCount > 10) {
                    handleLose();
                }
            }

            // If the player taps space, they either catch the diploma and win or miss it and lose
            if (Input.GetKeyDown(KeyCode.Space)) {
               float dist = Vector2.Distance(Diploma.position, CatchPoint.position);
                Debug.Log("dist: " + dist);
                if (dist < 90f) {
                    handleWin();
                } else {
                    handleLose();
                }
            }

            lastDiplomaPos = Diploma.position;
        }

        void handleWin() {
            result = MinigameResult.Win;

            DiplomaAnimator.SetTrigger("Catch");
            WinImage.SetActive(true);
            WinText.SetActive(true);

            StartCoroutine("returnToMenuAfterDelay");
        }

        void handleLose() {
            result = MinigameResult.Loss;

            DiplomaAnimator.SetTrigger("Miss");
            LoseImage.SetActive(true);
            LoseText.SetActive(true);

            StartCoroutine("returnToMenuAfterDelay");
        }

        IEnumerator returnToMenuAfterDelay() {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("MainMenu");
        }

    }
}