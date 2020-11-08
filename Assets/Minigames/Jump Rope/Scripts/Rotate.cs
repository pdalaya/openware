using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpRope {
    public class Rotate : MonoBehaviour {

        public float Speed;

        void Update() {
            transform.Rotate(Vector3.left, Speed);
        }
    }
}