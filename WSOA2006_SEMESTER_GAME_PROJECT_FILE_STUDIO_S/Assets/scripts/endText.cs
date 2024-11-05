using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endText : MonoBehaviour
{
    public GameObject firstText;
    public GameObject secondText;
    public GameObject thirdText;
    public GameObject fourthText;
    public GameObject fifthText;
    public GameObject sixthText;
    public GameObject seventhText;
    public GameObject eighthText;
    public GameObject paperBackdrop;
    void Start()
    {
        StartCoroutine(StartRollingText());
    }

    private IEnumerator StartRollingText()
    {
        paperBackdrop.SetActive(true);
        yield return new WaitForSeconds(1); 
        firstText.SetActive(true);
        yield return new WaitForSeconds(9f);
        firstText.SetActive(false);
        secondText.SetActive(true);
        yield return new WaitForSeconds(12f);
        secondText.SetActive(false);
        thirdText.SetActive(true);
        yield return new WaitForSeconds(11f);
        thirdText.SetActive(false);
        fourthText.SetActive(true);
        yield return new WaitForSeconds(14f);
        fourthText.SetActive(false);
        fifthText.SetActive(true);
        yield return new WaitForSeconds(12f);
        fifthText.SetActive(false);
        sixthText.SetActive(true);
        yield return new WaitForSeconds(12f);
        sixthText.SetActive(false);
        paperBackdrop.SetActive(false);
        seventhText.SetActive(true);
        yield return new WaitForSeconds(14f);
        seventhText.SetActive(false);
        eighthText.SetActive(true);
    }
    
}
