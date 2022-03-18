using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class InfoPlayerAttackFunction : InfoFunction
{
    public override float function(){
        GameScript gameScript=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
        float result = gameScript.getPlayer().getAttack();
        Terminal.log(TerminalLogType.Header, "Player Attack block returns {0}", result);
        return result;
    }
}
