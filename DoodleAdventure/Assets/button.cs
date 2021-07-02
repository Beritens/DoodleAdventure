using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class button : MonoBehaviour , IChangeable
{
    public room room;
    public int index;
    public UnityEvent extra;
    public Sprite onSprite;
    public Sprite offSprite;
    bool active = true;
    [SerializeField]
    AudioSource sound;

   void OnTriggerEnter2D(Collider2D other)
   {
        if(!active)
            return;
        if(other.GetComponent<player>()!= null){
            room.firstActivation(index);
            
            extra.Invoke();
            
        }    
   }
   public void ON(bool on){
       active = on;
       GetComponent<SpriteRenderer>().sprite= on? onSprite:offSprite;
   }

    public void activate()
    {
        ON(false);
    }

    public void deactivate()
    {
        ON(true);
    }

    public void firstactivation()
    {
        ON(false);
        sound.Play();
    }

}
