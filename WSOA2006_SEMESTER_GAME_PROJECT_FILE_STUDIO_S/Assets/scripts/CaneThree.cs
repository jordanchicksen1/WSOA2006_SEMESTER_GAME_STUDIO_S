using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneThree : MonoBehaviour
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
    public GameObject pointK;
    public GameObject pointL;
    
   
    
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
        if (target == pointA.transform)
        {
            Debug.Log("moving to A");
            rb.velocity = new Vector3(0, 0, speed);
        }
        else if (target == pointB.transform || target == pointF.transform || target == pointD.transform)
        {
            rb.velocity = new Vector3(-speed, 0, 0);
        }
        else if(target == pointJ.transform || target == pointL.transform)
        {
            rb.velocity = new Vector3(speed, 0, 0);
        }
        if (target == pointC.transform || target == pointG.transform || target == pointI.transform)
        {
            rb.velocity = new Vector3(0, 0, speed);
        }

        if (target == pointE.transform || target == pointH.transform || target == pointK.transform)
        {
            rb.velocity = new Vector3(0, 0, -speed);
        }

       



        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointA.transform)
        {
            target = pointB.transform;
            currentPoint.transform.Rotate(0, -90f, 0);
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointB.transform)
        {
            
            target = pointC.transform;
            currentPoint.transform.Rotate(0, 90f, 0);
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointC.transform)
        {
            currentPoint.transform.Rotate(0, -90f, 0);
            target = pointD.transform;
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointD.transform)
        {
            target = pointD.transform;
            currentPoint.transform.Rotate(0, -90f, 0);
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointD.transform)
        {
            target = pointE.transform;
            currentPoint.transform.Rotate(0, 360f, 0);
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointE.transform)
        {
            target = pointF.transform;
            currentPoint.transform.Rotate(0, 90f, 0);
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointF.transform)
        {
            target = pointG.transform;
            currentPoint.transform.Rotate(0, 90f, 0);
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointG.transform)
        {
            target = pointH.transform;
            currentPoint.transform.Rotate(0, 180f, 0);
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointH.transform)
        {
            pointF.SetActive(false);
            target = pointI.transform;
            currentPoint.transform.Rotate(0, 180f, 0);
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointI.transform)
        {
            
            target = pointJ.transform;
            currentPoint.transform.Rotate(0, 90f, 0);
        }


        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointJ.transform)
        {

            target = pointK.transform;
            currentPoint.transform.Rotate(0, 90f, 0);
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointK.transform)
        {

            target = pointL.transform;
            currentPoint.transform.Rotate(0, -90f, 0);
        }

        if (Vector3.Distance(currentPoint.position, target.position) < 0.5f && target == pointL.transform)
        {

            target = pointA.transform;
            currentPoint.transform.Rotate(0, -90f, 0);
        }







    }
}
