using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class InfoOpponentDefenceFunction : InfoFunction
{
    public override float function() {
        GameScript gameScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
        float result = gameScript.getOpponent().getDefence();
        Terminal.log(TerminalLogType.Header, "Opponent Defence block returns {0}", result);
        return result;
    }
}
