using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpRope {
    public class RopeCollider : MonoBehaviour {
        GameManager gameManager;

        private void Awake() {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void OnTriggerEnter(Collider other) {
            gameManager.HandleLose();
        }
    }
}