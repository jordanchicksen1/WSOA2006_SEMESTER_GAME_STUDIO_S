using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{
    public void Start()
    {
        Cursor.visible = true;
    }

    public void Retry()
    {
        SceneManager.LoadScene("1.5");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
