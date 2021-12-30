using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttackPunchFunction:ActionFunction
{
    public override GameAction function(Character target, Character instigator){
        return ActionCollection.punch(target,instigator);
    }
}
