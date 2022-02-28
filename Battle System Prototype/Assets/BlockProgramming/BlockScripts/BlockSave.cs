using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSave
{
    List<SelectionBlock> availableBlocks;
    List<SelectionBlock> lockedBlocks;
    List<string> defaultBlocks=new List<string>(){
                                    "Block Method Starter",
                                    "Block Action Attack Punch",
                                    "Block Action Attack Kick",
                                    "Block Action Status Effect Guard",
                                    "Block Action Heal Plaster",
                                    "Block Ins If",
                                    "Block Ins RepeatForever",
                                    "Block Ins BreakLoop",
                                    "Block Math Sum",
                                    "Block Logic True",
                                    "Block Logic False",
                                    "Block Info Number",
                                    "Block Character Player",
                                    "Block Character Opponent"
                                };
    
    public BlockSave(){
        availableBlocks=new List<SelectionBlock>();
        lockedBlocks=new List<SelectionBlock>();
        GameObject selectionContent=GameObject.FindGameObjectWithTag("SelectionContent");
        SelectionBlock[] selectionBlocks=selectionContent.GetComponentsInChildren<SelectionBlock>();
        foreach(SelectionBlock selectionBlock in selectionBlocks){
            if(!(defaultBlocks.Contains(selectionBlock.gameObject.name))){
                lockedBlocks.Add(selectionBlock);
                selectionBlock.gameObject.SetActive(false);
            }else{
                availableBlocks.Add(selectionBlock);
            }
        }
    }

    public List<SelectionBlock> unlockRandomBlocks(){
        List<SelectionBlock> newSelectionBlocks=new List<SelectionBlock>();
        for(int i=0;i<3;i++){
            if(lockedBlocks.Count>0){
                SelectionBlock newBlock=lockedBlocks[Random.Range(0,lockedBlocks.Count)];
                newBlock.gameObject.SetActive(true);
                lockedBlocks.Remove(newBlock);
                availableBlocks.Add(newBlock);
                newSelectionBlocks.Add(newBlock);
            }
        }
        return newSelectionBlocks;
    }

    public void refreshSelectionBlocks(){
        foreach(SelectionBlock selectionBlock in lockedBlocks){
            selectionBlock.gameObject.SetActive(false);
        }
        foreach(SelectionBlock selectionBlock in availableBlocks) {
            selectionBlock.gameObject.SetActive(true);
        }
    }
}
