using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hostages : MonoBehaviour
{
    public int hostages;
    public Text hostagesText;

    public void addHostageNumber()
    {
        hostages = hostages + 1;
        hostagesText.text = hostages.ToString();
    }
}
