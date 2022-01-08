using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoOpponentHealthFunction : InfoFunction
{
    public override float function(){
        GameScript gameScript=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
        return gameScript.getOpponent().getHealth();
    }
}
