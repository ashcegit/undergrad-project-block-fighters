using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUnlock : MonoBehaviour
{
    List<BlockUnlockSpace> blockUnlockSpaces;

    void Awake() {
        blockUnlockSpaces = new List<BlockUnlockSpace>(GetComponentsInChildren<BlockUnlockSpace>());
    }

    public void displayBlocks(List<SelectionBlock> newSelectionBlocks) {
        for (int i = 0; i < newSelectionBlocks.Count; i++) {
            blockUnlockSpaces[i].displayBlock(newSelectionBlocks[i]);
        }
    }
}
