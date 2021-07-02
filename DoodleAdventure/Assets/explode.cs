using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class explode : MonoBehaviour
{
    public UnityEvent boom;
    public void Explode(){
        boom.Invoke();
    }
}
