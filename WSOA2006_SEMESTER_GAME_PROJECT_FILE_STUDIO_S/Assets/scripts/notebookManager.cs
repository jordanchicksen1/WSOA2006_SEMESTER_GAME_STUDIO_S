using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notebookManager : MonoBehaviour
{
    public int notebookLevel = 0;


    public void addNotebookLevel()
    {
        notebookLevel = notebookLevel + 1;
        Debug.Log("add 1 to notebook level");
    }
}
