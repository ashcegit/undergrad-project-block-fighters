using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class InfoPlayerDefenceFunction : InfoFunction
{
    public override float function() {
        GameScript gameScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
        float result = gameScript.getPlayer().getDefence();
        Terminal.log(TerminalLogType.Header, "Player Defence block returns {0}", result);
        return result;
    }
}
