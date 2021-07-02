using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public List<room> rooms = new List<room>();
    public List<savePoint> savePoints = new List<savePoint>();
    int currentRoom = 0;
    int lastSavePoint;
    public bool drawPhase = true;
    public bool mapOpen = false;
    public player player;
    public Draw draw;
    public Camera mainCam;
    public Camera mapCam;
    cameraControl cameraControl;
    void Start()
    {
        PlayerPrefs.SetString("currentScene",SceneManager.GetActiveScene().name);
        cameraControl=mainCam.GetComponent<cameraControl>();
        if(!PlayerPrefs.HasKey("velx")){
            player.savePlayerStuff();
        }
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name+"savePoint0",1);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name+"roomVisible0",1);
        loadRooms();
        loadSavePoints();
        
        player.setPos();
        player.drawMode();
        
        SetRoom();
        startDrawPhase();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)&&!mapOpen){
            
            if(drawPhase){
                startRollPhase();
            }
            else{
                startDrawPhase();
            }
            
        }
        if(Input.GetKeyDown(KeyCode.Delete)){
            Reset();
        }
        if(Input.GetKeyDown(KeyCode.Backspace)){
            GoBack();
        }
        if(Input.GetKeyDown(KeyCode.Tab)){
            if(mapOpen){
                closeMap();
            }
            else{
                openMap();
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha0)){
            ResetRooms();
        }
    }
    void startRollPhase(){
        
        drawPhase=false;
        player.rollMode();
        rooms[currentRoom].enterRollPhase();
    }
    public void startDrawPhase(){
        //load room
        cameraControl.target=rooms[currentRoom].transform;
        cameraControl.changeSize(rooms[currentRoom].camSize);
        rooms[currentRoom].initiate(PlayerPrefs.GetInt(SceneManager.GetActiveScene().name+"room"+currentRoom.ToString()));
        draw.EnterDrawPhase();
        drawPhase=true;
        player.setPos();
        player.drawMode();
        rooms[currentRoom].exitRollPhase();
        
        
    }
    void openMap(){
        mapCam.transform.position= mainCam.transform.position;
        mapCam.transform.position= new Vector3(mapCam.transform.position.x,mapCam.transform.position.y,-30);
        mapOpen = true;
        startDrawPhase();
        mainCam.enabled=false;
        mapCam.enabled=true;
    }
    void closeMap(){
        mapOpen=false;
        mainCam.enabled=true;
        mapCam.enabled=false;
    }
    public void ChangeRoom(room room){
        int index = rooms.IndexOf(room);
        if(index == currentRoom){
            return;
        }
        //save every change in the room when room is completed
        SaveRoom(currentRoom);
        
        rooms[currentRoom].exitRollPhase();
        rooms[currentRoom].initiate(PlayerPrefs.GetInt(SceneManager.GetActiveScene().name+"room"+currentRoom.ToString()));
        currentRoom = index;

        PlayerPrefs.SetInt("currentRoom", currentRoom);
        rooms[currentRoom].show(true);
        
        player.savePlayerStuff();

        draw.DeleteLines();
        startDrawPhase();
    }
    void GotoSavePoint(int point){
        player.GetComponent<charge>().Discharge();
        rooms[currentRoom].initiate(PlayerPrefs.GetInt(SceneManager.GetActiveScene().name+"room"+currentRoom.ToString()));
        rooms[currentRoom].exitRollPhase();
        currentRoom = savePoints[point].room;
        

        PlayerPrefs.SetInt("currentRoom", currentRoom);
        rooms[currentRoom].show(true);
        
        PlayerPrefs.SetFloat("posx", savePoints[point].transform.position.x);
        PlayerPrefs.SetFloat("posy", savePoints[point].transform.position.y);
        PlayerPrefs.SetFloat("velx", 0);
        PlayerPrefs.SetFloat("vely", 0);
        PlayerPrefs.SetFloat("angular", 0);
        draw.DeleteLines();
        startDrawPhase();

    }
    public void ClickOnSavePoint(int i){
        if(mapOpen){
            GotoSavePoint(i);
            closeMap();
        }
    }
    void SetRoom(){
        currentRoom= PlayerPrefs.GetInt("currentRoom");
    }
    void SaveRoom(int roomID){
        //room is saved with an int, every bit can be used to save something
        rooms[roomID].save();
    }
    [ContextMenu("resetRooms")]
    void ResetRooms(){
        PlayerPrefs.DeleteAll();
    }
    
    
    public room getCurrentRoom(){
        return rooms[currentRoom];
    }
    void Reset(){
        startDrawPhase();
        draw.DeleteLines();
    }
    void GoBack(){
        GotoSavePoint(lastSavePoint);
        
    }
    void loadRooms(){
        for(int i = 0; i<rooms.Count;i++){
            rooms[i].index=i;
            rooms[i].initiate(PlayerPrefs.GetInt(SceneManager.GetActiveScene().name+"room"+i.ToString()));
        }
    }
    void loadSavePoints(){
        for(int i = 0; i<savePoints.Count;i++){
            savePoints[i].init(i);
        }
    }
    public void setLastSavePoint(int index){
        lastSavePoint=index;
    }
    public void changeScene(string scene){
        player.deletePlayerStuff();
        PlayerPrefs.DeleteKey("currentRoom");
        SceneManager.LoadScene(scene);
    }
    [ContextMenu("newScene")]
    public void testChangeScene(){
        player.deletePlayerStuff();
        PlayerPrefs.DeleteKey("currentRoom");
    }
}
