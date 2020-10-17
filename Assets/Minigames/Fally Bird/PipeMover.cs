using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallyBird {
    public class PipeMover : MonoBehaviour {

        // TODO: set the value that ends up working nicely as the initial value here
        public float PipeSpeed;

        void Update() {
            float amountToMove = PipeSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x - amountToMove, transform.position.y, transform.position.z);
        }
    }
}