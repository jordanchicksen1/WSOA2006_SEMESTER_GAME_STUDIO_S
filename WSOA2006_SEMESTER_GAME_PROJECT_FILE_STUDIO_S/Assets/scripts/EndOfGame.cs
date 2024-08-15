using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfGame : MonoBehaviour
{
   public void Replay()
    {
        SceneManager.LoadScene("GAME");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
