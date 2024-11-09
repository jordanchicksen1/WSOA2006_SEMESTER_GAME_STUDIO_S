using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class run : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    [SerializeField] private Transform target;
    public float speed = 3.0f;
    // Update is called once per frame
    void Update()
    {
        var step =  speed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
