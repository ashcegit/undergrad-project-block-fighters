using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//takes game actions and outputs interactions

public class InteractionHandler
{    
    public Interaction getInteraction(GameAction gameAction){
        Interaction interaction=null;

        switch(gameAction){
            case Attack attack:
                interaction=(Interaction) new AttackInteraction(attack.getTarget(),attack);
                break;
            case StatusEffect statusEffect:
                interaction=(Interaction) new StatusEffectInteraction(statusEffect.getTarget(),statusEffect);
                break;
            case Heal heal:
                interaction=(Interaction) new HealInteraction(heal.getTarget(),heal);
                break;
            default:
                break;  
        }

        return interaction;
    }

    public bool getPlayerFirst(GameAction playerGameAction,GameAction opponentGameAction,float playerSpeed,float opponentSpeed){
        if(!attackTypeTieCheck(playerGameAction,opponentGameAction)){
            return playerAttackTypeAdvantage(playerGameAction,opponentGameAction);
        }else{
            float playerGameActionSpeed;
            float opponentGameActionSpeed;

            if(playerGameAction is Attack){playerGameActionSpeed=((Attack)playerGameAction).getSpeed();}
            else{playerGameActionSpeed=playerSpeed;}
            if(opponentGameAction is Attack){opponentGameActionSpeed=((Attack)opponentGameAction).getSpeed();}
            else{opponentGameActionSpeed=opponentSpeed;}

            if(playerGameActionSpeed>opponentGameActionSpeed){return true;}
            else if(playerGameActionSpeed<opponentGameActionSpeed){return false;}
            else{
                if(Random.Range(0,1)==0){return true;}
                else{return false;}
            }
        }
    }

    public bool attackTypeTieCheck(GameAction playerGameAction,GameAction opponentGameAction){
        return playerGameAction.getActionType()==opponentGameAction.getActionType();
    }

    public bool playerAttackTypeAdvantage(GameAction playerGameAction,GameAction opponentGameAction){
        if(playerGameAction.getActionType()==ActionType.Physical){
            if(opponentGameAction.getActionType()==ActionType.Magic){return true;}
            else{return false;}
        }else if(playerGameAction.getActionType()==ActionType.Magic){
            if(opponentGameAction.getActionType()==ActionType.Science){return true;}
            else{return false;}
        }else{
            if(opponentGameAction.getActionType()==ActionType.Physical){return true;}
            else{return false;}
        }
    }
}
