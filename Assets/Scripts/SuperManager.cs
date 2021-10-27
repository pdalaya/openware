using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace SuperManager
{
    public class SuperManager : MonoBehaviour
    {
        int lives;
        int score;

        enum Screen {
            MainMenu, HighScoreScoreScreen, HighScoreMinigame, PracticeMenu, PracticeMinigame
        }

        List<string> minigameSceneNames = new List<string>();
        List<string> unplayedMinigameSceneNames = new List<string>();
        Screen currentScreen;
        string currentMinigame;

        void Start()
        {
            PopulateMinigameSceneNames();
            StartCoroutine("ShowAndSetupMainMenu");
        }

        IEnumerator ShowAndSetupMainMenu() {
            print("ShowAndSetupMainMenu");
            currentScreen = Screen.MainMenu;
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);

            GameObject play = null;
            GameObject playSpecific = null;
            GameObject highScoreText = null;

            while (play == null || playSpecific == null) {
                yield return new WaitForEndOfFrame();
                play = GameObject.Find("Play");
                playSpecific = GameObject.Find("Play specific minigames");
                highScoreText = GameObject.Find("High score text");
            }

            play.GetComponent<Button>().onClick.AddListener(DidTapMainMenuPlay);
            playSpecific.GetComponent<Button>().onClick.AddListener(DidTapMainMenuPlaySpecificMinigames);
            highScoreText.GetComponent<TextMeshProUGUI>().text = "High score: " + PlayerPrefs.GetInt("HighScore", 0);
        }

        void DidTapMainMenuPlay() {
            StartCoroutine("StartNewHighScoreGame");
        }

        IEnumerator StartNewHighScoreGame() {
            currentScreen = Screen.HighScoreScoreScreen;

            unplayedMinigameSceneNames = minigameSceneNames;
            lives = 3;
            score = 0;

            SceneManager.LoadScene("Score and lives", LoadSceneMode.Additive);

            GameObject scoreText = null;

            while (scoreText == null) {
                yield return new WaitForEndOfFrame();
                scoreText = GameObject.Find("Score text");
            }

            scoreText.GetComponent<TextMeshProUGUI>().text = "" + score;
            UpdateLifeIcons();

            SceneManager.UnloadSceneAsync("Main Menu");

            yield return new WaitForSeconds(2);
            ContinueHighScoreGame();
        }

        void ContinueHighScoreGame() {
            switch (currentScreen) {
                case Screen.HighScoreScoreScreen:
                    // If we're dead or out of mini games to play
                    if (lives <= 0 || unplayedMinigameSceneNames.Count == 0) {
                        // Update high score if higher
                        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
                        if (score > currentHighScore) {
                            PlayerPrefs.SetInt("HighScore", score);
                        }

                        // Back to main menu
                        StartCoroutine("ShowAndSetupMainMenu");
                        SceneManager.UnloadSceneAsync("Score and lives");
                    } else {
                        // Choose a random minigame to play next
                        int ind = Random.Range(0, unplayedMinigameSceneNames.Count - 1);
                        currentMinigame = unplayedMinigameSceneNames[ind];
                        unplayedMinigameSceneNames.RemoveAt(ind);
                        StartCoroutine(ShowAndSetupHighScoreMinigame(currentMinigame));
                        SceneManager.UnloadSceneAsync("Score and lives");
                    }
                    break;

                case Screen.HighScoreMinigame:
                    // We were in a minigame so return to the score screen
                    StartCoroutine("HighScoreFromGameToScoreScreen");
                    break;
            }
        }

        IEnumerator ShowAndSetupHighScoreMinigame(string sceneName) {
            currentScreen = Screen.HighScoreMinigame;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

            GameObject completionHandler = null;

            while (completionHandler == null) {
                yield return new WaitForEndOfFrame();
                completionHandler = GameObject.Find("Minigame Completion Handler");
            }

            completionHandler.GetComponent<MinigameCompletionHandler>().WinCallback = DidWinMinigame;
            completionHandler.GetComponent<MinigameCompletionHandler>().LoseCallback = DidLoseMinigame;
        }

        IEnumerator HighScoreFromGameToScoreScreen() {
            currentScreen = Screen.HighScoreScoreScreen;
            SceneManager.LoadScene("Score and lives", LoadSceneMode.Additive);

            GameObject scoreText = null;

            while (scoreText == null) {
                yield return new WaitForEndOfFrame();
                scoreText = GameObject.Find("Score text");
            }

            scoreText.GetComponent<TextMeshProUGUI>().text = "" + score;
            UpdateLifeIcons();

            SceneManager.UnloadSceneAsync(currentMinigame);

            yield return new WaitForSeconds(2);
            ContinueHighScoreGame();
        }

        void DidWinMinigame() {
            print("DidWinMinigame");
            score += 1;
            ContinueHighScoreGame();
        }

        void DidLoseMinigame() {
            print("DidLoseMinigame");
            lives -= 1;
            ContinueHighScoreGame();
        }

        void UpdateLifeIcons() {
            GameObject left = GameObject.Find("Life icon left");
            GameObject right = GameObject.Find("Life icon right");
            GameObject center = GameObject.Find("Life icon center");
            GameObject gameOver = GameObject.Find("Game over text");
            GameObject livesTitle = GameObject.Find("Lives title text");

            if (lives <= 0) {
                gameOver.SetActive(true);
                livesTitle.SetActive(false);
                left.SetActive(false);
                center.SetActive(false);
                right.SetActive(false);
            } else {
                gameOver.SetActive(false);
                livesTitle.SetActive(true);

                switch (lives) {
                    case 1:
                        left.SetActive(false);
                        center.SetActive(true);
                        right.SetActive(false);
                        break;
                    case 2:
                        left.SetActive(true);
                        center.SetActive(true);
                        right.SetActive(false);
                        break;
                    case 3:
                        left.SetActive(true);
                        center.SetActive(true);
                        right.SetActive(true);
                        break;
                }
            }
        }

        void DidTapMainMenuPlaySpecificMinigames() {

        }

        IEnumerator ShowAndSetupGameMenu() {
            // Load menu then wait a frame so it's completed
            SceneManager.LoadScene("Minigame Menu", LoadSceneMode.Additive);

            GameObject alphabetize = null;
            while (alphabetize == null) {
                yield return new WaitForEndOfFrame();
                alphabetize = GameObject.Find("Alphabetize");
            }

            // Setup menu button actions
            alphabetize.GetComponent<Button>().onClick.AddListener(DidTapAlphabetizeGame);
            GameObject.Find("Awp").GetComponent<Button>().onClick.AddListener(DidTapAwpGame);
            GameObject.Find("Button Mash").GetComponent<Button>().onClick.AddListener(DidTapButtonMashGame);
            GameObject.Find("Fally Bird").GetComponent<Button>().onClick.AddListener(DidTapFallyBirdGame);
            GameObject.Find("Fast or Slow You Decide").GetComponent<Button>().onClick.AddListener(DidTapFastOrSlowYouDecideGame);
            GameObject.Find("Field Goal").GetComponent<Button>().onClick.AddListener(DidTapFieldGoalGame);
            GameObject.Find("Graduation").GetComponent<Button>().onClick.AddListener(DidTapGraduationGame);
            GameObject.Find("Jump Rope").GetComponent<Button>().onClick.AddListener(DidTapJumpRopeGame);
            GameObject.Find("Keepie Uppie").GetComponent<Button>().onClick.AddListener(DidTapKeepieUppieGame);
            GameObject.Find("Relax").GetComponent<Button>().onClick.AddListener(DidTapRelaxGame);
        }       

        // Game menu callbacks
        void DidTapAlphabetizeGame()
        {
            SceneManager.LoadScene("Alphabetize", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("MainMenu");
        }

        void DidTapAwpGame()
        {
            SceneManager.LoadScene("Awp", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("MainMenu");
        }

        void DidTapButtonMashGame()
        {
            SceneManager.LoadScene("Button Mash", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("MainMenu");
        }

        void DidTapFallyBirdGame()
        {
            SceneManager.LoadScene("Fally Bird", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("MainMenu");
        }

        void DidTapFastOrSlowYouDecideGame()
        {
            SceneManager.LoadScene("Fast or Slow You Decide", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("MainMenu");
        }

        void DidTapFieldGoalGame()
        {
            SceneManager.LoadScene("Field Goal", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("MainMenu");
        }

        void DidTapGraduationGame()
        {
            SceneManager.LoadScene("Graduation", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("MainMenu");
        }

        void DidTapJumpRopeGame()
        {
            SceneManager.LoadScene("Jump Rope", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("MainMenu");
        }

        void DidTapKeepieUppieGame()
        {
            SceneManager.LoadScene("Keepie Uppie", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("MainMenu");
        }

        void DidTapRelaxGame()
        {
            SceneManager.LoadScene("Relax", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("MainMenu");
        }

        void PopulateMinigameSceneNames() {
            minigameSceneNames.Add("Alphabetize");
            minigameSceneNames.Add("Button Mash");
            minigameSceneNames.Add("Fally Bird");
            minigameSceneNames.Add("Fast or Slow You Decide");
            minigameSceneNames.Add("Field Goal");
            minigameSceneNames.Add("Graduation");
            minigameSceneNames.Add("Jump Rope");
            minigameSceneNames.Add("Keepie Uppie");
            minigameSceneNames.Add("Relax");
        }
    }
}