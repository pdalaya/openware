using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeepieUppie {
    public class SoccerBall : MonoBehaviour {

        public KeepieUppieGameManager GameManager;

        private void OnMouseDown() {
            GameManager.DidClickOnSoccerBall();
        }
    }
}
