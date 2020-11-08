using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Awp {
    public class AwpManager : MonoBehaviour {

        public GameObject Crosshair;
        public Camera Cam;
        public AudioClip Gunshot;
        public GameObject BulletHole;
        public TextMeshProUGUI CountdownText;

        public GameObject WinUI;
        public GameObject LoseUI;

        AudioSource audioSource;
        float lastShotTimestamp = 0f;
        int remainingEnemies = 2;
        bool didLose = false;
        int secondsRemaining = 10;

        void Awake() {
            audioSource = GetComponent<AudioSource>();
            Cursor.visible = false;
            WinUI.SetActive(false);
            LoseUI.SetActive(false);

            InvokeRepeating("countdown", 0, 1);
        }

        void countdown() {
            CountdownText.text = "" + secondsRemaining;

            if (secondsRemaining <= 0 && !didLose) {
                handleLose();
            } else {
                secondsRemaining--;
            }
        }

        void Update() {
            // Position crosshair on cursor
            Vector3 crosshairPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 8f, Cam.nearClipPlane);
            Crosshair.transform.position = Cam.ScreenToWorldPoint(crosshairPos);

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) {
                attemptToFire();
            }
        }

        void attemptToFire() {
            if (didLose) { return; }

            // Gun is on cooldown
            if (lastShotTimestamp != 0 && ((Time.time - lastShotTimestamp) < 1.4f)) {
                return;
            }

            lastShotTimestamp = Time.time;

            Vector3 bulletHoleScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Cam.nearClipPlane);
            Vector3 bulletHoleWorldPos = Cam.ScreenToWorldPoint(bulletHoleScreenPos);

            audioSource.PlayOneShot(Gunshot);
            GameObject newBulletHole = Instantiate(BulletHole, bulletHoleWorldPos, Quaternion.identity);
            newBulletHole.transform.localScale = new Vector3(0.003296526f, 0.003296526f, 0.003296526f);
        }

        public void DidShootEnemy(GameObject enemyObj) {
            StartCoroutine(handleShootEnemy(enemyObj));
        }

        IEnumerator handleShootEnemy(GameObject enemyObj) {
            // wait a frame to make sure we do this after attemptToFire
            yield return new WaitForEndOfFrame();

            float diff = Time.time - lastShotTimestamp;

            // Prevent enemy from dying when they get clicked and the gun is on cooldown
            if (diff == 0f) {
                enemyObj.GetComponent<Animator>().SetTrigger("Die");
                enemyObj.GetComponent<PolygonCollider2D>().enabled = false;

                remainingEnemies -= 1;

                if (remainingEnemies <= 0) {
                    handleWin();
                }
            }
        }

        void handleWin() {
            WinUI.SetActive(true);
            StartCoroutine("returnToMenuAfterDelay");
        }

        void handleLose() {
            didLose = true;
            LoseUI.SetActive(true);
            StartCoroutine("returnToMenuAfterDelay");
        }

        IEnumerator returnToMenuAfterDelay() {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}