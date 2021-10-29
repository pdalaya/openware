using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundOvalSetup : MonoBehaviour
{
    public Canvas Canvas;
    public GameObject OvalPrefab;
    public List<Sprite> Ovals;

    void Awake() {
        setupOvals();
    }

    void setupOvals() {
        for (int x = 0; x < 10; x++) {
            GameObject newOval = Instantiate(OvalPrefab);
            int ind = Random.Range(0, Ovals.Count);
            newOval.GetComponent<Image>().sprite = Ovals[ind];
            newOval.transform.SetParent(Canvas.transform);
            newOval.transform.SetSiblingIndex(1);
            newOval.name = "Background Oval";
        }
    }
}
