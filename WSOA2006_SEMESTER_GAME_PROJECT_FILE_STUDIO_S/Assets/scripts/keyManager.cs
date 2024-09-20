using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyManager : MonoBehaviour
{
    public int keyLevel;
    public Text keyLevelText;

    public void addKeyLevel()
    {
        keyLevel = keyLevel + 1;
        keyLevelText.text = keyLevel.ToString();
    }

    public void decreaseKeyLevel()
    {
        keyLevel = keyLevel - 1;
        keyLevelText.text = keyLevel.ToString();
    }
}
