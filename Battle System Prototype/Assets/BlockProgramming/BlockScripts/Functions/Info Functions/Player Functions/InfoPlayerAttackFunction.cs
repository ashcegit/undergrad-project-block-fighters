using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPlayerAttackFunction : InfoFunction
{
    public override float function(){
        GameScript gameScript=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
        return gameScript.getPlayer().getAttack();
    }
}
