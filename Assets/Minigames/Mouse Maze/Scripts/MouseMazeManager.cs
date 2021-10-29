using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MouseMaze {
    class Map {
        public Vector2 StartButtonPos;
        public Vector2 WinLoseTextPos;
        public Vector2 WinAreaPos;
        public int WallNumber;

        public Map(Vector2 startButtonPos, Vector2 winLoseTextPos, Vector2 winAreaPos, int wallNumber) {
            StartButtonPos = startButtonPos;
            WinLoseTextPos = winLoseTextPos;
            WinAreaPos = winAreaPos;
            WallNumber = wallNumber;
        }
    }

    public class MouseMazeManager : MonoBehaviour
    {
        public MinigameCompletionHandler MinigameCompletionHandler;
        public GameObject Hero;
        public Camera Cam;
        public GameObject StartButton;
        public GameObject WinText;
        public GameObject LoseText;
        public GameObject WinArea;

        bool didStart = false;
        bool gameOver = false;
        List<Map> maps = new List<Map>();
        GameObject currentWall = null;

        void Awake() {
            Cursor.visible = false;
            WinText.SetActive(false);
            LoseText.SetActive(false);
            PopulateMaps();
            SetupRandomMap();
        }

        void PopulateMaps() {
            maps.Add(new Map(new Vector2(-330, 330), new Vector2(219, 303), new Vector2(4.125f, -3.112f), 1));
            maps.Add(new Map(new Vector2(-235.5f, -225.8f), new Vector2(223.1f, 32.7f), new Vector2(3.09f, 3.91f), 2));
            maps.Add(new Map(new Vector2(85.5f, -86.7f), new Vector2(0f, -297.3f), new Vector2(-3.37f, 4.18f), 3));
            maps.Add(new Map(new Vector2(-312.7f, 313.8f), new Vector2(-193.3f, -296.4f), new Vector2(3.87f, -2.909f), 4));
        }

        void SetupRandomMap() {
            Map map = maps[Random.Range(0, maps.Count)];
            StartButton.GetComponent<RectTransform>().anchoredPosition = map.StartButtonPos;
            WinText.GetComponent<RectTransform>().anchoredPosition = map.WinLoseTextPos;
            LoseText.GetComponent<RectTransform>().anchoredPosition = map.WinLoseTextPos;
            WinArea.transform.position = map.WinAreaPos;
            WinArea.SetActive(false);

            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach (GameObject wall in walls) {
                if (wall.name == ("Walls " + map.WallNumber)) {
                    wall.SetActive(true);
                    currentWall = wall;
                } else {
                    wall.SetActive(false);
                }
            }

            currentWall.GetComponent<PolygonCollider2D>().enabled = false;
        }

        void Update()
        {
            if (gameOver) {
                return;
            }

            Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 8f, Cam.nearClipPlane);
            Hero.transform.position = Cam.ScreenToWorldPoint(pos);
        }

        public void DidClickStart() {
            didStart = true;
            StartButton.SetActive(false);
            currentWall.GetComponent<PolygonCollider2D>().enabled = true;
            WinArea.SetActive(true);
        }

        public void DidCollideWithWall() {
            if (!didStart) {
                return;
            }

            StartCoroutine("Lose");
        }

        public void DidCollideWithWin() {
            if (!didStart) {
                return;
            }

            StartCoroutine("Win");
        }

        IEnumerator Win() {
            WinText.SetActive(true);
            gameOver = true;
            GameObject.Destroy(Hero);
            Cursor.visible = true;

            yield return new WaitForSeconds(2f);
            MinigameCompletionHandler.WinCallback.Invoke();
        }

        IEnumerator Lose() {
            LoseText.SetActive(true);
            gameOver = true;
            GameObject.Destroy(Hero);
            Cursor.visible = true;

            yield return new WaitForSeconds(2f);
            MinigameCompletionHandler.LoseCallback.Invoke();
        }
    }
}
