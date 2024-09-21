using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfGame : MonoBehaviour
{
   public void Replay()
    {
        SceneManager.LoadScene("dayna_fuckaround");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
