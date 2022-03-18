using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class InfoPlayerSpeedFunction : InfoFunction{
    public override float function() {
        GameScript gameScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
        float result = gameScript.getPlayer().getSpeed();
        Terminal.log(TerminalLogType.Header, "Player Speed block returns {0}", result);
        return result;
    }
}
