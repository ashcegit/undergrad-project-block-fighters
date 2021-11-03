using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAction{
    
    protected string name;
    protected Character target;
    protected ActionType actionType;
        
    public GameAction(string name,Character target,ActionType actionType){
        this.name=name;
        this.target=target;
        this.actionType=actionType;
    }

    public string getName(){return name;}
    public Character getTarget(){return target;}
    public void setTarget(Character target){this.target=target;}
    public ActionType getActionType(){return actionType;}
    public void setActionType(ActionType actionType){this.actionType=actionType;}
}
