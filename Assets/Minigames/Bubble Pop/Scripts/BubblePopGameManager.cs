using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BubblePop
{
    public class BubblePopGameManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int _maxAmount = new Vector2Int(5, 10);
        [SerializeField] private float _startDelay = 1f;
        [SerializeField] private float _endDelay = 2f;
        [SerializeField] private MinigameCompletionHandler _minigameCompletionHandler = null;
        [SerializeField] private TextMeshProUGUI _resultText = null;

        private Bubble[] _bubbles = null;
        private int _count = 0;
        private int _amount = 0;


        private void Awake()
        {
            //lazy to cache :p
            _bubbles = GetComponentsInChildren<Bubble>();
            Array.ForEach(_bubbles, b => b.gameObject.SetActive(false));
            
            _amount = UnityEngine.Random.Range(_maxAmount.x, _maxAmount.y + 1);
            StartCoroutine(Cr_InitBubbles(_amount));
        }

        private IEnumerator Cr_InitBubbles(int amount)
        {
            var bubbleList = new List<Bubble>(_bubbles);

            for (int i = 0; i < Mathf.Min(amount, _bubbles.Length); i++)
            {
                var randomIndex = UnityEngine.Random.Range(0, bubbleList.Count);
                var randomBubble = bubbleList[randomIndex];
                randomBubble.Init(i);
                randomBubble.OnClicked += OnBubbleClicked;
                bubbleList.RemoveAt(randomIndex);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnBubbleClicked(int number)
        {
            if (number != _count)
            {
                OnGameFailed();
                return;
            }

            _count++;

            if (_count == _amount)
                OnGameWon();
        }

        private void OnGameWon()
        {
            ShowResultText(true);
            DOVirtual.DelayedCall(_endDelay, () => _minigameCompletionHandler.WinCallback?.Invoke());
            DOTween.KillAll();
        }

        public void OnGameFailed()
        {
            ShowResultText(false);
            Array.ForEach(_bubbles, b => b.gameObject.SetActive(false));
            DOVirtual.DelayedCall(_endDelay, () => _minigameCompletionHandler.LoseCallback?.Invoke());
        }

        private void ShowResultText(bool isWin)
        {
            _resultText.text = isWin ? "You Win!" : "You Loose!";
            _resultText.color = isWin ? Color.green : Color.red;
            _resultText.gameObject.SetActive(true);
        }
    }
}