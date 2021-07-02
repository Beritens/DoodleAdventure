using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charge : MonoBehaviour
{
    public bool charged = false;
    public GameObject aura;
    public GameObject explosion;
    // Start is called before the first frame update
    public void Charge(){
        charged = true;
        aura.SetActive(true);
    }
    public void Discharge(){
        charged = false;
        
        aura.SetActive(false);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<explode>()!=null && charged){
            other.gameObject.GetComponent<explode>().Explode();
            GameObject.Instantiate(explosion,transform.position,Quaternion.identity);
            Discharge();
        }
    }
}
