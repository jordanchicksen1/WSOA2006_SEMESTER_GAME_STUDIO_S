using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CaneScript : MonoBehaviour


{
    public GameObject pointA;
    public GameObject pointB;
    public GameObject pointC;
    public GameObject pointD;
    public GameObject pointE;
    public GameObject pointF;
    public GameObject pointG;
    public GameObject pointH;
    public GameObject pointI;
    public GameObject pointJ;


    private Rigidbody rb;
    private Transform currentPoint;
    public float speed;
    
    private Transform target;
   
    void Start()
    {
       rb = GetComponent<Rigidbody>();
       
       currentPoint = GetComponent<Transform>();

        target = pointA.transform;
       
    }

    // Update is called once per frame
    void Update()
    {
       
        if(target == pointA.transform)
        {
            Debug.Log("moving to A");
            rb.velocity = new Vector3(0, 0, speed);
        }
        else if(target == pointB.transform || target == pointF.transform)
        {
            rb.velocity = new Vector3(-speed, 0, 0);
        }
        if(target == pointC.transform || target == pointG.transform)
        {
            rb.velocity = new Vector3(0, 0, speed);
        }

        if(target == pointD.transform)
        {
            rb.velocity = new Vector3(-speed, 0, 0);
        }

        if (target == pointE.transform || pointH.transform)
        {
            rb.velocity = new Vector3(0, 0, -speed);
        }
        

       
        
        if(Vector3.Distance(currentPoint.position,target.position) < 0.5f && target == pointA.transform)
        {
            target = pointB.transform;
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointB.transform)
        {
            target = pointC.transform;
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointC.transform)
        {
            target = pointD.transform;
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointD.transform)
        {
            target = pointD.transform;
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointD.transform)
        {
            target = pointE.transform;
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointE.transform)
        {
            target = pointF.transform;
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointF.transform)
        {
            target = pointG.transform;
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointG.transform)
        {
            target = pointH.transform;
        }

       
    }
}
