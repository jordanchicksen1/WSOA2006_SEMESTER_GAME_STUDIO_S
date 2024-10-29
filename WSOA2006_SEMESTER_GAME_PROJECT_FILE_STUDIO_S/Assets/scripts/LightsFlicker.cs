using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsFlicker : MonoBehaviour
{
    public GameObject flickeringLight1;
    public GameObject flickeringLight2;
    public GameObject flickeringLight3;
    public GameObject flickeringLight4;
    public GameObject flickeringLight5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator FlickeringLight1()
    {
        yield return new WaitForSeconds(1.7f);
        flickeringLight1.SetActive(false);
        flickeringLight5.SetActive(true);
        StartCoroutine(FlickeringLight2());
    }
    private IEnumerator FlickeringLight2()
    {
        yield return new WaitForSeconds(1.7f);
        flickeringLight2.SetActive(false);
        flickeringLight1.SetActive(true);
        StartCoroutine(FlickeringLight3());
    }
    private IEnumerator FlickeringLight3()
    {
        yield return new WaitForSeconds(1.7f);
        flickeringLight3.SetActive(false);
        flickeringLight2.SetActive(true);
        StartCoroutine(FlickeringLight4());
    }
    private IEnumerator FlickeringLight4()
    {
        yield return new WaitForSeconds(1.7f);
        flickeringLight4.SetActive(false);
        flickeringLight3.SetActive(true);
        StartCoroutine(FlickeringLight5());
    }
    private IEnumerator FlickeringLight5()
    {
        yield return new WaitForSeconds(1.7f);
        flickeringLight5.SetActive(false);
        flickeringLight4.SetActive(true);
        StartCoroutine(FlickeringLight1());
    }
}
