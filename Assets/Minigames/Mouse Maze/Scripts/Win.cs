using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MouseMaze {
    public class Win : MonoBehaviour
    {
        public MouseMazeManager MouseMazeManager;

        private void OnCollisionEnter2D(Collision2D other) {
            MouseMazeManager.DidCollideWithWin();
        }
    }
}
