using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class InfoPlayerHealthFunction : InfoFunction
{
    public override float function() {
        GameScript gameScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
        float result = gameScript.getPlayer().getAttack();
        Terminal.log(TerminalLogType.Header, "Player Health block returns {0}", result);
        return result;
    }
}
