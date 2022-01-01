using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ExecutionWrapper
{
    private GameAction? gameAction;
    private bool endOfSection;

    public GameAction? getGameAction(){return gameAction;}
    public void setGameAction(GameAction? gameAction){this.gameAction=gameAction;}

    public bool getEndOfSection(){return endOfSection;}
    public void setEndOfSection(bool endOfSection){this.endOfSection=endOfSection;}
}
