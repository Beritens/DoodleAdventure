using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    Manager manager;
    public Transform target;
    void Awake()
    {
        manager = GameObject.FindObjectOfType<Manager>();
    }
    // Update is called once per frame
    void Update()
    {
        if(target ==null)
            return;
        //Vector3 target = manager.getCurrentRoom().transform.position;
        Vector3 pos = new Vector3(target.position.x, target.position.y, -10);
        transform.position=Vector3.Lerp(transform.position,pos,Time.deltaTime*10);
    }
    public void changeSize(float size){
        GetComponent<Camera>().orthographicSize=size;
    }
}
