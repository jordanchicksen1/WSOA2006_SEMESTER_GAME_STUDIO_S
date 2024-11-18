using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Safe : MonoBehaviour
{
    [SerializeField] private FirstPersonControls FPCman;
    private string actual_code;
    private string entering_code;

    [SerializeField]
    private Animator safeDoor;
    
    public bool CodeCorrect;

    public GameObject SafeKeypad;
    public TextMeshProUGUI ShowCodeText;
    
    void Start()
    {
        entering_code = "";
        actual_code = "2009";
        CodeCorrect = false;
        Debug.Log(entering_code);
        ShowCodeText.text = entering_code;
    }

    public void ShowKeypad()
    {
        SafeKeypad.SetActive(true);
        Cursor.visible = true;
    }

    public void HideKeypad()
    {
        SafeKeypad.SetActive(false);
        entering_code = "";
        ShowCodeText.text = entering_code;
        Cursor.visible = false;
    }
    
    public void enter()
    {
        if (entering_code == actual_code)
        {
            CodeCorrect = true;
            entering_code = "";
            ShowCodeText.text = entering_code;
            HideKeypad();
            safeDoor.Play("SafeDoor", 0, 0.0f);
            FPCman.CorrectSafeCombination();
        }
        else
        {
            entering_code = "";
            ShowCodeText.text = entering_code;
            FPCman.WrongSafeCombination();
        }
    }

    public void clear()
    {
        entering_code = "";
        ShowCodeText.text = entering_code;
        
    }
    
    public void addOne()
    {
        if (entering_code.Length < 4)
        {
            entering_code += "1";
            ShowCodeText.text = entering_code;
            Debug.Log(entering_code);
        }
        else
        {
            return;
        }
    }
    
    public void addTwo()
    {
        if (entering_code.Length < 4)
        {
            entering_code = entering_code + "2";
            ShowCodeText.text = entering_code;
            Debug.Log(entering_code);
        }
        else
        {
            return;
        }
    }
    
    public void addThree()
    {
        if (entering_code.Length < 4)
        {
            entering_code = entering_code + "3";
            ShowCodeText.text = entering_code;
            Debug.Log(entering_code);
        }
        else
        {
            return;
        }
    }
    
    public void addFour()
    {
        if (entering_code.Length < 4)
        {
            entering_code = entering_code + "4";
            ShowCodeText.text = entering_code;
            Debug.Log(entering_code);
        }
        else
        {
            return;
        }
    }
    
    public void addFive()
    {
        if (entering_code.Length < 4)
        {
            entering_code = entering_code + "5";
            ShowCodeText.text = entering_code;
            Debug.Log(entering_code);
        }
        else
        {
            return;
        }
    }
    
    public void addSix()
    {
        if (entering_code.Length < 4)
        {
            entering_code = entering_code + "6";
            ShowCodeText.text = entering_code;
            Debug.Log(entering_code);
        }
        else
        {
            return;
        }
    }
    
    public void addSeven()
    {
        if (entering_code.Length < 4)
        {
            entering_code = entering_code + "7";
            ShowCodeText.text = entering_code;
            Debug.Log(entering_code);
        }
        else
        {
            return;
        }
    }
    
    public void addEight()
    {
        if (entering_code.Length < 4)
        {
            entering_code = entering_code + "8";
            ShowCodeText.text = entering_code;
            Debug.Log(entering_code);
        }
        else
        {
            return;
        }
    }
    
    public void addNine()
    {
        if (entering_code.Length < 4)
        {
            entering_code = entering_code + "9";
            ShowCodeText.text = entering_code;
            Debug.Log(entering_code);
        }
        else
        {
            return;
        }
    }
    
    public void addZero()
    {
        if (entering_code.Length < 4)
        {
            entering_code = entering_code + "0";
            ShowCodeText.text = entering_code;
            Debug.Log(entering_code);
        }
        else
        {
            return;
        }
    }
}
