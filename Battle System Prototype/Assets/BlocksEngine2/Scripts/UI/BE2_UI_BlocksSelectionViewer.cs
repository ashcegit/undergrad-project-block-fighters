using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BE2_UI_BlocksSelectionViewer : MonoBehaviour
{
    public static BE2_UI_BlocksSelectionViewer instance;
    public List<BE2_UI_SelectionPanel> selectionPanelsList;
    [Header("Add Block To Panel")]
    public Transform blockToAddTransform;
    public int panelIndex;
    public bool addBlock = false;

    void Awake()
    {
        instance = this;
        selectionPanelsList = new List<BE2_UI_SelectionPanel>();
    }

    void Start()
    {
        selectionPanelsList.AddRange(GetComponentsInChildren<BE2_UI_SelectionPanel>());
    }

    void Update()
    {
        if (addBlock)
        {

            AddBlockToPanel(blockToAddTransform, selectionPanelsList[panelIndex]);
            addBlock = false;
        }
    }

    public void UpdateSelectionPanels()
    {
        selectionPanelsList = new List<BE2_UI_SelectionPanel>();
        selectionPanelsList.AddRange(GetComponentsInChildren<BE2_UI_SelectionPanel>());
    }

    public void AddBlockToPanel(Transform blockTransform, BE2_UI_SelectionPanel selectionPanel)
    {
        Transform blockCopy = Instantiate(blockTransform, Vector3.zero, Quaternion.identity, selectionPanel.transform);
        blockCopy.name = blockCopy.name.Replace("(Clone)", "");

        BE2_BlockUtils.RemoveEngineComponents(blockCopy);
        BE2_BlockUtils.AddSelectionMenuComponents(blockCopy);
        Debug.Log("+ Block added to selection menu");

        GameObject prefabBlock = BE2_BlockUtils.CreatePrefab(blockTransform.GetComponent<I_BE2_Block>());
        blockCopy.GetComponent<BE2_UI_SelectionBlock>().prefabBlock = prefabBlock;
        Debug.Log("+ Block prefab created");
    }
}
