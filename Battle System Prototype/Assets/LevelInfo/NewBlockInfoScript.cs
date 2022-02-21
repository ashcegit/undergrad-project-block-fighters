using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBlockInfoScript : MonoBehaviour
{

    List<SelectionBlock> newBlocks;
    List<GameObject> unlockedBlockSpaces;

    void Awake() {
        unlockedBlockSpaces = new List<GameObject>();
        foreach(Transform childTransform in transform) {
            unlockedBlockSpaces.Add(childTransform.gameObject);
        }
        newBlocks = new List<SelectionBlock>();
    }

    void OnEnable() {
        gameObject.SetActive(true);
    }

    void OnDisable() {
        gameObject.SetActive(false);
    }

    public void setNewBlocks(List<SelectionBlock> newSelectionBlocks) {
        newBlocks = newSelectionBlocks;
    }

    public void displayBlocks() {
        for (int i = 0; i < newBlocks.Count; i++) {
            Debug.Log(newBlocks[i].gameObject.name);
            unlockedBlockSpaces[i] = Instantiate(newBlocks[i].gameObject);
        }
    }

    public void nextLevel() {
        gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Main").GetComponent<Main>().nextLevel();
    }
}
