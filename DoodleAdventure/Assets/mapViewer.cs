using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapViewer : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    public float speed = 1;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!cam.enabled)
            return;
        
        Move();
        transform.position=new Vector3(transform.position.x,transform.position.y,-30);
    }

    void Move(){
        Vector2 mousePos = Input.mousePosition;
        mousePos= new Vector2((mousePos.x/Screen.width-0.5f)*2,(mousePos.y/Screen.height-0.5f)*2);
        if(outsideBox(mousePos)){
            transform.position= (Vector2)transform.position + mousePos*speed*Time.deltaTime;
        }
    }
    bool outsideBox(Vector2 mousePos){
        if(Mathf.Abs(mousePos.x)>0.6f || Mathf.Abs(mousePos.y)>0.6f){
            return true;
        }
        return false;
    }
}
