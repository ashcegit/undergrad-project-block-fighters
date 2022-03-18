using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHealMedicineFunction : ActionFunction
{
    public override GameAction function(Character instigator, bool computerPlayer) {
        Block inputBlock = gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers()[0].getInputBlock().GetComponent<Block>();
        Character target;
        if (computerPlayer) {
            GameScript gameScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
            target = gameScript.getOpponent();
        } else {
            CharacterFunction characterFunction = inputBlock.gameObject.GetComponent<CharacterFunction>();
            target = characterFunction.function();
        }
        return ActionCollection.medicine(target);
    }
}
