using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class keyManager : MonoBehaviour
{
    public int keyLevel;
    public TextMeshProUGUI keyLevelText;

    public void addKeyLevel()
    {
        keyLevel++;
        keyLevelText.text = keyLevel.ToString();
    }

    public void decreaseKeyLevel()
    {
        keyLevel--;
        keyLevelText.text = keyLevel.ToString();
    }
    
    
}
