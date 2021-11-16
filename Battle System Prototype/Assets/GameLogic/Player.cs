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
    
    public List<GameAction> punch_and_kick(){
        List<GameAction> gameActions=new List<GameAction>();
        gameActions.Add(new Attack("punch",
                                    opponent,
                                    ActionType.Physical,
                                    0.8f*getAttack(),
                                    getSpeed()*1.2f));
        gameActions.Add(new Attack("kick",
                                    opponent,
                                    ActionType.Physical,
                                    getAttack(),
                                    getSpeed()*0.8f));
        gameActions.Add(new Attack("punch",
                                    opponent,
                                    ActionType.Physical,
                                    0.8f*getAttack(),
                                    getSpeed()*1.2f));
        return gameActions;
    }

    public List<GameAction> heal_and_defenc(){
        List<GameAction> gameActions=new List<GameAction>();
        gameActions.Add(new Heal("healing magic",
                                    this,
                                    ActionType.Magic,
                                    13f));
        gameActions.Add(new StatusEffect("Defence",
                                    this,
                                    ActionType.Magic,
                                    AttributeEnum.Defence,
                                    1.4f,
                                    0.7f,
                                    2));
        return gameActions;
    }
}
