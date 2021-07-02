using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    public Transform otherPortal;
    public AudioSource source;
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.gameObject.tag=="Player"){
    //         Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
    //         Vector2 relPos = transform.InverseTransformPoint(rb.position);
    //         //float angle = Vector2.SignedAngle(transform.right,otherPortal.right);
    //         Vector2 vel = transform.InverseTransformDirection(rb.velocity);
    //         vel.y = -vel.y;
    //         rb.velocity= otherPortal.TransformDirection(vel);
    //         rb.position = otherPortal.TransformPoint(relPos)+otherPortal.up;
            
            


            
    //     }
    // }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag=="Player"){
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            player player= other.gameObject.GetComponent<player>();
            Vector2 relPos = transform.InverseTransformPoint(rb.position);
            relPos.y = 1.2f;
            //float angle = Vector2.SignedAngle(transform.right,otherPortal.right);
            Vector2 vel = transform.InverseTransformDirection(player.Velocity());
            vel.y = -vel.y;
            rb.velocity= otherPortal.TransformDirection(vel);
            rb.angularVelocity = player.AngularVelocity();
            rb.position = otherPortal.TransformPoint(relPos);
            source.Play();
        }
    }
}
