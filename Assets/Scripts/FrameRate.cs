using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu {
    public class FrameRate : MonoBehaviour {
        private void Awake() {
            Application.targetFrameRate = 60;
        }
    }
}