using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class room : MonoBehaviour
{
    int state = 0;
    public float camSize = 6;
    bool finished = false;
    public bool needsToBeFinished = false;
    //public UnityEvent[] OnEvents;
    //public UnityEvent[] OffEvents;
    
    public int index = 0;
    [System.Serializable]
    public class ObjectArray{
        public GameObject[] objectArray;
    }
    public ObjectArray[] stuff;
    public UnityEvent RollPhaseExit;
    public UnityEvent RollPhaseEnter;
    public void activate(int i){
        state |= 1<<i;
        if(stuff.Length> i && stuff[i]!= null){
            foreach (GameObject item in stuff[i].objectArray)
            {
                item.GetComponent<IChangeable>().activate();
            }
        }
        // if(OnEvents.Length> i && OnEvents[i]!= null)
        //     OnEvents[i].Invoke();
    }
    public void firstActivation(int i){
        state |= 1<<i;
        if(stuff.Length> i && stuff[i]!= null){
            foreach (GameObject item in stuff[i].objectArray)
            {
                item.GetComponent<IChangeable>().firstactivation();
            }
        }
    }
    public void turnOff(int i){
        state &= ~(1<< i);
        if(stuff.Length> i && stuff[i]!= null){
            foreach (GameObject item in stuff[i].objectArray)
            {
                item.GetComponent<IChangeable>().deactivate();
            }
        }
        // if(OffEvents.Length> i &&OffEvents[i]!= null)
        //     OffEvents[i].Invoke();
    }
    public void initiate(int _state){
        state = _state;
        for(int i = 0; i<30; i++){
            if(IsBitSet(state,i)){
                if(stuff.Length> i && stuff[i]!= null){
                    foreach (GameObject item in stuff[i].objectArray)
                    {
                        item.GetComponent<IChangeable>().activate();
                    }
                }
            }
            else{
                if(stuff.Length> i && stuff[i]!= null){
                    foreach (GameObject item in stuff[i].objectArray)
                    {
                        item.GetComponent<IChangeable>().deactivate();
                    }
                }
            }
        }
        
        finished=IsBitSet(state,30);
        
        
        show(PlayerPrefs.GetInt(SceneManager.GetActiveScene().name+"roomVisible"+index.ToString())==1);
    }
    public void finishRoom(){
        finished=true;
        state |= 1<<30;
    }
    public int getState(){
        return state;
    }
    bool IsBitSet(int b, int pos)
    {
        return (b & (1 << pos)) != 0;
    }
    //show on Map (hide grey stuff)
    public void show(bool visible){
        transform.GetChild(0).gameObject.SetActive(!visible);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name+"roomVisible"+index.ToString(),visible?1:0);
    }
    public void save(){
        if(!finished && needsToBeFinished)
            return;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name+"room"+index.ToString(),state);
    }
    public void exitRollPhase(){
         RollPhaseExit.Invoke();
    }
    public void enterRollPhase(){
        RollPhaseEnter.Invoke();
    }
}
