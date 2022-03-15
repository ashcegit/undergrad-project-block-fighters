using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ComputerPlayer:MonoBehaviour
{
    private GameObject computerPlayerBlocks;
    private GameObject punchPrefab;
    private GameObject kickPrefab;
    private List<Block> blockStack;
    private int pointer;
    string path, attackPath, healPath, statusEffectPath, controlPath, opponentInfoPath, playerInfoPath, logicPath, mathsPath;
    List<List<MethodInfo>> presetMethods;

    public ComputerPlayer(){
        pointer=0;
        blockStack=new List<Block>();
        path = "Prefabs/Blocks/";
        attackPath = path + "Actions/Attacks/Block Action Attack ";
        healPath = path + "Actions/Heals/Block Action Heal ";
        statusEffectPath = path + "Actions/Status Effects/Block Action Status Effect ";
        controlPath = path + "Control/";
        opponentInfoPath = path + "Info/Opponent Info/";
        playerInfoPath = path + "Info/Player Info/";
        logicPath = path + "Logic/";
        mathsPath = path + "Maths";
        computerPlayerBlocks = GameObject.FindGameObjectWithTag("ComputerPlayerBlocks");
        presetMethods = new List<List<MethodInfo>>();

        punchPrefab = (GameObject)Resouces.Load(attackPath + "Punch");
        kickPrefab = (GameObject)Resources.Load(attackPath + "Kick");

        initPresetMethods();
    }

    public void initBlockStack(int levelCounter){
        presetMethods[levelCounter][Random.Range(0, 3)].Invoke(this, new object []{ });
    }

    public void initPresetMethods() {
        //level 1
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("punches"),
            this.GetType().GetMethod("kicks"),
            this.GetType().GetMethod("punchesPlaster"),
            this.GetType().GetMethod("guard")
        });

        //level 2
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("knuckleSandwich"),
            this.GetType().GetMethod("poisonKicks"),
            this.GetType().GetMethod("kicks"),
            this.GetType().GetMethod("guard")
        });

        //level 3
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("laserPunches"),
            this.GetType().GetMethod("guard"),
            this.GetType().GetMethod("knuckleSandwich"),
            this.GetType().GetMethod("poisonKicks")
        });

        //level 4
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("laserPunches"),
            this.GetType().GetMethod("knuckleSandwich"),
            this.GetType().GetMethod("shieldCombo"),
            this.GetType().GetMethod("poisonKicks")
        });

        //level 5
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("punchesPlaster"),
            this.GetType().GetMethod("shieldCombo"),
            this.GetType().GetMethod("sandwichLaser"),
            this.GetType().GetMethod("poisonKicks")
        });

        //level 6

    }

    public void punches(){
        //string of punches
        for (int i=0;i<5;i++){
            addBlockToStack(punchPrefab);
        }
        
        finishSection();
    }

    public void kicks() {
        //string of kicks
        for (int i = 0; i < 5; i++) {
            addBlockToStack(kickPrefab);
        }
        Block endBlock = new Block();
        endBlock.blockType = BlockType.EndOfSection;
        blockStack.Add(endBlock);

        finishSection();
    }

    public void punchesPlaster() {
        //punchx3->fireball->plaster combo
        GameObject fireballPrefab = (GameObject)Resources.Load(attackPath + "Fireball");
        GameObject plasterPrefab = (GameObject)Resources.Load(healPath + "Plaster");

        for (int i = 0; i < 3; i++) {
            addBlockToStack(punchPrefab);
        }
        addBlockToStack(fireballPrefab);
        addBlockToStack(plasterPrefab);
        addBlockToStack(plasterPrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(plasterPrefab);

        finishSection();
    }

    public void guard() {
        //combo of all starting attack blocks + guard
        GameObject guardPrefab = (GameObject)Resources.Load(statusEffectPath + "Guard");
        GameObject fireballPrefab = (GameObject)Resources.Load(attackPath + "Fireball");

        addBlockToStack(guardPrefab);
        addBlockToStack(kickPrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(fireballPrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(fireballPrefab);

        finishSection();
    }

    public void knuckleSandwich() {
        //punchx2->sandwich->punchx2
        GameObject sandwichPrefab = (GameObject)Resources.Load(healPath + "Sandwich");
        GameObject fireballPrefab = (GameObject)Resources.Load(attackPath + "Fireball");

        addBlockToStack(punchPrefab);
        addBlockToStack(fireballPrefab);
        addBlockToStack(sandwichPrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(punchPrefab);

        finishSection();
    }

    public void poisonKicks() {
        //poison->kickx3->poison
        GameObject poisonPrefab = (GameObject)Resources.Load(statusEffectPath + "Poison");
        GameObject kickPrefab = (GameObject)Resources.Load(attackPath + "Poison");

        addBlockToStack(poisonPrefab);
        for (int i = 0; i < 3; i++) { addBlockToStack(kickPrefab); }
        addBlockToStack(poisonPrefab);

        finishSection();
    }

    public void laserPunches() {
        //punchx2->laser->kick->laser
        GameObject laserPrefab = (GameObject)Resources.Load(attackPath + "Laser");

        addBlockToStack(punchPrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(laserPrefab);
        addBlockToStack(kickPrefab);
        addBlockToStack(laserPrefab);

        finishSection();
    }

    public void shieldCombo() {
        //Shield then attack
        GameObject shieldPrefab=(GameObject)Resources.Load(statusEffectPath + "Shield");
        GameObject fireballPrefab = (GameObject)Resources.Load(attackPath + "Fireball");

        addBlockToStack(shieldPrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(fireballPrefab);
        addBlockToStack(kickPrefab);
        addBlockToStack(kickPrefab);
        
        finishSection()
    }

    public void sandwichLaser() {
        //sandwichx2->laser->punchx2
        GameObject sandwichPrefab = (GameObject)Resources.Load(healPath + "Sandwich");
        GameObject laserPrefab = (GameObject)Resources.Load(attackPath + "Laser");

        addBlockToStack(sandwichPrefab);
        addBlockToStack(sandwichPrefab);
        addBlockToStack(laserPrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(punchPrefab);

        finishSection()
    }

    public void lightningKicks() {
        GameObject lightningStrikePrefab = (GameObject)Resources.Load(attackPath + "Lightning Strike");
        
        addBlockToStack(lightningStrikePrefab);
        addBlockToStack(lightningStrikePrefab);
        addBlockToStack(lightningStrikePrefab);
        addBlockToStack(kickPrefab);
        addBlockToStack(kickPrefab);

        finishSection();
    }


    public void addBlockToStack(GameObject blockObject) {
        GameObject newBlock = Instantiate(blockObject);
        newBlock.transform.SetParent(computerPlayerBlocks.transform);
        blockStack.Add(newBlock.GetComponent<Block>());
    }

    public void finishSection() {
        Block endBlock = new Block();
        endBlock.blockType = BlockType.EndOfSection;
        blockStack.Add(endBlock);
    }

    public void clearBlockStack(){
        foreach(Transform childTransform in computerPlayerBlocks.transform) {
            Destroy(childTransform.gameObject);
        }
        blockStack=new List<Block>();
        pointer=0;
    }

    public GameAction? executeCurrentBlock(){
        GameAction gameAction = null;
        if(pointer>=blockStack.Count){
            executionWrapper.setGameAction(null);
        }else{
            Block currentBlock=blockStack[pointer];
            switch(currentBlock.blockType){
                case(BlockType.Action):
                    pointer++;
                    GameScript gameScript=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
                    Character opponent=gameScript.getOpponent();
                    ActionFunction actionFunction=currentBlock.gameObject.GetComponent<ActionFunction>();
                    gameAction=actionFunction.function(opponent,true);
                    break;
                case(BlockType.Control):
                    ControlFunction controlFunction=currentBlock.gameObject.GetComponent<ControlFunction>();
                    pointer=controlFunction.function(pointer,ref blockStack);
                    break;
                case(BlockType.EndOfSection):
                    Block startBlock=currentBlock.getStartBlock();
                    if(startBlock!=null){
                        if(startBlock.getBlockType()==BlockType.Control){
                            ControlFunction endControlFunction=startBlock.gameObject.GetComponent<ControlFunction>();
                            switch(endControlFunction.getName()){
                                case("If Else"):
                                    ControlIfElseFunction controlIfElseFunction=(ControlIfElseFunction)endControlFunction;
                                    pointer=controlIfElseFunction.onRepeat(pointer,ref blockStack);
                                    break;
                                case("Repeat"):
                                    ControlRepeatFunction controlRepeatFunction=(ControlRepeatFunction)endControlFunction;
                                    pointer=controlRepeatFunction.onRepeat(pointer,ref blockStack);
                                    break;
                                case("Repeat Until"):
                                    ControlRepeatUntilFunction controlRepeatUntilFunction=(ControlRepeatUntilFunction)endControlFunction;
                                    pointer=controlRepeatUntilFunction.onRepeat(pointer,ref blockStack);
                                    break;
                                case("Repeat Forever"):
                                    ControlRepeatForeverFunction controlForeverFunction=(ControlRepeatForeverFunction)endControlFunction;
                                    pointer=controlForeverFunction.onRepeat(pointer,ref blockStack);
                                    break;
                            }
                        }
                    }
                    break;
                default:
                    pointer++;
                    break;
            }
        }
        return gameAction;
    }

    public int getBlockStackCount(){return blockStack.Count;}
}
