using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Awp {
    public class AwpManager : MonoBehaviour {

        public GameObject Crosshair;
        public Camera Cam;
        public AudioClip Gunshot;
        public GameObject BulletHole;

        AudioSource audioSource;

        void Awake() {
            audioSource = GetComponent<AudioSource>();
            Cursor.visible = false;
            StartCoroutine("returnToMenuAfterDelay");
        }

        IEnumerator returnToMenuAfterDelay() {
            yield return new WaitForSeconds(8f);
            SceneManager.LoadScene("MainMenu");
        }

        void Update() {
            // Position crosshair on cursor
            Vector3 crosshairPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 8f, Cam.nearClipPlane);
            Crosshair.transform.position = Cam.ScreenToWorldPoint(crosshairPos);

            Vector3 bulletHoleScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Cam.nearClipPlane);
            Vector3 bulletHoleWorldPos = Cam.ScreenToWorldPoint(bulletHoleScreenPos);

            // Fire
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) {
                audioSource.PlayOneShot(Gunshot);
                GameObject newBulletHole = Instantiate(BulletHole, bulletHoleWorldPos, Quaternion.identity);
                newBulletHole.transform.localScale = new Vector3(0.003296526f, 0.003296526f, 0.003296526f);
            }
        }
    }
}