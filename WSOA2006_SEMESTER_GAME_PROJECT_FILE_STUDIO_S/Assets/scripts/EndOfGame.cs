using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfGame : MonoBehaviour
{
    public void Replay()
    {
        SceneManager.LoadScene("GAME");
        Debug.Log("should load scene");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("should quit");
    }

}
