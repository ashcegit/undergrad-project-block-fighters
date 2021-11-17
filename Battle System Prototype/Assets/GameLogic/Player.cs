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
        gameActions.Add(ActionCollection.punch(getOpponent(),this));
        gameActions.Add(ActionCollection.kick(getOpponent(),this));
        gameActions.Add(ActionCollection.punch(getOpponent(),this));
        Debug.Log(getOpponent().getCharacterName());
        Debug.Log(gameActions[0].getTarget().getCharacterName());
        return gameActions;
    }

    public List<GameAction> heal_and_defend(){
        List<GameAction> gameActions=new List<GameAction>();
        gameActions.Add(ActionCollection.healingMagic(this));
        gameActions.Add(ActionCollection.guard(this));
        return gameActions;
    }
}
