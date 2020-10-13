using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
  public class MenuManager : MonoBehaviour
  {
      public void DidClickPlay() {
          SceneManager.LoadScene("");
      }
  }
}