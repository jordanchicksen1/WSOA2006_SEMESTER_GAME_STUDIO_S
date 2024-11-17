using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour
{
    private string actual_code;
    private string entering_code;

    public bool CodeCorrect;
    void Start()
    {
        entering_code = "";
        actual_code = "1234";
        CodeCorrect = false;
        Debug.Log(entering_code);
    }

    public void enter()
    {
        if (entering_code == actual_code)
        {
            CodeCorrect = true;
            entering_code = "";
            Debug.Log("safeOpened");
        }
        else{Debug.Log("Wrong");}
    }

    public void clear()
    {
        entering_code = "";
    }
    
    public void addOne()
    {
        entering_code = entering_code + "1";
        Debug.Log(entering_code);
    }
    
    public void addTwo()
    {
        entering_code = entering_code + "2";
        Debug.Log(entering_code);
    }
    
    public void addThree()
    {
        entering_code = entering_code + "3";
        Debug.Log(entering_code);
    }
    
    public void addFour()
    {
        entering_code = entering_code + "4";
        Debug.Log(entering_code);
    }
    
    public void addFive()
    {
        entering_code = entering_code + "5";
        Debug.Log(entering_code);
    }
    
    public void addSix()
    {
        entering_code = entering_code + "6";
        Debug.Log(entering_code);
    }
    
    public void addSeven()
    {
        entering_code = entering_code + "7";
        Debug.Log(entering_code);
    }
    
    public void addEight()
    {
        entering_code = entering_code + "8";
        Debug.Log(entering_code);
    }
    
    public void addNine()
    {
        entering_code = entering_code + "9";
        Debug.Log(entering_code);
    }
    
    public void addZero()
    {
        entering_code = entering_code + "0";
        Debug.Log(entering_code);
    }
}
