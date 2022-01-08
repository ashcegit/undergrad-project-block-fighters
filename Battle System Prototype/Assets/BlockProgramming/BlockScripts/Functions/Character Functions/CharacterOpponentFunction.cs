using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOpponentFunction : CharacterFunction
{
    public override Character function(){
        GameScript gameScript=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
        return gameScript.getOpponent();
    }
}
