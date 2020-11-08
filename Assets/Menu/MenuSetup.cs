using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSetup : MonoBehaviour
{
    private void Awake() {
        restoreDefaults();
    }

    void restoreDefaults() {
        Cursor.visible = true;
        Physics.gravity = new Vector3(0f, -9.8f, 0f);
    }
}
