using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
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

        punchPrefab = (GameObject)Resources.Load(attackPath + "Punch");
        kickPrefab = (GameObject)Resources.Load(attackPath + "Kick");

        initPresetMethods();
    }

    public void initBlockStack(int levelCounter){
        pointer = 0;
        presetMethods[levelCounter][UnityEngine.Random.Range(0, 3)].Invoke(this, new object []{ });
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
            this.GetType().GetMethod("punches"),
            this.GetType().GetMethod("kicks"),
            this.GetType().GetMethod("guard")
        });

        //level 3
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("coffeeBreak"),
            this.GetType().GetMethod("guard"),
            this.GetType().GetMethod("knuckleSandwich"),
            this.GetType().GetMethod("coffeeBreak")
        });

        //level 4
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("coffeeBreak"),
            this.GetType().GetMethod("punches"),
            this.GetType().GetMethod("guard"),
            this.GetType().GetMethod("poisonKicks")
        });

        //level 5
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("coffeeBreak"),
            this.GetType().GetMethod("wardCombo"),
            this.GetType().GetMethod("guard"),
            this.GetType().GetMethod("poisonKicks")
        });

        //level 6
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("coffeeBreak"),
            this.GetType().GetMethod("wardCombo"),
            this.GetType().GetMethod("guard"),
            this.GetType().GetMethod("lightningKicks")
        });

        //level 7
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("lightningKicks"),
            this.GetType().GetMethod("wardCombo"),
            this.GetType().GetMethod("coffeeBreak"),
            this.GetType().GetMethod("laserPunches")
        });

        //level 8
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("lightningKicks"),
            this.GetType().GetMethod("shieldCombo"),
            this.GetType().GetMethod("coffeeBreak"),
            this.GetType().GetMethod("laserPunches")
        });

        //level 9
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("lightningKicks"),
            this.GetType().GetMethod("shieldCombo"),
            this.GetType().GetMethod("coffeeBreak"),
            this.GetType().GetMethod("freezingMedicide")
        });

        //level 10
        presetMethods.Add(new List<MethodInfo> {
            this.GetType().GetMethod("lightningKicks"),
            this.GetType().GetMethod("shieldCombo"),
            this.GetType().GetMethod("freezingMedicine"),
            this.GetType().GetMethod("coffeeCatapult")
        });

    }

    //Hardcoded methods for opponent to use

    public void punches(){
        //string of punches
        for (int i=0;i<7;i++){
            addBlockToStack(punchPrefab);
        }
        
        finishSection();
    }

    public void kicks() {
        //string of kicks
        for (int i = 0; i < 7; i++) {
            addBlockToStack(kickPrefab);
        }

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

        addBlockToStack(guardPrefab);
        addBlockToStack(kickPrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(kickPrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(kickPrefab);

        finishSection();
    }

    public void wardCombo() {
        GameObject wardSpellPrefab = (GameObject)Resources.Load(statusEffectPath + "Ward Spell");

        for (int i = 0; i < 2; i++) { addBlockToStack(wardSpellPrefab); }
        for (int i = 0; i < 3; i++) { addBlockToStack(punchPrefab); }
        for (int i = 0; i < 2; i++) { addBlockToStack(kickPrefab); }
        for (int i = 0; i < 2; i++) { addBlockToStack(punchPrefab); }

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
        addBlockToStack(punchPrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(sandwichPrefab);

        finishSection();
    }

    public void poisonKicks() {
        //poison->kickx3->poison
        GameObject poisonPrefab = (GameObject)Resources.Load(statusEffectPath + "Poison");
        GameObject kickPrefab = (GameObject)Resources.Load(attackPath + "Poison");

        addBlockToStack(poisonPrefab);
        for (int i = 0; i < 4; i++) { addBlockToStack(kickPrefab); }
        addBlockToStack(poisonPrefab);
        addBlockToStack(poisonPrefab);
        for (int i = 0; i < 3; i++) { addBlockToStack(kickPrefab); }

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

        finishSection();
    }

    public void sandwichLaser() {
        //sandwichx2->laserx4->punchx3
        GameObject sandwichPrefab = (GameObject)Resources.Load(healPath + "Sandwich");
        GameObject laserPrefab = (GameObject)Resources.Load(attackPath + "Laser");

        addBlockToStack(sandwichPrefab);
        addBlockToStack(sandwichPrefab);
        for (int i = 0; i < 4; i++) { addBlockToStack(laserPrefab); }
        for (int i = 0; i < 3; i++) { addBlockToStack(punchPrefab); }

        finishSection();
    }

    public void coffeeBreak() {
        GameObject coffeePrefab = (GameObject)Resources.Load(statusEffectPath + "Coffee");

        addBlockToStack(coffeePrefab);
        addBlockToStack(coffeePrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(kickPrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(coffeePrefab);
        addBlockToStack(punchPrefab);
        addBlockToStack(kickPrefab);
        for (int i = 0; i < 3; i++) { addBlockToStack(punchPrefab); }

        finishSection();
    }

    public void lightningKicks() {
        GameObject lightningStrikePrefab = (GameObject)Resources.Load(attackPath + "Lightning Strike");

        for (int i = 0; i < 3; i++) { addBlockToStack(lightningStrikePrefab); }
        for (int i = 0; i < 3; i++) { addBlockToStack(kickPrefab); }
        for (int i = 0; i < 2; i++) { addBlockToStack(lightningStrikePrefab); }

        finishSection();
    }

    public void freezingMedicine() {
        GameObject freezingSpellPrefab = (GameObject)Resources.Load(statusEffectPath + "Freezing Spell");
        GameObject medicinePrefab = (GameObject)Resources.Load(healPath + "Medicine");

        for (int i = 0; i < 3; i++) { addBlockToStack(freezingSpellPrefab); }
        for (int i = 0; i < 2; i++) { addBlockToStack(medicinePrefab); }
        for (int i = 0; i < 3; i++) { addBlockToStack(punchPrefab); }
        for (int i = 0; i < 2; i++) { addBlockToStack(kickPrefab); }

        finishSection();
    }

    public void coffeeCatapult() {
        GameObject coffeePrefab = (GameObject)Resources.Load(statusEffectPath + "Coffee");
        GameObject catapultPrefab = (GameObject)Resources.Load(attackPath + "Catapult");
        GameObject shieldPrefab = (GameObject)Resources.Load(statusEffectPath + "Shield");

        for (int i = 0; i < 3; i++) { addBlockToStack(coffeePrefab); }
        for (int i = 0; i < 2; i++) { addBlockToStack(catapultPrefab); }
        for (int i = 0; i < 3; i++) { addBlockToStack(punchPrefab); }
        for (int i = 0; i < 2; i++) { addBlockToStack(shieldPrefab); }

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
        blockStack.Clear();
        pointer=0;
    }

    public Tuple<GameAction?, bool> executeCurrentBlock() {
        //returns a tuple of a game action and whether the method has come to an end
        GameAction gameAction = null;
        if (pointer >= blockStack.Count - 1) {
            return new Tuple<GameAction?, bool>(gameAction, true);
        } else {
            Block currentBlock = blockStack[pointer];
            switch (currentBlock.blockType) {
                case (BlockType.Action):
                    pointer++;
                    GameScript gameScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
                    Character opponent = gameScript.getOpponent();
                    ActionFunction actionFunction = currentBlock.gameObject.GetComponent<ActionFunction>();
                    gameAction = actionFunction.function(opponent, true);
                    break;
                case (BlockType.Control):
                    ControlFunction controlFunction = currentBlock.gameObject.GetComponent<ControlFunction>();
                    pointer = controlFunction.function(pointer, ref blockStack);
                    break;
                case (BlockType.EndOfSection):
                    Block startBlock = currentBlock.getStartBlock();
                    if (startBlock != null) {
                        if (startBlock.getBlockType() == BlockType.Control) {
                            ControlFunction endControlFunction = startBlock.gameObject.GetComponent<ControlFunction>();
                            pointer = endControlFunction.onRepeat(pointer, ref blockStack);
                        }
                    } else {
                        pointer++;
                    }
                    break;
                default:
                    pointer++;
                    break;
            }
        }
        return new Tuple<GameAction?, bool>(gameAction, false);
    }


    public int getBlockStackCount(){return blockStack.Count;}
}
