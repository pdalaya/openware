using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CapstaGameManager : MonoBehaviour
{
    public GameObject PositiveFeedback;
    public GameObject NegativeFeedback;
    public TextMeshProUGUI QuantityText;
    public TextMeshProUGUI PriceText;
    public GameObject Error;
    public Image CaptchaImage;
    public TMP_InputField InputField;
    public TextMeshProUGUI CountdownText;
    public GameObject WinText;
    public GameObject LoseText;
    public MinigameCompletionHandler MinigameCompletionHandler;
    public GameObject Clippy;

    float price = 79.99f;
    int quantity = 25;
    string capstaText = "";
    string lastInput;
    int secondsRemaining = 11;
    bool gameOver = false;

    void Start() {
        Error.SetActive(false);
        UpdateQuantityAndPrice();
        SetupCapsta();
        PositiveFeedback.SetActive(false);
        NegativeFeedback.SetActive(false);
        StartCoroutine("Countdown");
        WinText.SetActive(false);
        LoseText.SetActive(false);
        Clippy.SetActive(false);
    }

    void Update() {
        if (gameOver) {
            return;
        }

        if (Input.anyKeyDown) {
            ValidateInput();
        }
    }

    IEnumerator Countdown() {
        if (!gameOver) {
            secondsRemaining -= 1;
            CountdownText.text = "" + secondsRemaining;

            if (secondsRemaining <= 0) {
                StartCoroutine("Lose");
            }

            yield return new WaitForSeconds(1);
            StartCoroutine("Countdown");
        }
    }

    IEnumerator Win() {
        gameOver = true;
        WinText.SetActive(true);
        yield return new WaitForSeconds(2);
        MinigameCompletionHandler.WinCallback.Invoke();
    }

    IEnumerator Lose() {
        gameOver = true;
        LoseText.SetActive(true);
        yield return new WaitForSeconds(2);
        MinigameCompletionHandler.LoseCallback.Invoke();
    }

    void ValidateInput() {
        PositiveFeedback.SetActive(false);
        NegativeFeedback.SetActive(false);

        string text = InputField.text.Trim();

        if (text == lastInput || text == "") {
            return;
        }

        if (text.Length == 2 && lastInput.Length == 1) {
            Clippy.SetActive(true);
        }

        lastInput = text;

        if (text == capstaText) {
            StartCoroutine("Win");
        } else if (capstaText.StartsWith(text)) {
            PositiveFeedback.SetActive(true);
        } else {
            NegativeFeedback.SetActive(true);
        }
    }

    void SetupCapsta() {
        Object[] objs = Resources.LoadAll("Capstas", typeof(Sprite));
        int ind = Random.Range(0, objs.Length);
        CaptchaImage.overrideSprite = (Sprite)objs[ind];
        capstaText = objs[ind].name;
    }

    void UpdateQuantityAndPrice() {
        QuantityText.text = "" + quantity;
        PriceText.text = "$" + string.Format("{0:.##}", quantity * price);
    }

    public void IncreaseQuantity() {
        if (quantity > 100000) {
            return;
        }

        quantity += 1;
        UpdateQuantityAndPrice();
    }

    public void DecreaseQuantity() {
        if (quantity <= 0) {
            return;
        }

        quantity -= 1;
        UpdateQuantityAndPrice();
    }

    public void Buy() {
        StartCoroutine("ShowError");
    }

    IEnumerator ShowError () {
        Error.SetActive(true);
        yield return new WaitForSeconds(2);
        Error.SetActive(false);
    }
}
