using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBlockInfoScript : MonoBehaviour
{

    BlockUnlock blockUnlock;

    void Awake() {
        blockUnlock = GetComponentInChildren<BlockUnlock>();
        gameObject.SetActive(false);
    }

    void OnEnable() {
        gameObject.SetActive(true);
    }

    void OnDisable() {
        gameObject.SetActive(false);
    }

    public void setNewBlocks(List<SelectionBlock> newSelectionBlocks) {
        blockUnlock.displayBlocks(newSelectionBlocks);
    }

    public void nextLevel() {
        gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Main").GetComponent<Main>().nextLevel();
    }
}
