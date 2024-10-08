using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndOfGame : MonoBehaviour
{
   public void Replay()
    {
        SceneManager.LoadScene("dayna_fuckaround");
        Debug.Log("it should work");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("it should work");
    }
}
