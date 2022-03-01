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

    private GameScript gameScript;

    private GameObject playerMethod;
    private GameObject opponentMethod;

    private List<int> lastPlayerStatIncrease;

    private List<int> lastOpponentStatIncrease;

    private Terminal terminalScript;

    private LevelInfoScript levelInfoScript;

    private GameOverScript gameOverScript;

    private WinScript winScript;

    private InteractionHandler interactionHandler;

    private bool loopDone;
    public bool levellingUp;

    private const int maxLevels=9;

    private string terminalWelcomeMessage1 = "Welcome to _, here you can program methods that your character will use to fight 10 battles";
    private string terminalWelcomeMessage2 = "Press tab on this interface for a list of available commands - all commands are in the form \"command()\"";
    private string terminalWelcomeMessage3 = "When you're finished programming, type \"finish()\" to  begin your first battle";

    void Start(){
        StopAllCoroutines();
        blockProgrammer=GameObject.FindGameObjectWithTag("BlockProgrammer");
        blockProgrammerScript=(BlockProgrammerScript)blockProgrammer.GetComponent<BlockProgrammerScript>();

        gameScript=GetComponentInChildren<GameScript>();

        terminalScript=GetComponentInChildren<Terminal>();

        levelInfoScript=GetComponentInChildren<LevelInfoScript>();

        gameOverScript = GetComponentInChildren<GameOverScript>();

        winScript = GetComponentInChildren<WinScript>();

        interactionHandler=new InteractionHandler();

        Character player=gameScript.getPlayer();
        Character opponent=gameScript.getOpponent();

        terminalScript.initShell(gameScript);

        Terminal.log(TerminalLogType.Message,"{0}\n",terminalWelcomeMessage1);
        Terminal.log(TerminalLogType.Message, "{0}\n", terminalWelcomeMessage2);
        Terminal.log(TerminalLogType.Message, "{0}\n", terminalWelcomeMessage3);

        blockProgrammerScript.displayStamina(gameScript.getPlayer().getStamina());

        loopDone=true;
        terminalScript.setState(TerminalState.Write);
        enabled=false;
        gameScript.enabled=false;
        levelInfoScript.enabled=false;
        gameOverScript.enabled=false;
        gameOverScript.gameObject.SetActive(false);
        winScript.enabled = false;
        winScript.gameObject.SetActive(false);
    }
    
    //void resetGame(){
    //    enabled=false;
    //    Terminal.Buffer.Clear();
    //    gameScript.resetGame();
    //    terminalScript.initShell(gameScript);
    //    openProgramming();
    //    loopDone=true;
    //}

    void gameOver() {
        enabled = false;
        gameScript.enabled = false;
        terminalScript.setState(TerminalState.Close);
        gameOverScript.enabled = true;
        gameOverScript.gameObject.SetActive(true);
    }

    void levelDone(){
        enabled=false;
        Terminal.Buffer.Clear();
        if(!gameScript.hasPlayerLost()){
            if (gameScript.getLevelCounter()>8) {
                gameScript.enabled = false;
                terminalScript.setState(TerminalState.Close);
                winScript.gameObject.SetActive(true);
                winScript.enabled = true;
            } else {
                Tuple<List<int>,List<int>> statTuple=gameScript.nextLevel();
                lastPlayerStatIncrease=statTuple.Item1;
                //lastOpponentStatIncrease=statTuple.Item2;
                gameScript.enabled = false;
                terminalScript.setState(TerminalState.Close);
                levelInfoScript.gameObject.SetActive(true);
                levelInfoScript.enabled = true;
                List<SelectionBlock> newSelectionBlocks = blockProgrammerScript.unlockRandomBlocks();
                levelInfoScript.setNewBlocks(newSelectionBlocks);
                levelInfoScript.levelUp(gameScript.getLevelCounter(), maxLevels);
            }
        }else{
            gameOver();
        }
    }

    public void nextLevel(){
        terminalScript.setState(TerminalState.Write);
        terminalScript.initShell(gameScript);
        levelInfoScript.enabled=false;
        if (gameScript.getLevelCounter() > 0) {
            Terminal.log(TerminalLogType.Message,"Stats increased!");
            Terminal.log(TerminalLogType.Message, "Max Health increased by: {0}",lastPlayerStatIncrease[0]);
            Terminal.log(TerminalLogType.Message, "Attack increased by: {0}", lastPlayerStatIncrease[1]);
            Terminal.log(TerminalLogType.Message, "Defence increased by: {0}", lastPlayerStatIncrease[2]);
            Terminal.log(TerminalLogType.Message, "Speed increased by: {0}", lastPlayerStatIncrease[3]);
            Terminal.log(TerminalLogType.Message, "Stamina increased by 2");
        }
        openProgramming();
        loopDone=true;
    }

    public void openProgramming(){
        terminalScript.setState(TerminalState.Write);
        blockProgrammerScript.gameObject.SetActive(true);
        blockProgrammerScript.displayStamina(gameScript.getPlayer().getStamina());
        blockProgrammerScript.enabled=true;
        gameScript.enabled=false;
    }

    public void finishProgramming(){
        terminalScript.setState(TerminalState.Write);
        terminalScript.registerCharacterCommands(blockProgrammerScript.getMethodBlockObjects());
        
        blockProgrammerScript.enabled=false;

        Character player=gameScript.getPlayer();
        Character opponent=gameScript.getOpponent();

        gameScript.updatePlayerName(player.getCharacterName());
        gameScript.updateOpponentName(opponent.getCharacterName());

        gameScript.updatePlayerHealth();
        gameScript.updateOpponentHealth();

        gameScript.enabled=true;
        enabled=true;
    }

    void Update(){
        if(gameScript.getCharactersLoaded()&&!gameScript.isGameOver()&&loopDone){
            StopAllCoroutines();
            Character player=gameScript.getPlayer();
            Character opponent=gameScript.getOpponent();

            if(!gameScript.getOpponentChosen()){
                gameScript.initComputerPlayerBlockStack();
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
            levelDone();
        }
    }

    IEnumerator playLoop(Character player,Character opponent){
        //cannot modularise because IEnumerators cannot take ref parameters : (
        Block playerMethodBlock=gameScript.getPlayerMethod().GetComponent<Block>();
        int playerStamina=player.getStamina();
        int opponentStamina=opponent.getStamina();
        if(player.getSpeed()==opponent.getSpeed()){
            if(UnityEngine.Random.Range(0,1)<0.5){
                while(playerStamina>0&&!gameScript.isGameOver()){
                    ExecutionWrapper playerExecutionWrapper=playerMethodBlock.executeCurrentBlock();
                    GameAction? playerGameAction=playerExecutionWrapper.getGameAction();
                    if(playerGameAction!=null&&!gameScript.isGameOver()){
                        Interaction playerInteraction=gameScript.getInteraction(playerGameAction);
                        yield return StartCoroutine(playInteractionAfterDelay(0.5f,playerInteraction,player));
                    }
                    if(!playerExecutionWrapper.getEndOfSection()){
                        playerStamina--;
                    }
                }
                while(opponentStamina>0&&!gameScript.isGameOver()){
                    ExecutionWrapper opponentExecutionWrapper=gameScript.executeCurrentComputerPlayerBlock();
                    GameAction? opponentGameAction=opponentExecutionWrapper.getGameAction();
                    if(opponentGameAction!=null&&!gameScript.isGameOver()){
                        Interaction opponentInteraction=gameScript.getInteraction(opponentGameAction);
                        yield return StartCoroutine(playInteractionAfterDelay(0.5f,opponentInteraction,opponent));
                    }
                    if(!opponentExecutionWrapper.getEndOfSection()){
                        opponentStamina--;
                    }
                }
            }else{
                while(opponentStamina>0&&!gameScript.isGameOver()){
                    ExecutionWrapper opponentExecutionWrapper=gameScript.executeCurrentComputerPlayerBlock();
                    GameAction? opponentGameAction=opponentExecutionWrapper.getGameAction();
                    if(opponentGameAction!=null&&!gameScript.isGameOver()){
                        Interaction opponentInteraction=gameScript.getInteraction(opponentGameAction);
                        yield return StartCoroutine(playInteractionAfterDelay(0.5f,opponentInteraction,opponent));
                    }
                    if(!opponentExecutionWrapper.getEndOfSection()){
                        opponentStamina--;
                    }
                }
                while(playerStamina>0&&!gameScript.isGameOver()){
                    ExecutionWrapper playerExecutionWrapper=playerMethodBlock.executeCurrentBlock();
                    GameAction? playerGameAction=playerExecutionWrapper.getGameAction();
                    if(playerGameAction!=null&&!gameScript.isGameOver()){
                        Interaction playerInteraction=gameScript.getInteraction(playerGameAction);
                        yield return StartCoroutine(playInteractionAfterDelay(0.5f,playerInteraction,player));
                    }
                    if(!playerExecutionWrapper.getEndOfSection()){
                        playerStamina--;
                    }
                }
            }
        }else if(player.getSpeed()<opponent.getSpeed()){
            while(opponentStamina>0&&!gameScript.isGameOver()){
                ExecutionWrapper opponentExecutionWrapper=gameScript.executeCurrentComputerPlayerBlock();
                GameAction? opponentGameAction=opponentExecutionWrapper.getGameAction();
                if(opponentGameAction!=null&&!gameScript.isGameOver()){
                    Interaction opponentInteraction=gameScript.getInteraction(opponentGameAction);
                    yield return StartCoroutine(playInteractionAfterDelay(0.5f,opponentInteraction,opponent));
                }
                if(!opponentExecutionWrapper.getEndOfSection()){
                    opponentStamina--;
                }
            }
            while(playerStamina>0&&!gameScript.isGameOver()){
                ExecutionWrapper playerExecutionWrapper=playerMethodBlock.executeCurrentBlock();
                GameAction? playerGameAction=playerExecutionWrapper.getGameAction();
                if(playerGameAction!=null&&!gameScript.isGameOver()){
                    Interaction playerInteraction=gameScript.getInteraction(playerGameAction);
                    yield return StartCoroutine(playInteractionAfterDelay(0.5f,playerInteraction,player));
                }
                if(!playerExecutionWrapper.getEndOfSection()){
                    playerStamina--;
                }
            }
        }else{
            while(playerStamina>0&&!gameScript.isGameOver()){
                    ExecutionWrapper playerExecutionWrapper=playerMethodBlock.executeCurrentBlock();
                    GameAction? playerGameAction=playerExecutionWrapper.getGameAction();
                    if(playerGameAction!=null&&!gameScript.isGameOver()){
                        Interaction playerInteraction=gameScript.getInteraction(playerGameAction);
                        yield return StartCoroutine(playInteractionAfterDelay(0.5f,playerInteraction,player));
                    }
                    if(!playerExecutionWrapper.getEndOfSection()){
                        playerStamina--;
                    }
                }
                while(opponentStamina>0&&!gameScript.isGameOver()){
                    ExecutionWrapper opponentExecutionWrapper=gameScript.executeCurrentComputerPlayerBlock();
                    GameAction? opponentGameAction=opponentExecutionWrapper.getGameAction();
                    if(opponentGameAction!=null&&!gameScript.isGameOver()){
                        Interaction opponentInteraction=gameScript.getInteraction(opponentGameAction);
                        yield return StartCoroutine(playInteractionAfterDelay(0.5f,opponentInteraction,opponent));
                    }
                    if(!opponentExecutionWrapper.getEndOfSection()){
                        opponentStamina--;
                    }
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
                                        Mathf.Round(attackInteraction.getDamage()).ToString());
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
                                     (100*Mathf.Round(statusEffectInteraction.getMultiplier())-100f).ToString():
                                     (100*Mathf.Round(statusEffectInteraction.getMultiplier())).ToString()),
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
                                Mathf.Round(healInteraction.getHealingAmount()).ToString());
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
