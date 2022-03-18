using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class InfoOpponentSpeedFunction : InfoFunction
{
    public override float function() {
        GameScript gameScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
        float result = gameScript.getOpponent().getSpeed();
        Terminal.log(TerminalLogType.Header, "Opponent Speed block returns {0}", result);
        return result;
    }
}
