using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayerFunction : CharacterFunction
{
    public override Character function(){
        GameScript gameScript=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
        return gameScript.getPlayer();
    }
}
