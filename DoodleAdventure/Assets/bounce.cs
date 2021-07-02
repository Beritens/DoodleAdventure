using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounce : MonoBehaviour
{
    public Transform target;
    public float force = 1;
    AudioSource source;
    void Start()
    {
        source = GetComponentInChildren<AudioSource>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<player>()!=null){
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = force*(target.position-transform.position).normalized;
            rb.angularVelocity = 0;
            GetComponent<Animation>().Play();
            source.Play();
        }
    }
}
