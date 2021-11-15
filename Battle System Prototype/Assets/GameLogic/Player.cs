using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class Player:Character
{   
    public Player(string characterName,
                  float baseMaxHealth,
                  float baseAttack,
                  float baseDefence,
                  float baseSpeed):
                  base(characterName,
                       baseMaxHealth,
                       baseAttack,
                       baseDefence,
                       baseSpeed){
    }
    
    public List<GameAction> punchKickCombo(){
        List<GameAction> gameActions=new List<GameAction>();
        gameActions.Add(new Attack("punch",opponent,ActionType.Physical,0.8f*getAttack(),getSpeed()*1.2f));
        gameActions.Add(new Attack("kick",opponent,ActionType.Physical,getAttack(),getSpeed()*0.8f));
        gameActions.Add(new Attack("punch",opponent,ActionType.Physical,0.8f*getAttack(),getSpeed()*1.2f));
        return gameActions;
    }
}
