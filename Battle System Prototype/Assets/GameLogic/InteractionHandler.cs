using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//takes game actions and outputs interactions

public class InteractionHandler
{
    Player player;
    Opponent opponent;

    GameAction playerGameAction;
    GameAction opponentGameAction;

    Interaction playerInteraction;
    Interaction opponentInteraction;

    bool playerFirst;
    
    public void runInteractions(GameAction playerGameAction,GameAction opponentGameAction,Player player,Opponent opponent){
        this.playerGameAction=playerGameAction;
        this.opponentGameAction=opponentGameAction;

        this.player=player;
        this.opponent=opponent;

        playerFirst=isPlayerFirst();

        switch(playerGameAction){
            case Attack attack:
                playerInteraction=(Interaction) new AttackInteraction(attack.getTarget(),attack);
                break;
            case StatusEffect statusEffect:
                playerInteraction=(Interaction) new StatusEffectInteraction(statusEffect.getTarget(),statusEffect);
                break;
            case Heal heal:
                playerInteraction=(Interaction) new HealInteraction(heal.getTarget(),heal);
                break;
            default:
                break;  
        }

        switch(opponentGameAction){
            case Attack attack:
                opponentInteraction=(Interaction) new AttackInteraction(attack.getTarget(),attack);
                break;
            case StatusEffect statusEffect:
                opponentInteraction=(Interaction) new StatusEffectInteraction(statusEffect.getTarget(),statusEffect);
                break;
            case Heal heal:
                opponentInteraction=(Interaction) new HealInteraction(heal.getTarget(),heal);
                break;
            default:
                break;  
        }
    }

    public bool isPlayerFirst(){
        if(!attackTypeTieCheck()){return playerAttackTypeAdvantage();}
        else{
            float playerGameActionSpeed;
            float opponentGameActionSpeed;

            if(playerGameAction is Attack){playerGameActionSpeed=((Attack)playerGameAction).getSpeed();}
            else{playerGameActionSpeed=player.getSpeed();}
            if(opponentGameAction is Attack){opponentGameActionSpeed=((Attack)opponentGameAction).getSpeed();}
            else{opponentGameActionSpeed=opponent.getSpeed();}

            if(playerGameActionSpeed>opponentGameActionSpeed){return true;}
            else if(playerGameActionSpeed<opponentGameActionSpeed){return false;}
            else{
                if(Random.Range(0,1)==0){return true;}
                else{return false;}
            }
        }
    }

    public bool attackTypeTieCheck(){
        return playerGameAction.getActionType()==opponentGameAction.getActionType();
    }

    public bool playerAttackTypeAdvantage(){
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

    public bool getPlayerFirst(){return playerFirst;}

    public Interaction getPlayerInteraction(){return playerInteraction;}
    public Interaction getOpponentInteraction(){return opponentInteraction;}
}
