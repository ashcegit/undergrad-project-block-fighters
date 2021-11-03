using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeModifier
{
    int remainingTurns;
    AttributeEnum attribute;
    float multiplier;
    bool finished;

    public AttributeModifier(int remainingTurns,AttributeEnum attribute,float multiplier){
        this.remainingTurns=remainingTurns;
        this.attribute=attribute;
        this.multiplier=multiplier;
        this.finished=false;
    }

    public int getRemainingTurns(){return remainingTurns;}
    public AttributeEnum getAttribute(){return attribute;}
    public float getMultiplier(){return multiplier;}
    public bool isFinished(){return finished;}

    public void decrementRemainingTurns(){remainingTurns-=1;}
    public void setFinished(bool finished){this.finished=finished;}
    public void turnHasPassed(){
        if(getRemainingTurns()==0){setFinished(true);
        }else{decrementRemainingTurns();}
    }
}
