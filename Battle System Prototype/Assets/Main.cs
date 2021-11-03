using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using CommandTerminal;

public class Main : MonoBehaviour
{
    private GameObject game;
    private GameScript gameScript;

    private GameAction playerGameAction;
    private GameAction opponentGameAction;

    private GameObject terminal;
    private Terminal terminalScript;

    private InteractionHandler interactionHandler;

    void Start(){
        enabled=false;

        game=new GameObject();
        game.transform.parent=GameObject.FindGameObjectWithTag("Main").transform;
        game.name="Game";
        game.tag="GameController";
        gameScript=(GameScript)game.AddComponent<GameScript>();

        gameScript.initGameScript();
        
        terminal=new GameObject();
        terminal.transform.parent=GameObject.FindGameObjectWithTag("Main").transform;
        terminal.name="Terminal";
 
        terminalScript=(Terminal)terminal.AddComponent<Terminal>();

        interactionHandler=new InteractionHandler();

        Player player=gameScript.getPlayer();
        Opponent opponent=gameScript.getOpponent();

        terminalScript.initShell(player,opponent,gameScript);

        enabled=true;
    }

    void Update(){
        if(!gameScript.isGameOver()){
            Player player=gameScript.getPlayer();
            Opponent opponent=gameScript.getOpponent();
            if(!gameScript.getOpponentChosen()){
                opponentGameAction=gameScript.getOpponentGameAction();
                gameScript.setOpponentChosen(true);
                terminalScript.setState(TerminalState.Write);
            }else if(gameScript.getPlayerChosen()&&gameScript.getOpponentChosen()){
                Debug.Log("player health: "+player.getHealth().ToString());
                Debug.Log("opponent health: "+opponent.getHealth().ToString());
                terminalScript.setState(TerminalState.ReadOnly);
                playerGameAction=gameScript.getPlayerGameAction();
                interactionHandler.runInteractions(playerGameAction,opponentGameAction,player.getSpeed(),opponent.getSpeed());
                Interaction playerInteraction=interactionHandler.getPlayerInteraction();
                Interaction opponentInteraction=interactionHandler.getOpponentInteraction();
                if(interactionHandler.getPlayerFirst()){
                    playInteraction(playerInteraction,player);
                    if(!gameScript.isGameOver()){
                        playInteraction(opponentInteraction,opponent);
                    }
                }else{
                    playInteraction(opponentInteraction,opponent);
                    if(!gameScript.isGameOver()){
                        playInteraction(playerInteraction,player);
                    }
                }
                gameScript.setPlayerChosen(false);
                gameScript.setOpponentChosen(false);
            }
        }else{
            Debug.Log("Game Over yay");
        }
    }

    //add results of interactions

    void playInteraction(Interaction interaction,Character instigator){
        switch(interaction){
            case AttackInteraction attackInteraction:
                if(attackInteraction.getResult()==InteractionEnum.Hit){
                    Debug.Log(instigator.getCharacterName()+"'s attack: "+attackInteraction.getAttack().getName()+
                              " hits for "+attackInteraction.getDamage().ToString()+" damage!");
                    gameScript.dealDamage(attackInteraction.getTarget(),attackInteraction.getDamage());
                }else{
                    Debug.Log(instigator.getCharacterName()+"'s attack: "+
                              attackInteraction.getAttack().getName()+
                              " misses!");
                }
                break;
            case StatusEffectInteraction statusEffectInteraction:
                if(statusEffectInteraction.getResult()==InteractionEnum.Hit){
                    Debug.Log(instigator.getCharacterName()+"'s status effect: "+
                              statusEffectInteraction.getStatusEffect().getName()+" has "+
                              (statusEffectInteraction.getMultiplier()>1?"raised ":"decreased ")+
                              statusEffectInteraction.getTarget().getCharacterName()+"'s "+
                              statusEffectInteraction.getAttribute().ToString()+
                              " by "+(statusEffectInteraction.getMultiplier()>1?
                                     (100*statusEffectInteraction.getMultiplier()-100f).ToString():
                                     (100*statusEffectInteraction.getMultiplier()).ToString())+"% for "+
                                    statusEffectInteraction.getTurns().ToString()+" turns!");
                    gameScript.addModifier(statusEffectInteraction.getTarget(),statusEffectInteraction.getAttributeModifier());
                }else{
                    Debug.Log(instigator.getCharacterName()+"'s status effect: "+
                              statusEffectInteraction.getStatusEffect().getName()+
                              " misses!");
                }
                break;
            case HealInteraction healInteraction:
                if(healInteraction.getResult()==InteractionEnum.Hit){
                    Debug.Log(instigator.getCharacterName()+"'s heal: "+
                              healInteraction.getHeal().getName()+
                              " heals for "+healInteraction.getHealingAmount().ToString()+" health!");
                    gameScript.heal(healInteraction.getTarget(),healInteraction.getHealingAmount());
                }else{
                    Debug.Log(instigator.getCharacterName()+"'s heal: "+
                              healInteraction.getHeal().getName()+
                              " misses!");
                }
                break;
            default:
                break; 
        }
    }
    
    IEnumerator waitTime(int seconds){yield return new WaitForSeconds(seconds);}
    
}
