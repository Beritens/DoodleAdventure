using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public GameObject standardBrush;
    GameObject brush;
    public Camera cam;

    LineRenderer lineRenderer;
    EdgeCollider2D lineCollider;
    List<Vector2> points = new List<Vector2>();
    Manager manager;
    public Transform lineContainer;
    bool cantDrawYet = false;
    public LayerMask mask;
    bool erase = false;
    public LayerMask eraserMask;
    List<GameObject> lines = new List<GameObject>();
    public GameObject eraserIcon;
    public GameObject brushIcon;
    public AudioSource audioSource;
    
    void Awake()
    {
        manager = GameObject.FindObjectOfType<Manager>();
        brush = standardBrush;
    }
    void Update()
    {
        
        if(manager.drawPhase&&!manager.mapOpen && !cantDrawYet){
            if(erase){
                doEraserThing();
                audioSource.enabled=false;
            }
            else{
                doThePencilThing();
            }
            if(Input.GetKeyDown(KeyCode.Z)&&Input.GetKey(KeyCode.LeftControl)||Input.GetKey(KeyCode.Z)&&Input.GetKeyDown(KeyCode.LeftControl)||Input.GetKeyDown(KeyCode.Z)&&Input.GetKeyDown(KeyCode.LeftControl)){
                controlZ();
            }
        }
        else{
            audioSource.enabled=false;
        }
            
        if(Input.GetKeyUp(KeyCode.Mouse0)){
            cantDrawYet = false;
        }
        if(Input.GetKeyDown(KeyCode.E)){
            Erase();
        }
        
    }
    public void Erase(){
        erase = !erase;
        eraserIcon.SetActive(erase);
        //brush.SetActive(!erase);
    }
    void controlZ(){
        if(lines.Count==0)
            return;
        lines[lines.Count-1].SetActive(!lines[lines.Count-1].activeSelf);
        lines.RemoveAt(lines.Count-1);
    }

    void doEraserThing(){
        if(Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse0)){
            Vector2 mousePos= cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] colliders =  Physics2D.OverlapCircleAll(mousePos, 0.1f,eraserMask);
            
            foreach (Collider2D collider in colliders)
            {
                if(collider.transform.parent==lineContainer){
                    collider.gameObject.SetActive(false);
                    lines.Add(collider.gameObject);
                }
            }
        }
        
    }
    void doThePencilThing()
    {
        Vector2 mousePos= cam.ScreenToWorldPoint(Input.mousePosition);
        bool mouseIsOver = MouseOver();
        if((Input.GetKeyDown(KeyCode.Mouse0)&&!mouseIsOver)|| Input.GetKeyDown(KeyCode.Mouse1)){
            CreateBrush(mousePos);
            return;
        }
        if(Input.GetKey(KeyCode.Mouse0) && !mouseIsOver){
            if(lineRenderer==null && lineCollider== null){
                CreateBrush(mousePos);
            }
            else{
                
                if(mousePos!= points[points.Count-1]){
                    AddPoint(mousePos);
                    playAudio();
                    dontDrawOverStuff();
                }
                else{
                    audioSource.volume=0;
                }
            }
            return;
            
        }
        if(Input.GetKey(KeyCode.Mouse1)&&lineRenderer.positionCount<=2){
            if(lineRenderer.positionCount==1){
                AddPoint(mousePos);
            }
            else{
                points[1] = mousePos;
                lineRenderer.SetPosition(1, mousePos);
                lineCollider.points= points.ToArray();
            }
            return;
        }
        dontDrawOverStuff();
        audioSource.enabled=false;
        lineRenderer=null;
        lineCollider=null;
        points = new List<Vector2>();
        
        
    }
    void playAudio(){
        float dist = (points[points.Count-1]-points[points.Count-2]).magnitude;
        
        audioSource.volume=dist.Remap(0,0.1f,0,1);
    }
    
    void CreateBrush(Vector2 mousePos){
        audioSource.enabled=true;
        audioSource.Play();
        audioSource.volume=0;
        points = new List<Vector2>();
        GameObject brushInstance = Instantiate(brush,Vector3.zero,Quaternion.identity,lineContainer);
        lineRenderer = brushInstance.GetComponent<LineRenderer>();
        lineCollider = brushInstance.GetComponent<EdgeCollider2D>();
        AddPoint(mousePos);
        lines.Add(brushInstance);
    }

    void AddPoint(Vector2 point){
        points.Add(point);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(points.Count-1, point);
        lineCollider.points= points.ToArray();
    }
    public void DeleteLines(){
        lineRenderer=null;
        lineCollider=null;
        points = new List<Vector2>();
        foreach (Transform child in lineContainer)
        {
            Destroy(child.gameObject);
        }
        lines = new List<GameObject>();
    }
    public void EnterDrawPhase(){
        resetBrush();
        lineRenderer=null;
        lineCollider=null;
        points = new List<Vector2>();
        if(Input.GetKey(KeyCode.Mouse0)|| Input.GetKeyDown(KeyCode.Mouse0)){
            cantDrawYet = true;
        }
        else{
            cantDrawYet=false;
        }
    }
    bool MouseOver(){
        Vector2 mousePos= cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero,0,mask);
        if(hit.collider!=null){
            return true;
        }
        else{
            return false;
        }
    }
    public void changeBrush(GameObject _brush){
        brush = _brush;
    }
    public void resetBrush(){
        brush=standardBrush;
    }
    void changePoint(int index, Vector2 point){
        points[index]=point;
        lineRenderer.SetPosition(index, point);
        lineCollider.points= points.ToArray();
    }
    void dontDrawOverStuff(){
        
        if(points.Count>= 2){
            Vector2 point1 = points[points.Count-2];
            Vector2 point2 = points[points.Count-1];
            RaycastHit2D hit = Physics2D.Raycast(point1,point2-point1,(point2-point1).magnitude,mask);
            if(hit.collider!=null){
                changePoint(points.Count-1,hit.point);
                if(points.Count==2 && point1==hit.point){
                    Destroy(lineRenderer.gameObject);
                    lines.RemoveAt(lines.Count-1);
                }
            }
            RaycastHit2D hit2 = Physics2D.Raycast(point2,point1-point2,(point2-point1).magnitude,mask);
            if(hit2.collider!= null){
                if(hit2.point==point2){
                    audioSource.enabled=false;
                    lineRenderer=null;
                    lineCollider=null;
                    points = new List<Vector2>();
                    return;
                }
                else{
                    CreateBrush(hit2.point);
                    AddPoint(point2);
                }
                
            }
        }
    }
}
