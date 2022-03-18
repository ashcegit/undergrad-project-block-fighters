using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class InfoOpponentAttackFunction : InfoFunction
{
    public override float function() {
        GameScript gameScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
        float result = gameScript.getOpponent().getAttack();
        Terminal.log(TerminalLogType.Header, "Opponent Attack block returns {0}", result);
        return result;
    }
}
