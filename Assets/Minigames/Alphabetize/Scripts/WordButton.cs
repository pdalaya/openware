using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alphabetize {
    public class WordButton : MonoBehaviour {
        AlphabetizeGameManager GameManager;

        private void Awake() {
            GameManager = GameObject.Find("AlphabetizeGameManager").GetComponent<AlphabetizeGameManager>();
        }

        public void DidTapButton() {
            GameManager.DidTapWord(gameObject);
        }
    }
}