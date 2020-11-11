using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeepieUppie {
    public class Ground : MonoBehaviour {

        public KeepieUppieGameManager GameManager;

        private void OnTriggerEnter2D(Collider2D collision) {
            GameManager.BallDidHitGround();
        }
    }
}