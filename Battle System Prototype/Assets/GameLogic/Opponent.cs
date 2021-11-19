using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class Opponent:Character
{   
    public Opponent(string characterName,
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
        gameActions.Add(ActionCollection.punch(player,this));
        gameActions.Add(ActionCollection.kick(player,this));
        gameActions.Add(ActionCollection.punch(player,this));
        return gameActions;
    }

    public List<GameAction> heal_and_defend(){
        List<GameAction> gameActions=new List<GameAction>();
        gameActions.Add(ActionCollection.healingMagic(this));
        gameActions.Add(ActionCollection.guard(this));
        return gameActions;
    }
}
