using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour , IChangeable
{
    public Color on;
    public Color off;
    public bool startOn=true;
    public void activate()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if(renderer!= null){
            GetComponent<Collider2D>().enabled=!startOn;
            renderer.color= startOn?off:on;
        }
        else{
            gameObject.SetActive(!startOn);
        }
    }

    public void deactivate()
    {
         SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if(renderer!= null){
            GetComponent<Collider2D>().enabled=startOn;
            renderer.color= startOn?on:off;
        }
        else{
            gameObject.SetActive(startOn);
        }
        // SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        // if(renderer!= null){
        //     GetComponent<Collider2D>().enabled=true;
        //     renderer.color= on;
        // }
        // else{
        //     gameObject.SetActive(false);
        // }
    }

    public void firstactivation()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if(renderer!= null){
            GetComponent<Collider2D>().enabled=!startOn;
            renderer.color= startOn?off:on;
        }
        else{
            gameObject.SetActive(!startOn);
        }
    }

}
