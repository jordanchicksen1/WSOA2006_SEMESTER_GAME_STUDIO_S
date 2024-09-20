using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startScreen : MonoBehaviour
{
    public GameObject camera1;
    public void Play()
    {
        camera1.transform.position = new Vector3(685f, 1f, -579.9f);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void next1()
    {
        camera1.transform.position = new Vector3(1858f, 1f, -579.9f);
    }

    public void next2()
    {
        SceneManager.LoadScene("dayna_fuckaround");
    }
}
