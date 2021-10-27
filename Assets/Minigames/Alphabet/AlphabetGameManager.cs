using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Alphabet
{
    public class AlphabetGameManager : MonoBehaviour
    {
        public MinigameCompletionHandler MinigameCompletionHandler;
        public TMP_InputField InputField;
        public GameObject WinText;
        public GameObject LoseText;
        public TextMeshProUGUI CountdownText;

        public GameObject PositiveFeedbackImage;
        public GameObject PositiveFeedbackText;

        public GameObject NegativeFeedbackImage;
        public GameObject NegativeFeedbackText;

        int secondsRemaining = 13;
        bool gameIsOver = false;
        string fullAlphabet = "abcdefghijklmnopqrstuvwxyz";
        string lastText = "";

        void Start()
        {
            StartCoroutine("Countdown");
            WinText.SetActive(false);
            LoseText.SetActive(false);
        }

        void Update()
        {
            if (gameIsOver)
            {
                return;
            }

            if (Input.anyKeyDown)
            {
                ValidateInput();
            }
        }

        void ValidateInput()
        {
            string text = InputField.text.Trim();

            if (text == "")
            {
                PositiveFeedbackImage.SetActive(false);
                PositiveFeedbackText.SetActive(false);

                NegativeFeedbackImage.SetActive(false);
                NegativeFeedbackText.SetActive(false);
                return;
            }

            if (text == lastText)
            {
                return;
            }

            lastText = text;

            PositiveFeedbackImage.SetActive(false);
            PositiveFeedbackText.SetActive(false);

            NegativeFeedbackImage.SetActive(false);
            NegativeFeedbackText.SetActive(false);

            if (text == fullAlphabet)
            {
                StartCoroutine("Win");
            }
            else if (fullAlphabet.StartsWith(text))
            {
                PositiveFeedbackImage.SetActive(true);
                PositiveFeedbackText.SetActive(true);
            }
            else
            {
                NegativeFeedbackImage.SetActive(true);
                NegativeFeedbackText.SetActive(true);
            }
        }

        IEnumerator Countdown()
        {
            yield return new WaitForSeconds(0);

            if (!gameIsOver)
            {
                secondsRemaining -= 1;
                CountdownText.text = "" + secondsRemaining;

                if (secondsRemaining <= 0)
                {
                    StartCoroutine("Lose");
                }
                else
                {
                    yield return new WaitForSeconds(1);
                    StartCoroutine("Countdown");
                }
            }

        }

        IEnumerator Lose()
        {
            yield return new WaitForSeconds(0f);

            if (!gameIsOver)
            {
                gameIsOver = true;
                LoseText.SetActive(true);
                yield return new WaitForSeconds(2f);
                MinigameCompletionHandler.LoseCallback.Invoke();
            }
        }

        IEnumerator Win()
        {
            yield return new WaitForSeconds(0f);

            if (!gameIsOver)
            {
                gameIsOver = true;
                WinText.SetActive(true);
                yield return new WaitForSeconds(2f);
                MinigameCompletionHandler.WinCallback.Invoke();
            }
        }
    }
}

