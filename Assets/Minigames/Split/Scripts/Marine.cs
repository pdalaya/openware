using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Split
{
    public class Marine : MonoBehaviour
    {
        SplitGameManager SplitGameManager;

        void Awake() {
            SplitGameManager = GameObject.Find("Split Game Manager").GetComponent<SplitGameManager>();
        }

        private void OnMouseDown() {
            SplitGameManager.DidSelectMarine(gameObject);
        }
    }
}
