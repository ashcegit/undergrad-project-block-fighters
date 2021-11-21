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

    private List<GameAction> playerGameActions;
    private List<GameAction> opponentGameActions;

    private GameObject terminal;
    private Terminal terminalScript;

    private InteractionHandler interactionHandler;

    private bool loopDone;

    void Start(){
        enabled=false;

        StopAllCoroutines();

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

        loopDone=true;

        enabled=true;
    }
    
    void resetGame(){
        enabled=false;
        Terminal.Buffer.Clear();
        gameScript.resetGame();
        terminalScript.initShell(gameScript.getPlayer(),gameScript.getOpponent(),gameScript);

        loopDone=true;
        enabled=true;
    }

    void Update(){
        if(gameScript.getCharactersLoaded()&&!gameScript.isGameOver()&&loopDone){
            StopAllCoroutines();
            Player player=gameScript.getPlayer();
            Opponent opponent=gameScript.getOpponent();

            if(!gameScript.getOpponentChosen()){
                opponentGameActions=gameScript.getOpponentGameActions();
                gameScript.setOpponentChosen(true);
                terminalScript.setState(TerminalState.Write);
            }else if(gameScript.getPlayerChosen()){
                loopDone=false;
                terminalScript.setState(TerminalState.ReadOnly);
                playerGameActions=gameScript.getPlayerGameActions();
                StartCoroutine(playLoop(player,opponent));                
                gameScript.setPlayerChosen(false);
                gameScript.setOpponentChosen(false);
                gameScript.endTurn();
            }
        }else if(gameScript.isGameOver()){
            resetGame();
        }
    }

    IEnumerator playLoop(Player player,Opponent opponent){
        int largestListCount=(playerGameActions.Count>=opponentGameActions.Count?playerGameActions.Count:opponentGameActions.Count);
        for(int i=0;i<largestListCount;i++){
            if(!gameScript.isGameOver()){
                if(i>playerGameActions.Count-1){
                    //Only perform Opponent Game Actions
                    Interaction opponentInteraction=gameScript.getInteraction(opponentGameActions[i]);
                    yield return StartCoroutine(playInteractionAfterDelay(0.5f,opponentInteraction,opponent));
                }else if(i>opponentGameActions.Count-1){
                    //Only perform Player Game Actions
                    Interaction playerInteraction=gameScript.getInteraction(playerGameActions[i]);
                    yield return StartCoroutine(playInteractionAfterDelay(0.5f,playerInteraction,player));
                }else{
                    Interaction playerInteraction=gameScript.getInteraction(playerGameActions[i]);
                    Interaction opponentInteraction=gameScript.getInteraction(opponentGameActions[i]);
                    if(gameScript.getPlayerFirst(playerGameActions[i],opponentGameActions[i])){
                        yield return StartCoroutine(playInteractionAfterDelay(0.5f,playerInteraction,player));
                        if(!gameScript.isGameOver()){
                            yield return StartCoroutine(playInteractionAfterDelay(0.5f,opponentInteraction,opponent));
                        }
                    }else{
                        yield return StartCoroutine(playInteractionAfterDelay(0.5f,opponentInteraction,opponent));
                        if(!gameScript.isGameOver()){
                            yield return StartCoroutine(playInteractionAfterDelay(0.5f,playerInteraction,player));
                        }
                    }
                }
            }
        }
        loopDone=true;
    }

    IEnumerator playInteractionAfterDelay(float seconds,Interaction interaction,Character character){
        float elapsedSeconds=0f;
        while(elapsedSeconds<seconds){
            elapsedSeconds+=Time.deltaTime;
            yield return null;
        }
        playInteraction(interaction,character);
    }

    void playInteraction(Interaction interaction,Character instigator){
        switch(interaction){
            case AttackInteraction attackInteraction:
                if(attackInteraction.getResult()==InteractionEnum.Hit){
                    Terminal.log(TerminalLogType.Message,
                                        "{0}'s attack '{1}' hits for {2} damage!\n",
                                        instigator.getCharacterName(),
                                        attackInteraction.getAttack().getName(),
                                        attackInteraction.getDamage().ToString());
                    gameScript.dealDamage(attackInteraction.getTarget(),attackInteraction.getDamage());
                }else{
                    Terminal.log("{0}'s attack '{1}' misses!\n",
                                        instigator.getCharacterName(),
                                        attackInteraction.getAttack().getName());
                }
                break;
            case StatusEffectInteraction statusEffectInteraction:
                if(statusEffectInteraction.getResult()==InteractionEnum.Hit){
                    Terminal.log(TerminalLogType.Message,
                                "{0}'s status effect '{1}' has {2} {3}'s {4} by {5}% for {6} turns!\n",
                                //character name
                                instigator.getCharacterName(),
                                //status effect name
                                statusEffectInteraction.getStatusEffect().getName(),
                                //raised or decreased
                                (statusEffectInteraction.getMultiplier()>1?"raised ":"decreased "),
                                //target name
                                statusEffectInteraction.getTarget().getCharacterName(),
                                //attribute affected
                                statusEffectInteraction.getAttribute().ToString(),
                                //percentage amount - affected by whether attribute is raised or lowered
                                (statusEffectInteraction.getMultiplier()>1?
                                     (100*statusEffectInteraction.getMultiplier()-100f).ToString():
                                     (100*statusEffectInteraction.getMultiplier()).ToString()),
                                //number of turns
                                statusEffectInteraction.getTurns().ToString());
                    gameScript.addModifier(statusEffectInteraction.getTarget(),statusEffectInteraction.getAttributeModifier());
                }else{
                    Terminal.log(TerminalLogType.Message,
                                "{0}'s status effect '{1}' misses!\n",
                                instigator.getCharacterName(),
                                statusEffectInteraction.getStatusEffect().getName());
                }
                break;
            case HealInteraction healInteraction:
                if(healInteraction.getResult()==InteractionEnum.Hit){
                    Terminal.log(TerminalLogType.Message,
                                "{0}'s heal '{1}' heals for {2} health!\n",
                                instigator.getCharacterName(),
                                healInteraction.getHeal().getName(),
                                healInteraction.getHealingAmount().ToString());
                    gameScript.heal(healInteraction.getTarget(),healInteraction.getHealingAmount());
                }else{
                    Terminal.log(TerminalLogType.Message,
                                "{0}'s heal '{1}' misses!\n",
                                instigator.getCharacterName(),
                                healInteraction.getHeal().getName());
                }
                break;
            default:
                break; 
        }
    }
    
    IEnumerator waitTime(int seconds){yield return new WaitForSeconds(seconds);}
    
}
