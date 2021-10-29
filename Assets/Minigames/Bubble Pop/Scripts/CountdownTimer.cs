using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace BubblePop
{
    public class CountdownTimer : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onTimerEnd = null;
        [SerializeField] private float _initialDelay = 1f;
        [SerializeField] private int _seconds = 10;
        [SerializeField] private TextMeshProUGUI _text = null;

        private void Start()
        {
            DOVirtual.Int(_seconds, 0, _seconds, (count) =>
            {
                _text.text = count.ToString();
                
                if (count == 0)
                    _onTimerEnd?.Invoke();
                
            }).SetEase(Ease.Linear).SetDelay(_initialDelay);
        }
    }
}