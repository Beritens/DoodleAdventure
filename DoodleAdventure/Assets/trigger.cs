using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class trigger : MonoBehaviour
{

    public UnityEvent eve;


   void OnTriggerEnter2D(Collider2D other)
   {
        if(other.tag=="Player"){
            
            eve.Invoke();
            
        }
        
            
   }
}
