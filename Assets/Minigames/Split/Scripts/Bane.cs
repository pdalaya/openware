using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Split
{
    public class Bane : MonoBehaviour
    {
        SplitGameManager SplitGameManager;
        float Speed = 3f;
        GameObject Target = null;
        List<GameObject> allPossibleMarines = new List<GameObject>();

        void Awake()
        {
            SplitGameManager = GameObject.Find("Split Game Manager").GetComponent<SplitGameManager>();
            GameObject[] marines = GameObject.FindGameObjectsWithTag("Marine");
            foreach (GameObject marine in marines) {
                allPossibleMarines.Add(marine);
            }
        }

        void Update()
        {
            UpdateTarget();
            Attack();
        }

        void Attack()
        {
            if (Target == null)
            {
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);
            float dist = Vector3.Distance(transform.position, Target.transform.position);
            if (dist < 0.1f)
            {
                SplitGameManager.BaneExploded(gameObject);
            }
        }

        void UpdateTarget()
        {
            float nearestDist = float.MaxValue;
            foreach (GameObject marine in allPossibleMarines)
            {
                if (marine == null) {
                    continue;
                }

                float dist = Vector3.Distance(transform.position, marine.transform.position);
                if (dist < nearestDist)
                {
                    Target = marine;
                    nearestDist = dist;
                }
            }
        }
    }
}