using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    public float time;
    float t;
    bool timerOn;
    public UnityEvent timeOver;
    public Transform pointer;
    public Image circle;
    

    public void startTimer(){
        timerOn=true;
        t= 0;
        display();
    }
    public void stopTimer(){
        timerOn=false;
        t= 0;
        display();
    }

    

    void Update()
    {
        if(timerOn){
            t+= Time.deltaTime;
            display();
            if(t>= time){
                timeOver.Invoke();
                timerOn=false;
            }
            
        }
    }
    void display(){
        float f = Mathf.Clamp01(t/time);
        circle.fillAmount=f;
        pointer.rotation=Quaternion.Euler(0,0, f*360);
    }
}
