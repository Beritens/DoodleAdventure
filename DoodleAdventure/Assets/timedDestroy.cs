using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timedDestroy : MonoBehaviour
{
    public float time;
    float t;

    // Update is called once per frame
    void Update()
    {
        t+= Time.deltaTime;
        if(t>=time){
            Destroy(gameObject);
        }
    }
}
