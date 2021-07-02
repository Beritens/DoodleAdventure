using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    Manager manager;
    [SerializeField]
    GameObject arrow;
    [SerializeField]
    Transform head;
    [SerializeField]
    LineRenderer line;
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    float maxLength = 5;
    [SerializeField]
    float lengthStuff = 0.1f;
    [SerializeField]
    float minSpeed;
    [SerializeField]
    Transform rotationStuff;
    [SerializeField]
    SpriteMask[] masks;
    [SerializeField]
    float minRot;
    Vector2 velocity;
    float angularVelocity;
    public AudioSource audioSource;
    public LayerMask makeSound;
    // Start is called before the first frame update
    void Awake()
    {
        manager = GameObject.FindObjectOfType<Manager>();
    }

    // Update is called once per frame
    public void drawMode(){
        
        
        rb.bodyType= RigidbodyType2D.Static;
        rb.bodyType= RigidbodyType2D.Kinematic;
        Vector2 speed = new Vector2(PlayerPrefs.GetFloat("velx"),PlayerPrefs.GetFloat("vely"));
        float angVel = PlayerPrefs.GetFloat("angular");
        
        if(Mathf.Abs(angVel) <= minRot){
            rotationStuff.gameObject.SetActive(false);
        }
        else{
            //rotation arrows
            rotationStuff.gameObject.SetActive(true);
            float f = Mathf.Abs(angVel).Remap(300,5000,0.88f,0f);
            foreach(SpriteMask mask in masks){
                mask.alphaCutoff=f;
            }
            rotationStuff.localScale=new Vector3(angVel>0?-1:1,1,1);
            
        }
        


        //velocity arrow
        if(speed.magnitude<= minSpeed){
            arrow.SetActive(false);
        }
        else{
            //way too complicated lol: length of arrow
            speed = speed.normalized*Mathf.Clamp((-1f/(1+Mathf.Pow(2,speed.magnitude*lengthStuff-4))+1)*maxLength+1,1,maxLength+1);
            arrow.SetActive(true);
            line.SetPosition(0,(Vector2)transform.position);
            line.SetPosition(1,(Vector2)transform.position+speed);
            head.position=(Vector2)transform.position+speed;
            head.right = speed;
        }
        
        
        
    }
    void Update()
    {
        velocity = rb.velocity;
        angularVelocity = rb.angularVelocity;
    }
    public float AngularVelocity(){
        return angularVelocity;
    }
    public Vector2 Velocity(){
        return velocity;
    }
    public void rollMode(){
        rotationStuff.gameObject.SetActive(false);
        arrow.SetActive(false);
        rb.bodyType= RigidbodyType2D.Dynamic;
        setVelocity();
    }
    void setVelocity(){
        rb.velocity= new Vector2(PlayerPrefs.GetFloat("velx"),PlayerPrefs.GetFloat("vely"));
       rb.angularVelocity= PlayerPrefs.GetFloat("angular");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<room>()!= null){
            manager.ChangeRoom(other.gameObject.GetComponent<room>());
        }
    }
    public void savePlayerStuff(){
        Vector2 pos = transform.position;
        Vector2 vel = rb.velocity;
        PlayerPrefs.SetFloat("posx", pos.x);
        PlayerPrefs.SetFloat("posy", pos.y);
        PlayerPrefs.SetFloat("velx", vel.x);
        PlayerPrefs.SetFloat("vely", vel.y);
        PlayerPrefs.SetFloat("angular", rb.angularVelocity);
    }
    public void setPos(){
        transform.position= new Vector2(PlayerPrefs.GetFloat("posx"),PlayerPrefs.GetFloat("posy"));
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "death"){
            manager.startDrawPhase();
        }
        if(makeSound== (makeSound | (1 << other.gameObject.layer))){
            if(Vector2.Angle(velocity,-other.contacts[0].normal)<25)
                audioSource.PlayOneShot(audioSource.clip,Mathf.Clamp01(other.relativeVelocity.magnitude*0.01f));
            
        }

    }
    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
        
    }
    [ContextMenu("deleteStuff")]
    public void deletePlayerStuff(){
        PlayerPrefs.DeleteKey("posx");
        PlayerPrefs.DeleteKey("posy");
        PlayerPrefs.DeleteKey("velx");
        PlayerPrefs.DeleteKey("vely");
        PlayerPrefs.DeleteKey("angular");
    }
    
}
public static class ExtensionMethods {
 
    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
   
}
