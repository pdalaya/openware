using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BubblePop
{
    public class Bubble : MonoBehaviour
    {
        public Action<int> OnClicked;
        
        [SerializeField] private TextMeshProUGUI _numberText = null;
     
        [Header("Juice")] 
        [SerializeField] private float _easeInDuration = 0.5f;
        [SerializeField] private Ease _easeIn = Ease.Flash;
        
        [SerializeField] private float _easeOutDuration = 0.25f;
        [SerializeField] private Ease _easeOut = Ease.Flash;
        
        private int _number = 0;

        public void Init(int number)
        {
            _number = number;
            _numberText.text = (number + 1).ToString();
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            transform.DOScale(Vector3.one, _easeInDuration).SetEase(_easeIn);
        }

        public void OnButtonClicked()
        {
            transform.DOScale(Vector3.zero, _easeOutDuration).SetEase(_easeOut)
                .OnComplete(() =>
                {
                    OnClicked?.Invoke(_number);  
                    gameObject.SetActive(false);
                });
        }
    }
}