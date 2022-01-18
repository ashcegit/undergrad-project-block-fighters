using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using CommandTerminal;

public class Main : MonoBehaviour
{
    private GameObject blockProgrammer;
    private BlockProgrammerScript blockProgrammerScript;
    private List<Block> methodBlocks;

    private GameObject game;
    private GameScript gameScript;

    private GameObject playerMethod;
    private GameObject opponentMethod;

    private GameObject terminal;
    private Terminal terminalScript;

    private InteractionHandler interactionHandler;

    private bool loopDone;

    void Start(){
        enabled=false;

        StopAllCoroutines();
        blockProgrammer=GameObject.FindGameObjectWithTag("BlockProgrammer");
        blockProgrammerScript=(BlockProgrammerScript)blockProgrammer.GetComponent<BlockProgrammerScript>();

        gameScript=GetComponentInChildren<GameScript>();

        gameScript.initGameScript();

        terminalScript=GetComponentInChildren<Terminal>();

        interactionHandler=new InteractionHandler();

        Character player=gameScript.getPlayer();
        Character opponent=gameScript.getOpponent();

        terminalScript.initShell(gameScript);

        loopDone=true;
        terminalScript.setState(TerminalState.Write);
        gameScript.enabled=false;
    }
    
    void resetGame(){
        enabled=false;
        Terminal.Buffer.Clear();
        gameScript.resetGame();
        terminalScript.initShell(gameScript);
        openProgramming();
        loopDone=true;
    }

    public void openProgramming(){
        terminalScript.setState(TerminalState.Write);
        blockProgrammerScript.enabled=true;
        gameScript.enabled=false;
    }

    public void finishProgramming(){
        terminalScript.setState(TerminalState.Write);
        blockProgrammerScript.applyMethodNames();
        terminalScript.registerCharacterCommands(gameScript.getPlayer(),
                                                    gameScript.getOpponent(),
                                                    blockProgrammerScript.getMethodBlockObjects());
        blockProgrammerScript.enabled=false;

        Character player=gameScript.getPlayer();
        Character opponent=gameScript.getOpponent();

        gameScript.updatePlayerName(player.getCharacterName());
        gameScript.updateOpponentName(opponent.getCharacterName());

        gameScript.updatePlayerHealth(player.getHealth(),player.getMaxHealth());
        gameScript.updateOpponentHealth(opponent.getHealth(),opponent.getMaxHealth());

        gameScript.enabled=true;
        enabled=true;
    }

    void Update(){
        if(gameScript.getCharactersLoaded()&&!gameScript.isGameOver()&&loopDone){
            StopAllCoroutines();
            Character player=gameScript.getPlayer();
            Character opponent=gameScript.getOpponent();

            if(!gameScript.getOpponentChosen()){
                gameScript.getComputerPlayer().initComputerBlockStackA();
                gameScript.setOpponentChosen(true);
                terminalScript.setState(TerminalState.Write);
            }else if(gameScript.getPlayerChosen()){
                loopDone=false;
                terminalScript.setState(TerminalState.ReadOnly);
                gameScript.getPlayerMethod().GetComponent<Block>().initBlockStack();
                StartCoroutine(playLoop(player,opponent));
            }
        }else if(gameScript.isGameOver()){
            StopAllCoroutines();
            resetGame();
        }
    }

    IEnumerator playLoop(Character player,Character opponent){
        Block playerMethodBlock=gameScript.getPlayerMethod().GetComponent<Block>();
        ComputerPlayer computerPlayer=gameScript.getComputerPlayer();
        int playerStamina=player.getStamina();
        int opponentStamina=opponent.getStamina();
        while(playerStamina>0||opponentStamina>0){
            ExecutionWrapper playerExecutionWrapper=playerMethodBlock.executeCurrentBlock(opponent,player);
            ExecutionWrapper opponentExecutionWrapper=computerPlayer.executeCurrentBlock(player,opponent);
            GameAction? playerGameAction=playerExecutionWrapper.getGameAction();
            GameAction? opponentGameAction=opponentExecutionWrapper.getGameAction();
            if(playerGameAction!=null&&opponentGameAction!=null&&playerStamina>0&&opponentStamina>0){
                Interaction playerInteraction=gameScript.getInteraction(playerGameAction);
                Interaction opponentInteraction=gameScript.getInteraction(opponentGameAction);
                if(gameScript.getPlayerFirst(playerGameAction,opponentGameAction)){
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
            }else if(playerGameAction!=null&&playerStamina>0){
                Interaction playerInteraction=gameScript.getInteraction(playerGameAction);
                yield return StartCoroutine(playInteractionAfterDelay(0.5f,playerInteraction,player));
            }else if(opponentGameAction!=null&&opponentStamina>0){
                Interaction opponentInteraction=gameScript.getInteraction(opponentGameAction);
                yield return StartCoroutine(playInteractionAfterDelay(0.5f,opponentInteraction,opponent));
            }
            if(!playerExecutionWrapper.getEndOfSection()){
                playerStamina--;
            }
            if(!opponentExecutionWrapper.getEndOfSection()){
                opponentStamina--;
            }
        }
        gameScript.clearPlayerBlockStack();
        gameScript.clearComputerBlockStack();
        gameScript.endTurn();
        loopDone=true;
    }

    IEnumerator playInteractionAfterDelay(float seconds,Interaction interaction,Character character){
        float elapsedSeconds=0f;
        while(elapsedSeconds<seconds){
            elapsedSeconds+=Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(playInteraction(interaction,character));
    }

    IEnumerator playInteraction(Interaction interaction,Character instigator){
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
        yield return null;
    }
    
    IEnumerator waitTime(int seconds){yield return new WaitForSeconds(seconds);}
    
}
