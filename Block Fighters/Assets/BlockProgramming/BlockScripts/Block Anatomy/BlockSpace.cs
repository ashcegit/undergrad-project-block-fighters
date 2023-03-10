using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpace : I_Space
{
    GameObject parentBody;
    Vector2 position;
    int index;
    bool active;
    GameObject test;

    public BlockSpace() {
        parentBody = null;
    }

    // Start is called before the first frame update

    public GameObject getParentBody(){return parentBody;}

    public void setParentBody(GameObject parentBody){this.parentBody=parentBody;}

    public Vector2 getPosition(){return position;}

    public void setPosition(Vector2 position){
        this.position=position;
    }

    public int getIndex(){return index;}

    public void setIndex(int index){this.index=index;}

    public bool getActive(){return active;}

    public void setActive(bool active){this.active=active;}
}
