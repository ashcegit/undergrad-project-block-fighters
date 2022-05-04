using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSave
{
    List<SelectionBlock> availableBlocks;
    List<SelectionBlock> lockedActionBlocks;
    List<SelectionBlock> lockedNonActionBlocks;
    List<string> defaultBlocks=new List<string>(){
                                    "Method Starter",
                                    "Punch",
                                    "Kick",
                                    "Guard",
                                    "Plaster",
                                    "If",
                                    "RepeatForever",
                                    "BreakLoop",
                                    "Sum",
                                    "Less Than",
                                    "True",
                                    "False",
                                    "Number",
                                    "Player Health",
                                    "Opponent Health",
                                    "Player",
                                    "Opponent"
                                };
    const bool unlockAll = false;
    
    public BlockSave(){
        availableBlocks=new List<SelectionBlock>();
        lockedActionBlocks=new List<SelectionBlock>();
        lockedNonActionBlocks = new List<SelectionBlock>();
        GameObject selectionContent=GameObject.FindGameObjectWithTag("SelectionContent");
        SelectionBlock[] selectionBlocks=selectionContent.GetComponentsInChildren<SelectionBlock>();
        foreach(SelectionBlock selectionBlock in selectionBlocks){
            if(!(unlockAll||defaultBlocks.Contains(selectionBlock.gameObject.name))){
                if (selectionBlock.prefabBlock.name.Contains("Action")) {
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
