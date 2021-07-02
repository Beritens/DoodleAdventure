using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class savePoint : MonoBehaviour
{
    public int room;
    int index;
    bool active = false;
    Manager manager;
    [SerializeField]
    Color highlighted;
    [SerializeField]
    Color normal;
    SpriteRenderer render;
    void Start()
    {
        manager = GameObject.FindObjectOfType<Manager>();
        
    }


    public void init(int i){
        render=GetComponent<SpriteRenderer>();
        index = i;
        active = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name+"savePoint"+i.ToString())==1;
        if(active){
            render.color=normal;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag != "Player")
            return;
        active=true;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name+"savePoint"+index.ToString(),1);
        manager.setLastSavePoint(index);
        render.color=normal;
    }
    void OnMouseEnter()
    {
        if(active&&manager.mapOpen){
            render.color=highlighted;
        }
        
    
    }
    void OnMouseExit()
    {
        if(active){
            render.color=normal;
        }
    }
    void OnMouseDown()
    {
        if(active){
            manager.ClickOnSavePoint(index);
            render.color=normal;
        }
    }
}
