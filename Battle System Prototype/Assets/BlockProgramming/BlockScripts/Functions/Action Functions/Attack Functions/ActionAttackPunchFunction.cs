using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttackPunchFunction:ActionFunction
{
    public override GameAction function(Character target, Character instigator){
        if(target!=null){
            return ActionCollection.punch(target,instigator);
        }else{
            Block inputBlock=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers()[0].getInputBlock().GetComponent<Block>();
            CharacterFunction characterFunction=inputBlock.gameObject.GetComponent<CharacterFunction>();
            return ActionCollection.punch(characterFunction.function(),instigator);
        }     
    }
}
