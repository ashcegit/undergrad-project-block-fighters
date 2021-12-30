using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttackKickFunction : ActionFunction
{
    public override GameAction function(Character target,Character instigator){
        return ActionCollection.kick(target,instigator);
    }
}
