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

        enum Screen
        {
            MainMenu, HighScoreScoreScreen, HighScoreMinigame, PracticeMenu, PracticeMinigame
        }

        List<string> minigameSceneNames = new List<string>();
        List<string> unplayedMinigameSceneNames = new List<string>();
        Screen currentScreen;
        string currentMinigame;

        void Start()
        {
            minigameSceneNames = Utilities.MinigameScenes();
            StartCoroutine("ShowAndSetupMainMenu");
        }

        IEnumerator ShowAndSetupMainMenu()
        {
            print("ShowAndSetupMainMenu");
            currentScreen = Screen.MainMenu;
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);

            GameObject play = null;
            GameObject playSpecific = null;
            GameObject highScoreText = null;

            while (play == null || playSpecific == null)
            {
                yield return new WaitForEndOfFrame();
                play = GameObject.Find("Play");
                playSpecific = GameObject.Find("Play specific minigames");
                highScoreText = GameObject.Find("High score text");
            }

            play.GetComponent<Button>().onClick.AddListener(DidTapMainMenuPlay);
            playSpecific.GetComponent<Button>().onClick.AddListener(DidTapMainMenuPlaySpecificMinigames);
            highScoreText.GetComponent<TextMeshProUGUI>().text = "High score: " + PlayerPrefs.GetInt("HighScore", 0);
        }

        void DidTapMainMenuPlay()
        {
            StartCoroutine("StartNewHighScoreGame");
        }

        IEnumerator StartNewHighScoreGame()
        {
            currentScreen = Screen.HighScoreScoreScreen;

            unplayedMinigameSceneNames = minigameSceneNames;
            lives = 3;
            score = 0;

            SceneManager.LoadScene("Score and lives", LoadSceneMode.Additive);

            GameObject scoreText = null;

            while (scoreText == null)
            {
                yield return new WaitForEndOfFrame();
                scoreText = GameObject.Find("Score text");
            }

            scoreText.GetComponent<TextMeshProUGUI>().text = "" + score;
            UpdateLifeIcons();

            SceneManager.UnloadSceneAsync("Main Menu");

            yield return new WaitForSeconds(2);
            ContinueHighScoreGame();
        }

        void ContinueHighScoreGame()
        {
            switch (currentScreen)
            {
                case Screen.HighScoreScoreScreen:
                    // If we're dead or out of mini games to play
                    if (lives <= 0 || unplayedMinigameSceneNames.Count == 0)
                    {
                        // Update high score if higher
                        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
                        if (score > currentHighScore)
                        {
                            PlayerPrefs.SetInt("HighScore", score);
                        }

                        // Back to main menu
                        StartCoroutine("ShowAndSetupMainMenu");
                        SceneManager.UnloadSceneAsync("Score and lives");
                    }
                    else
                    {
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

        IEnumerator ShowAndSetupHighScoreMinigame(string sceneName)
        {
            currentScreen = Screen.HighScoreMinigame;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

            GameObject completionHandler = null;

            while (completionHandler == null)
            {
                yield return new WaitForEndOfFrame();
                completionHandler = GameObject.Find("Minigame Completion Handler");
            }

            completionHandler.GetComponent<MinigameCompletionHandler>().WinCallback = DidWinMinigame;
            completionHandler.GetComponent<MinigameCompletionHandler>().LoseCallback = DidLoseMinigame;
        }

        IEnumerator HighScoreFromGameToScoreScreen()
        {
            currentScreen = Screen.HighScoreScoreScreen;
            SceneManager.LoadScene("Score and lives", LoadSceneMode.Additive);

            GameObject scoreText = null;

            while (scoreText == null)
            {
                yield return new WaitForEndOfFrame();
                scoreText = GameObject.Find("Score text");
            }

            scoreText.GetComponent<TextMeshProUGUI>().text = "" + score;
            UpdateLifeIcons();

            SceneManager.UnloadSceneAsync(currentMinigame);

            yield return new WaitForSeconds(2);
            ContinueHighScoreGame();
        }

        void DidWinMinigame()
        {
            print("DidWinMinigame");
            score += 1;
            ContinueHighScoreGame();
        }

        void DidLoseMinigame()
        {
            print("DidLoseMinigame");
            lives -= 1;
            ContinueHighScoreGame();
        }

        void DidCompleteSpecificMinigame()
        {
            StartCoroutine("ShowAndSetupSpecificMiniGameMenu");
        }

        void UpdateLifeIcons()
        {
            GameObject left = GameObject.Find("Life icon left");
            GameObject right = GameObject.Find("Life icon right");
            GameObject center = GameObject.Find("Life icon center");
            GameObject gameOver = GameObject.Find("Game over text");
            GameObject livesTitle = GameObject.Find("Lives title text");

            if (lives <= 0)
            {
                gameOver.SetActive(true);
                livesTitle.SetActive(false);
                left.SetActive(false);
                center.SetActive(false);
                right.SetActive(false);
            }
            else
            {
                gameOver.SetActive(false);
                livesTitle.SetActive(true);

                switch (lives)
                {
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

        void DidTapMainMenuPlaySpecificMinigames()
        {
            print("DidTapMainMenuPlaySpecificMinigames");
            StartCoroutine("ShowAndSetupSpecificMiniGameMenu");
        }

        void BackToMainMenu()
        {
            StartCoroutine("ShowAndSetupMainMenu");
            SceneManager.UnloadSceneAsync("Minigame Menu");
        }

        IEnumerator ShowAndSetupSpecificMiniGameMenu()
        {
            currentScreen = Screen.PracticeMenu;

            // Load menu then wait a frame so it's completed
            SceneManager.LoadScene("Minigame Menu", LoadSceneMode.Additive);

            GameObject alphabetize = null;

            while (alphabetize == null)
            {
                yield return new WaitForEndOfFrame();
                alphabetize = GameObject.Find("Alphabetize");
            }

            // Unload any scene that's not SuperManager or Minigame Menu
            for (int x = 0; x < SceneManager.sceneCount; x++)
            {
                Scene scene = SceneManager.GetSceneAt(x);
                if (scene.name != "SuperManager" && scene.name != "Minigame Menu")
                {
                    SceneManager.UnloadSceneAsync(scene.name);
                }
            }

            yield return new WaitForEndOfFrame();

            GameObject.Find("Back button").GetComponent<Button>().onClick.AddListener(BackToMainMenu);

            // Add new minigame here
            // Here we set the callback on the button for each minigame. Make sure the button name is correct.
            GameObject.Find("Alphabet").GetComponent<Button>().onClick.AddListener(DidTapAlphabetGame);
            alphabetize.GetComponent<Button>().onClick.AddListener(DidTapAlphabetizeGame);
            GameObject.Find("Awp").GetComponent<Button>().onClick.AddListener(DidTapAwpGame);
            GameObject.Find("Button Mash").GetComponent<Button>().onClick.AddListener(DidTapButtonMashGame);
            GameObject.Find("Capsta").GetComponent<Button>().onClick.AddListener(DidTapCapstaGame);
            GameObject.Find("Fally Bird").GetComponent<Button>().onClick.AddListener(DidTapFallyBirdGame);
            GameObject.Find("Fast or Slow You Decide").GetComponent<Button>().onClick.AddListener(DidTapFastOrSlowYouDecideGame);
            GameObject.Find("Field Goal").GetComponent<Button>().onClick.AddListener(DidTapFieldGoalGame);
            GameObject.Find("Graduation").GetComponent<Button>().onClick.AddListener(DidTapGraduationGame);
            GameObject.Find("Jump Rope").GetComponent<Button>().onClick.AddListener(DidTapJumpRopeGame);
            GameObject.Find("Keepie Uppie").GetComponent<Button>().onClick.AddListener(DidTapKeepieUppieGame);
            GameObject.Find("Relax").GetComponent<Button>().onClick.AddListener(DidTapRelaxGame);
            GameObject.Find("Mouse Maze").GetComponent<Button>().onClick.AddListener(DidTapMouseMazeGame);
            GameObject.Find("Bubble Pop").GetComponent<Button>().onClick.AddListener(DidTapBubblePopGame);
            GameObject.Find("Split").GetComponent<Button>().onClick.AddListener(DidTapSplitGame);
        }

        // Specific minigame menu callbacks        
        // Add new minigame here: a function that gets called when a player clicks the new minigame button
        void DidTapAlphabetGame()
        {
            StartCoroutine(ShowAndSetupSpecificMinigame("Alphabet"));
        }

        void DidTapAlphabetizeGame()
        {
            StartCoroutine(ShowAndSetupSpecificMinigame("Alphabetize"));
        }

        void DidTapAwpGame()
        {
            StartCoroutine(ShowAndSetupSpecificMinigame("Awp"));
        }

        void DidTapButtonMashGame()
        {
            StartCoroutine(ShowAndSetupSpecificMinigame("Button Mash"));
        }

        void DidTapCapstaGame() {
            StartCoroutine(ShowAndSetupSpecificMinigame("Capsta"));
        }

        void DidTapFallyBirdGame()
        {
            StartCoroutine(ShowAndSetupSpecificMinigame("Fally Bird"));
        }

        void DidTapFastOrSlowYouDecideGame()
        {
            StartCoroutine(ShowAndSetupSpecificMinigame("Fast or Slow You Decide"));
        }

        void DidTapFieldGoalGame()
        {
            StartCoroutine(ShowAndSetupSpecificMinigame("Field Goal"));
        }

        void DidTapGraduationGame()
        {
            StartCoroutine(ShowAndSetupSpecificMinigame("Graduation"));
        }

        void DidTapJumpRopeGame()
        {
            StartCoroutine(ShowAndSetupSpecificMinigame("Jump Rope"));
        }

        void DidTapKeepieUppieGame()
        {
            StartCoroutine(ShowAndSetupSpecificMinigame("Keepie Uppie"));
        }

        void DidTapMouseMazeGame() {
            StartCoroutine(ShowAndSetupSpecificMinigame("Mouse Maze"));
        }

        void DidTapRelaxGame()
        {
            StartCoroutine(ShowAndSetupSpecificMinigame("Relax"));
        }
        
        void DidTapBubblePopGame()
        {
            StartCoroutine(ShowAndSetupSpecificMinigame("Bubble Pop"));
        }

        void DidTapSplitGame() {
            StartCoroutine(ShowAndSetupSpecificMinigame("Split"));
        }

        IEnumerator ShowAndSetupSpecificMinigame(string sceneName)
        {
            currentMinigame = sceneName;
            currentScreen = Screen.PracticeMinigame;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

            MinigameCompletionHandler completionHandler = null;

            while (completionHandler == null)
            {
                yield return new WaitForEndOfFrame();
                completionHandler = FindObjectOfType<MinigameCompletionHandler>();
            }

            SceneManager.UnloadSceneAsync("Minigame Menu");

            completionHandler.WinCallback = DidCompleteSpecificMinigame;
            completionHandler.LoseCallback = DidCompleteSpecificMinigame;
        }
    }
}
