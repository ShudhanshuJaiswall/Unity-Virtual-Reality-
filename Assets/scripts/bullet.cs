using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
     private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ZB>()) 
        {
            other.GetComponent<ZB>().Death();
        }     
    }
}
