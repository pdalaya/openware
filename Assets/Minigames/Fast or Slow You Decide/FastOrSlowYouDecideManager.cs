using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace FastOrSlowYouDecide {
    public class FastOrSlowYouDecideManager : MonoBehaviour {

        public MinigameCompletionHandler MinigameCompletionHandler;
        public TextMeshProUGUI ResultText;
        public TextMeshProUGUI ClockText;

        public AudioClip TenSeconds;
        public AudioClip NineSeconds;
        public AudioClip EightSeconds;
        public AudioClip SevenSeconds;
        public AudioClip SixSeconds;
        public AudioClip FiveSeconds;
        public AudioClip FourSeconds;
        public AudioClip ThreeSeconds;
        public AudioClip TwoSeconds;
        public AudioClip OneSecond;

        public AudioClip LossClip;
        public AudioClip WinClip;

        AudioSource audioSource;

        int secondsRemaining = 10;

        float slowCount = 1.2f;
        float fastCount = 0.8f;
        float regularCount = 1.01f;

        enum Speed { slow, regular, fast }
        Speed currentSpeed;
        bool isComplete = false;

        void Awake() {
            ResultText.text = "";

            float v = Random.Range(0f, 3f);

            if (v < 1f) {
                currentSpeed = Speed.slow;
            } else if (v >= 1f && v < 2f) {
                currentSpeed = Speed.regular;
            } else if (v >= 2f) {
                currentSpeed = Speed.fast;
            }

            InvokeRepeating("Countdown", Delay(), Delay());
            audioSource = GetComponent<AudioSource>();
        }

        float Delay() {
            switch (currentSpeed) {
                case Speed.slow:
                    return slowCount;
                case Speed.regular:
                    return regularCount;
                case Speed.fast:
                    return fastCount;
            }

            Debug.LogError("shouldn't get here");
            return regularCount;
        }

        void Countdown() {
            if (isComplete) {
                return;
            }

            if (secondsRemaining > 9) {
                ClockText.text = "0:" + secondsRemaining;
            } else if (secondsRemaining > 0) {
                ClockText.text = "0:0" + secondsRemaining;
            } else {
                ClockText.text = "0:00";
            }

            if (secondsRemaining <= 0) {
                print("Done!");
                isComplete = true;
                StartCoroutine("handleLoss");
            } else {
                print("Seconds remaining: " + secondsRemaining);

                playCountdownAudio();
                secondsRemaining--;
            }
        }

        void playCountdownAudio() {
            switch (secondsRemaining) {
                case 10:
                    audioSource.PlayOneShot(TenSeconds);
                    break;
                case 9:
                    audioSource.PlayOneShot(NineSeconds);
                    break;
                case 8:
                    audioSource.PlayOneShot(EightSeconds);
                    break;
                case 7:
                    audioSource.PlayOneShot(SevenSeconds);
                    break;
                case 6:
                    audioSource.PlayOneShot(SixSeconds);
                    break;
                case 5:
                    audioSource.PlayOneShot(FiveSeconds);
                    break;
                case 4:
                    audioSource.PlayOneShot(FourSeconds);
                    break;
                case 3:
                    audioSource.PlayOneShot(ThreeSeconds);
                    break;
                case 2:
                    audioSource.PlayOneShot(TwoSeconds);
                    break;
                case 1:
                    audioSource.PlayOneShot(OneSecond);
                    break;
            }
        }

        IEnumerator handleLoss() {
            isComplete = true;
            
            audioSource.PlayOneShot(LossClip);
            yield return new WaitForSeconds(2);
            MinigameCompletionHandler.LoseCallback.Invoke();
        }

        IEnumerator handleWin() {
            isComplete = true;
            
            audioSource.PlayOneShot(WinClip);

            yield return new WaitForSeconds(2);
            MinigameCompletionHandler.WinCallback.Invoke();
        }

        public void GuessSlow() {
            if (isComplete) {
                return;
            }

            switch (currentSpeed) {
                case Speed.slow:
                    ResultText.text = "Correct!";
                    StartCoroutine("handleWin");
                    break;
                case Speed.regular:
                    ResultText.text = "Wrong... the clock was accurate";
                    StartCoroutine("handleLoss");
                    break;
                case Speed.fast:
                    ResultText.text = "Wrong... the clock was fast";
                    StartCoroutine("handleLoss");
                    break;
            }
        }

        public void GuessRegular() {
            if (isComplete) {
                return;
            }

            switch (currentSpeed) {
                case Speed.slow:
                    ResultText.text = "Wrong... the clock was slow";
                    StartCoroutine("handleLoss");
                    break;
                case Speed.regular:
                    ResultText.text = "Correct!";
                    StartCoroutine("handleWin");
                    break;
                case Speed.fast:
                    ResultText.text = "Wrong... the clock was fast";
                    StartCoroutine("handleLoss");
                    break;
            }
        }

        public void GuessFast() {
            if (isComplete) {
                return;
            }

            switch (currentSpeed) {
                case Speed.slow:
                    ResultText.text = "Wrong... the clock was slow";
                    StartCoroutine("handleLoss");
                    break;
                case Speed.regular:
                    ResultText.text = "Wrong... the clock was accurate";
                    StartCoroutine("handleLoss");
                    break;
                case Speed.fast:
                    ResultText.text = "Correct!";
                    StartCoroutine("handleWin");
                    break;
            }
        }
    }
}
