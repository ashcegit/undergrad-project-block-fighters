using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSpace : MonoBehaviour,I_Space
{
    InputFieldHandler inputFieldHandler;
    Vector2 position;
    int index;
    bool active;

    void Awake(){
        inputFieldHandler=null;
    }

    public InputFieldHandler getInputFieldHandler(){return inputFieldHandler;}

    public void setInputFieldHandler(InputFieldHandler inputFieldHandler){this.inputFieldHandler=inputFieldHandler;}

    public Vector2 getPosition(){return position;}

    public void setPosition(Vector2 position){this.position=position;}

    public int getIndex(){return index;}

    public void setIndex(int index){this.index=index;}

    public bool getActive(){return active;}

    public void setActive(bool active){this.active=active;}
}
