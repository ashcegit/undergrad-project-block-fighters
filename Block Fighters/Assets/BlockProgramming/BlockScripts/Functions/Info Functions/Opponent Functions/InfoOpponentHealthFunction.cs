using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class InfoOpponentHealthFunction : InfoFunction
{
    public override float function() {
        GameScript gameScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
        float result = gameScript.getOpponent().getHealth();
        Terminal.log(TerminalLogType.Header, "Opponent Health block returns {0}", result);
        return result;
    }
}
