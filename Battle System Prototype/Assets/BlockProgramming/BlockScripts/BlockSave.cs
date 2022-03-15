using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSave
{
    List<SelectionBlock> availableBlocks;
    List<SelectionBlock> lockedActionBlocks;
    List<SelectionBlock> lockedNonActionBlocks;
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
        lockedActionBlocks=new List<SelectionBlock>();
        lockedNonActionBlocks = new List<SelectionBlock>();
        GameObject selectionContent=GameObject.FindGameObjectWithTag("SelectionContent");
        SelectionBlock[] selectionBlocks=selectionContent.GetComponentsInChildren<SelectionBlock>();
        foreach(SelectionBlock selectionBlock in selectionBlocks){
            if(!(defaultBlocks.Contains(selectionBlock.gameObject.name))){
                if (selectionBlock.gameObject.name.Contains("Action")) {
                    lockedActionBlocks.Add(selectionBlock);
                } else {
                    lockedNonActionBlocks.Add(selectionBlock);
                }
                selectionBlock.gameObject.SetActive(false);
            }else{
                availableBlocks.Add(selectionBlock);
            }
        }
    }

    public List<SelectionBlock> unlockRandomBlocks(){
        List<SelectionBlock> newSelectionBlocks=new List<SelectionBlock>();
        bool noMoreActionBlocks = false;
        if (lockedActionBlocks.Count > 0) {
            SelectionBlock newBlock = lockedActionBlocks[Random.Range(0, lockedActionBlocks.Count)];
            newBlock.gameObject.SetActive(true);
            lockedActionBlocks.Remove(newBlock);
            availableBlocks.Add(newBlock);
            newSelectionBlocks.Add(newBlock);
        } else {
            noMoreActionBlocks = true;
        }
        for(int i=0;i<(noMoreActionBlocks?3:2);i++){
            if(lockedNonActionBlocks.Count>0){
                SelectionBlock newBlock= lockedNonActionBlocks[Random.Range(0, lockedNonActionBlocks.Count)];
                newBlock.gameObject.SetActive(true);
                lockedNonActionBlocks.Remove(newBlock);
                availableBlocks.Add(newBlock);
                newSelectionBlocks.Add(newBlock);
            }
        }
        return newSelectionBlocks;
    }

    public void refreshSelectionBlocks(){
        foreach (SelectionBlock selectionBlock in lockedActionBlocks) {
            selectionBlock.gameObject.SetActive(false);
        }
        foreach (SelectionBlock selectionBlock in lockedNonActionBlocks) {
            selectionBlock.gameObject.SetActive(false);
        }
        foreach(SelectionBlock selectionBlock in availableBlocks) {
            selectionBlock.gameObject.SetActive(true);
        }
    }
}
