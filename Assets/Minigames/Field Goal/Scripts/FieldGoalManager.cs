using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FieldGoal
{
    enum GameState
    {
        Aim,
        Result
    }

    public class FieldGoalManager : MonoBehaviour
    {
        public MinigameCompletionHandler MinigameCompletionHandler;

        public Transform WindArrowTransform;
        public Transform AimArrowTransform;
        public Animator FootballAnimator;

        public List<GameObject> WinObjects;
        public List<GameObject> InstructionObjects;

        public List<GameObject> LoseObjects;

        float windPower;
        bool windIsLeft;

        GameState state;

        private void Awake() {
            state = GameState.Aim;
            
            foreach (GameObject instruction in InstructionObjects) {
                instruction.SetActive(true);
            }

            foreach (GameObject win in WinObjects) {
                win.SetActive(false);
            }

            foreach (GameObject win in LoseObjects) {
                win.SetActive(false);
            }

            setupWind();
        }

        private void setupWind() {
            float windPower = Random.Range(0f, 180f) - 90f;
            WindArrowTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, windPower));
        }

        private void Update() {
           if (state == GameState.Aim) {
                AimArrowTransform.Rotate(new Vector3(0f, 0f, -4f));
                if (Input.anyKeyDown) {
                    state = GameState.Result;
                    kick();
                }
           }
        }

        void kick() {
            // First convert each angle to be between 0-180 (positive or negative) of the y+ axis
            float aimZ = AimArrowTransform.eulerAngles.z > 180f ? -360f + AimArrowTransform.eulerAngles.z : AimArrowTransform.eulerAngles.z;
            float windZ = WindArrowTransform.eulerAngles.z > 180f ? -360f + WindArrowTransform.eulerAngles.z : WindArrowTransform.eulerAngles.z;
            float angle = aimZ + windZ;

            print("aimZ: " + aimZ);
            print("windZ: " + windZ);
            print("angle: " + angle);

            // Then convert the angle to our blend range 0-1 where 0.5 is right down the middle
            if (angle < 0) {
                float z = Mathf.Max(-90f, angle);
                z /= (90f * 2f);
                float result = 0.5f + Mathf.Abs(z);
                FootballAnimator.SetFloat("Blend", result);
            } else {
                float z = Mathf.Min(90f, angle);
                z /= (90f * 2f);
                float result = 0.5f - z;
                FootballAnimator.SetFloat("Blend", result);
            }

            FootballAnimator.SetTrigger("Kick");

            // Then handle if the kick was good or not
            if (Mathf.Abs(angle) <= 46f) {
                print("WIN");
                StartCoroutine("handleWin");
            } else {
                print("LOSE");
                StartCoroutine("handleLose");
            }
        }

        IEnumerator handleWin() {
            yield return new WaitForSeconds(2.5f);

            foreach (GameObject instruction in InstructionObjects) {
                instruction.SetActive(false);
            }

            foreach (GameObject win in WinObjects) {
                win.SetActive(true);
            }

            yield return new WaitForSeconds(2f);
            MinigameCompletionHandler.WinCallback.Invoke();
        }

        IEnumerator handleLose() {
            yield return new WaitForSeconds(2.5f);

            foreach (GameObject instruction in InstructionObjects) {
                instruction.SetActive(false);
            }

            foreach (GameObject lose in LoseObjects) {
                lose.SetActive(true);
            }

            yield return new WaitForSeconds(2f);
            MinigameCompletionHandler.LoseCallback.Invoke();
        }
    }
}
