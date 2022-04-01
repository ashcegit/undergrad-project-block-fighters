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

    private bool loopDone;
    public bool levellingUp;

    private const int maxLevels=10;

    private string terminalWelcomeMessage1 = "Welcome to Block Fighters! Each round you will make the methods for a class of fighter";
    private string terminalWelcomeMessage2 = "Here you can program methods that your character will use to fight 10 battles";
    private string terminalWelcomeMessage3 = "Press tab on this interface for a list of available commands - all commands are in the form \"command()\"";
    private string terminalWelcomeMessage4 = "When you're finished programming, type \"finish()\" to  begin your first battle";
    private string terminalWelcomeMessage5 = "Enter the \"quit()\" command to exit to the main menu";

    void Start(){
        StopAllCoroutines();
        blockProgrammer=GameObject.FindGameObjectWithTag("BlockProgrammer");
        blockProgrammerScript=(BlockProgrammerScript)blockProgrammer.GetComponent<BlockProgrammerScript>();

        gameScript=GetComponentInChildren<GameScript>();

        terminalScript=GetComponentInChildren<Terminal>();

        terminalScript.gameObject.SetActive(false);
        terminalScript.gameObject.SetActive(true);

        levelInfoScript =GetComponentInChildren<LevelInfoScript>();

        gameOverScript = GetComponentInChildren<GameOverScript>();

        winScript = GetComponentInChildren<WinScript>();

        Character player=gameScript.getPlayer();
        Character opponent=gameScript.getOpponent();

        terminalScript.initShell(gameScript);

        Terminal.log(TerminalLogType.Message,"{0}\n",terminalWelcomeMessage1);
        Terminal.log(TerminalLogType.Message, "{0}\n", terminalWelcomeMessage2);
        Terminal.log(TerminalLogType.Message, "{0}\n", terminalWelcomeMessage3);
        Terminal.log(TerminalLogType.Message, "{0}\n", terminalWelcomeMessage4);
        Terminal.log(TerminalLogType.Message, "{0}\n", terminalWelcomeMessage5);

        blockProgrammerScript.displayStamina(gameScript.getPlayer().getStamina());

        loopDone=true;
        terminalScript.setState(TerminalState.Write);
        
        gameScript.enabled=false;
        levelInfoScript.enabled=false;
        gameOverScript.enabled=false;
        gameOverScript.gameObject.SetActive(false);
        winScript.enabled = false;
        winScript.gameObject.SetActive(false);
    }
    
    public void resetGame(){
        enabled=false;
        winScript.enabled = false;
        winScript.gameObject.SetActive(false);
        terminalScript.gameObject.SetActive(true);
        Terminal.Buffer.Clear();
        gameScript.resetGame();
        terminalScript.initShell(gameScript);
        blockProgrammerScript.clearEnvironment();
        openProgramming();
        loopDone=true;
    }

    void gameOver() {
        enabled = false;
        gameScript.enabled = false;
        terminalScript.setState(TerminalState.Close);
        terminalScript.gameObject.SetActive(false);
        gameOverScript.enabled = true;
        gameOverScript.gameObject.SetActive(true);
    }

    void levelDone(){
        enabled=false;
        Terminal.Buffer.Clear();
        if(!gameScript.hasPlayerLost()){
            if (gameScript.getLevelCounter()==maxLevels) {
                gameScript.enabled = false;
                terminalScript.setState(TerminalState.Close);
                terminalScript.gameObject.SetActive(false);
                winScript.gameObject.SetActive(true);
                winScript.enabled = true;
            } else {
                Tuple<List<int>,List<int>> statTuple=gameScript.nextLevel();
                lastPlayerStatIncrease=statTuple.Item1;
                //lastOpponentStatIncrease=statTuple.Item2;
                gameScript.enabled = false;
                terminalScript.setState(TerminalState.Close);
                terminalScript.gameObject.SetActive(false);
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
        terminalScript.gameObject.SetActive(true);
        terminalScript.setState(TerminalState.Write);
        terminalScript.initShell(gameScript);
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
        //plays out interaction between player and opponent
        //in order of action's priority

        //cannot factor out repeated lines because
        //IEnumerators cannot take parameters by reference

        Block playerMethodBlock=gameScript.getPlayerMethod().GetComponent<Block>();
        int playerStamina=player.getStamina();
        int opponentStamina=opponent.getStamina();
        int infLoopDetection = 0;
        if(player.getSpeed()==opponent.getSpeed()){
            //toss up priority if character speeds are equal
            //(the chances of this are slim)
            //if(UnityEngine.Random.Range(0,11)<5){
                Terminal.log(TerminalLogType.Message, "Player Turn\n");
                while (playerStamina > 0 && !gameScript.isGameOver()) {
                    Tuple<GameAction?, bool> execution = playerMethodBlock.executeCurrentBlock();
                    if (execution.Item2) {
                        Debug.Log("!!!! END OF METHOD !!!!");
                        break;
                    }
                    GameAction? playerGameAction = execution.Item1;
                    if (playerGameAction != null) {
                        infLoopDetection = 0;
                        Interaction playerInteraction = gameScript.getInteraction(playerGameAction);
                        yield return StartCoroutine(playInteractionAfterDelay(0.5f, playerInteraction, player));
                        playerStamina--;
                    } else {
                        if (infLoopDetection > 1000) {
                            Terminal.log(TerminalLogType.Error, "Maximum loop amount reached, turn lost");
                            break;    
                        } else {
                            infLoopDetection++;
                        }
                    }
                }
        //        Terminal.log(TerminalLogType.Message, "Opponent Turn\n");
        //        while (opponentStamina>0&&!gameScript.isGameOver()){
        //            Tuple<GameAction?, bool> execution = gameScript.executeCurrentComputerPlayerBlock();
        //            if (execution.Item2) {
        //                break;
        //            } else {
        //                GameAction? opponentGameAction = execution.Item1;
        //                if (opponentGameAction != null && !gameScript.isGameOver()) {
        //                    Interaction opponentInteraction = gameScript.getInteraction(opponentGameAction);
        //                    yield return StartCoroutine(playInteractionAfterDelay(0.5f, opponentInteraction, opponent));
        //                    opponentStamina--;
        //                }
        //            }
        //        }
        //    }else{
        //        Terminal.log(TerminalLogType.Message, "Opponent Turn\n");
        //        while (opponentStamina > 0 && !gameScript.isGameOver()) {
        //            Tuple<GameAction?, bool> execution = gameScript.executeCurrentComputerPlayerBlock();
        //            if (execution.Item2) {
        //                break;
        //            } else {
        //                GameAction? opponentGameAction = execution.Item1;
        //                if (opponentGameAction != null && !gameScript.isGameOver()) {
        //                    Interaction opponentInteraction = gameScript.getInteraction(opponentGameAction);
        //                    yield return StartCoroutine(playInteractionAfterDelay(0.5f, opponentInteraction, opponent));
        //                    opponentStamina--;
        //                }
        //            }
        //        }
        //        Terminal.log(TerminalLogType.Message, "Player Turn\n");
        //        while (playerStamina > 0 && !gameScript.isGameOver()) {
        //            Tuple<GameAction?, bool> execution = playerMethodBlock.executeCurrentBlock();
        //            if (execution.Item2) {
        //                break;
        //            }
        //            GameAction? playerGameAction = execution.Item1;
        //            if (playerGameAction != null) {
        //                infLoopDetection = 0;
        //                Interaction playerInteraction = gameScript.getInteraction(playerGameAction);
        //                yield return StartCoroutine(playInteractionAfterDelay(0.5f, playerInteraction, player));
        //                playerStamina--;
        //            } else {
        //                if (infLoopDetection > 1000) {
        //                    Terminal.log(TerminalLogType.Error, "Maximum loop amount reached, turn lost");
        //                } else {
        //                    infLoopDetection++;
        //                }
        //            }
        //        }
        //    }
        //}else if(player.getSpeed()<opponent.getSpeed()){
        //    Terminal.log(TerminalLogType.Message, "Opponent Turn\n");
        //    while (opponentStamina > 0 && !gameScript.isGameOver()) {
        //        Tuple<GameAction?, bool> execution = gameScript.executeCurrentComputerPlayerBlock();
        //        if (execution.Item2) {
        //            break;
        //        } else {
        //            GameAction? opponentGameAction = execution.Item1;
        //            if (opponentGameAction != null && !gameScript.isGameOver()) {
        //                Interaction opponentInteraction = gameScript.getInteraction(opponentGameAction);
        //                yield return StartCoroutine(playInteractionAfterDelay(0.5f, opponentInteraction, opponent));
        //                opponentStamina--;
        //            }
        //        }
        //    }
        //    Terminal.log(TerminalLogType.Message, "Player Turn\n");
        //    while (playerStamina > 0 && !gameScript.isGameOver()) {
        //        Tuple<GameAction?, bool> execution = playerMethodBlock.executeCurrentBlock();
        //        if (execution.Item2) {
        //            break;
        //        }
        //        GameAction? playerGameAction = execution.Item1;
        //        if (playerGameAction != null) {
        //            infLoopDetection = 0;
        //            Interaction playerInteraction = gameScript.getInteraction(playerGameAction);
        //            yield return StartCoroutine(playInteractionAfterDelay(0.5f, playerInteraction, player));
        //            playerStamina--;
        //        } else {
        //            if (infLoopDetection > 1000) {
        //                Terminal.log(TerminalLogType.Error, "Maximum loop amount reached, turn lost");
        //            } else {
        //                infLoopDetection++;
        //            }
        //        }
        //    }
        //} else{
        //    Terminal.log(TerminalLogType.Message, "Player Turn\n");
        //    while (playerStamina > 0 && !gameScript.isGameOver()) {
        //        Tuple<GameAction?, bool> execution = playerMethodBlock.executeCurrentBlock();
        //        if (execution.Item2) {
        //            break;
        //        }
        //        GameAction? playerGameAction = execution.Item1;
        //        if (playerGameAction != null) {
        //            infLoopDetection = 0;
        //            Interaction playerInteraction = gameScript.getInteraction(playerGameAction);
        //            yield return StartCoroutine(playInteractionAfterDelay(0.5f, playerInteraction, player));
        //            playerStamina--;
        //        } else {
        //            if (infLoopDetection > 1000) {
        //                Terminal.log(TerminalLogType.Error, "Maximum loop amount reached, turn lost");
        //            } else {
        //                infLoopDetection++;
        //            }
        //        }
        //    }
        //    Terminal.log(TerminalLogType.Message, "Opponent Turn\n");
        //    while (opponentStamina > 0 && !gameScript.isGameOver()) {
        //        Tuple<GameAction?, bool> execution = gameScript.executeCurrentComputerPlayerBlock();
        //        if (execution.Item2) {
        //            break;
        //        } else {
        //            GameAction? opponentGameAction = execution.Item1;
        //            if (opponentGameAction != null && !gameScript.isGameOver()) {
        //                Interaction opponentInteraction = gameScript.getInteraction(opponentGameAction);
        //                yield return StartCoroutine(playInteractionAfterDelay(0.5f, opponentInteraction, opponent));
        //                opponentStamina--;
        //            }
        //        }
        //    }
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
                    Terminal.log(TerminalLogType.Action,
                                        "{0}'s attack '{1}' hits for {2} damage!\n",
                                        instigator.getCharacterName(),
                                        attackInteraction.getAttack().getName(),
                                        Mathf.Round(attackInteraction.getDamage()).ToString());
                    gameScript.dealDamage(attackInteraction.getAttack().getTarget(),attackInteraction.getDamage());
                }else{
                    Terminal.log("{0}'s attack '{1}' misses!\n",
                                        instigator.getCharacterName(),
                                        attackInteraction.getAttack().getName());
                }
                break;
            case StatusEffectInteraction statusEffectInteraction:
                if(statusEffectInteraction.getResult()==InteractionEnum.Hit){
                    Terminal.log(TerminalLogType.Action,
                                "{0}'s status effect '{1}' has {2} {3}'s {4} for {6} turns!\n",
                                //character name
                                instigator.getCharacterName(),
                                //status effect name
                                statusEffectInteraction.getStatusEffect().getName(),
                                //raised or decreased
                                (statusEffectInteraction.getMultiplier()>1?"raised ":"decreased "),
                                //target name
                                statusEffectInteraction.getStatusEffect().getTarget().getCharacterName(),
                                //attribute affected
                                statusEffectInteraction.getAttribute().ToString(),
                                //percentage amount - affected by whether attribute is raised or lowered
                                //(statusEffectInteraction.getMultiplier()>1?
                                //     (100*Mathf.Round(statusEffectInteraction.getMultiplier())-100f).ToString():
                                //     (100*Mathf.Round(statusEffectInteraction.getMultiplier())).ToString()),
                                //number of turns
                                statusEffectInteraction.getTurns().ToString());
                    gameScript.addModifier(statusEffectInteraction.getStatusEffect().getTarget(),statusEffectInteraction.getAttributeModifier());
                }else{
                    Terminal.log(TerminalLogType.Action,
                                "{0}'s status effect '{1}' misses!\n",
                                instigator.getCharacterName(),
                                statusEffectInteraction.getStatusEffect().getName());
                }
                break;
            case HealInteraction healInteraction:
                if(healInteraction.getResult()==InteractionEnum.Hit){
                    Terminal.log(TerminalLogType.Action,
                                "{0}'s heal '{1}' heals for {2} health!\n",
                                instigator.getCharacterName(),
                                healInteraction.getHeal().getName(),
                                Mathf.Round(healInteraction.getHealingAmount()).ToString());
                    gameScript.heal(healInteraction.getHeal().getTarget(),healInteraction.getHealingAmount());
                }else{
                    Terminal.log(TerminalLogType.Action,
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
