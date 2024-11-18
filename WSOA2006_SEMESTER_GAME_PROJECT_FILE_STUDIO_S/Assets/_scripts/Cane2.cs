using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cane2 : MonoBehaviour
{
    public List <Transform> Points;


    private void Update()
    {
        Transform Spot = Points[0];
        if ( Spot.position == transform.position )
        {
            Points.Add( Spot );
            Points.RemoveAt( 0 );
        }
        transform.position = Vector3.MoveTowards(transform.position, Spot.position, 5 * Time.deltaTime);
        

    }
}
