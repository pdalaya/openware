using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Split
{
    public class SplitGameManager : MonoBehaviour
    {        
        public Sprite MarineUnselected;
        public Sprite MarineSelected;        
        public RectTransform SelectionBox;
        public GameObject xPrefab;
        public GameObject Instructions;
        public TextMeshProUGUI CountdownText;
        public GameObject BaneExplosionPrefab;
        public GameObject BanePrefab;
        public GameObject BaneSpawnMin;
        public GameObject BaneSpawnMax;
        public TextMeshProUGUI BanesText;
        public TextMeshProUGUI MarinesText;
        public GameObject WinText;
        public GameObject LoseText;
        public MinigameCompletionHandler MinigameCompletionHandler;
        
        List<GameObject> SelectedMarines = new List<GameObject>();
        List<GameObject> Marines = new List<GameObject>();
        Dictionary<GameObject, Vector3> SelectedMarineOrders = new Dictionary<GameObject, Vector3>();
        int secondsRemaining = 10;
        Vector2 boxSelectStartPos;
        float MarineSpeed = 3f;
        int banelingCount = -1;
        bool gameOver = false;

        void Awake() {
            SelectionBox.gameObject.SetActive(false);
            foreach (GameObject marine in GameObject.FindGameObjectsWithTag("Marine")) {
                Marines.Add(marine);
            }
            MarinesText.text = "Marines: " + Marines.Count;
            StartCoroutine("Countdown");
            BanesText.text = "Banes: 0";
        }

        void Update() {
            if (gameOver) {
                return;
            }

            if (banelingCount == 0) {
                StartCoroutine("Win");
                gameOver = true;
            }

            if (Marines.Count == 0) {
                StartCoroutine("Lose");
                gameOver = true;
            }

            if (Input.GetMouseButtonDown(0)) {
                boxSelectStartPos = Input.mousePosition;
            } else if (Input.GetMouseButton(0)) {
                UpdateSelectionBox(Input.mousePosition);
            } else if (Input.GetMouseButtonUp(0)) {
                ReleaseSelectionBox();
            }

            if (Input.GetMouseButtonDown(1) && SelectedMarines.Count > 0) {
                Vector3 p = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 8f, Camera.main.nearClipPlane);
                Vector3 pos = Camera.main.ScreenToWorldPoint(p);
                StartCoroutine(SpawnX(pos));

                foreach (GameObject marine in SelectedMarines) {
                    SelectedMarineOrders[marine] = pos;
                }
            }

            List<GameObject> objsToClear = new List<GameObject>();
            foreach (KeyValuePair<GameObject, Vector3> pair in SelectedMarineOrders) {
                float dist = Vector3.Distance(pair.Key.transform.position, pair.Value);
                if (dist < 0.2f) {
                    // The unit arrived at its destination so stop moving it
                    objsToClear.Add(pair.Key);
                } else {
                    // Move the unit closer to its destination
                    pair.Key.transform.position = Vector3.MoveTowards(pair.Key.transform.position, pair.Value, MarineSpeed * Time.deltaTime);
                }
            }
        }

        IEnumerator Countdown() {
            secondsRemaining -= 1;
            CountdownText.text = "" + secondsRemaining;

            if (secondsRemaining == 0) {
                SpawnBanes();
            } else {
                yield return new WaitForSeconds(1);
                StartCoroutine("Countdown");
            }
        }

        void SpawnBanes() {
            banelingCount = 0;
            for (float x = BaneSpawnMin.transform.position.x; x < BaneSpawnMax.transform.position.x; x += 0.6f) {
                Instantiate(BanePrefab, new Vector3(x, BaneSpawnMin.transform.position.y + Random.Range(0f, 2f), BaneSpawnMin.transform.position.z), Quaternion.identity);
                banelingCount += 1;
            }
            BanesText.text = "Banes: " + banelingCount;
        }

        public void DidSelectMarine(GameObject obj) {
            DeselectAllUnits();
            SelectMarine(obj);
        }

        void SelectMarine(GameObject marine) {
            marine.GetComponent<SpriteRenderer>().sprite = MarineSelected;
            SelectedMarines.Add(marine);
        }

        IEnumerator SpawnX(Vector3 pos) {
            GameObject x = Instantiate(xPrefab, pos, Quaternion.identity);
            yield return new WaitForSeconds(1.5f);
            Destroy(x);
        }

        void DeselectAllUnits() {
            foreach (GameObject marine in SelectedMarines) {
                marine.GetComponent<SpriteRenderer>().sprite = MarineUnselected;
            }
            SelectedMarines.Clear();
        }

        void UpdateSelectionBox(Vector2 mousePos) {
            if (!SelectionBox.gameObject.activeInHierarchy) {
                SelectionBox.gameObject.SetActive(true);
            }

            float width = mousePos.x - boxSelectStartPos.x;
            float height = mousePos.y - boxSelectStartPos.y;

            SelectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            SelectionBox.anchoredPosition = boxSelectStartPos + new Vector2(width / 2, height / 2);
        }

        void ReleaseSelectionBox() {
            SelectionBox.gameObject.SetActive(false);

            if (SelectionBox.sizeDelta == Vector2.zero) {
                return;
            }

            Vector2 min = SelectionBox.anchoredPosition - (SelectionBox.sizeDelta / 2);
            Vector2 max = SelectionBox.anchoredPosition + (SelectionBox.sizeDelta / 2);

            DeselectAllUnits();

            foreach (GameObject marine in Marines) {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(marine.transform.position);
                if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y) {
                    SelectMarine(marine);
                }
            }
        }

        public void BaneExploded(GameObject bane) {
            Vector3 pos = bane.transform.position;
            Destroy(bane);
            StartCoroutine(BaneExplosionEffect(pos));
            banelingCount -= 1;
            BanesText.text = "Banes: " + banelingCount;

            // Destroy all marines within a certain radius
            List<GameObject> marinesToKill = new List<GameObject>();
            foreach (GameObject marine in Marines) {
                float dist = Vector3.Distance(marine.transform.position, pos);
                if (dist < 0.5f) {
                    marinesToKill.Add(marine);
                }
            }
            KillMarines(marinesToKill);
        }

        void KillMarines(List<GameObject> marines) {
            for (int x = 0; x < marines.Count; x++) {
                Marines.Remove(marines[x]);
                SelectedMarines.Remove(marines[x]);
                SelectedMarineOrders.Remove(marines[x]);
                Destroy(marines[x]);
            }

            MarinesText.text = "Marines: " + Marines.Count;
        }

        IEnumerator BaneExplosionEffect(Vector3 pos) {
            GameObject e = Instantiate(BaneExplosionPrefab, pos, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            Destroy(e);
        }

        IEnumerator Win() {
            WinText.SetActive(true);
            yield return new WaitForSeconds(2);
            MinigameCompletionHandler.WinCallback.Invoke();
        }

        IEnumerator Lose() {
            LoseText.SetActive(true);
            yield return new WaitForSeconds(2);
            MinigameCompletionHandler.LoseCallback.Invoke();
        }
    }
}
