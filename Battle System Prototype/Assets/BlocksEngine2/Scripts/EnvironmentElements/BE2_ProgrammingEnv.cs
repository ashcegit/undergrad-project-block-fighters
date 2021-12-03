using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_ProgrammingEnv : MonoBehaviour, I_BE2_ProgrammingEnv
{
    Transform _transform;
    RectTransform _rectTransform;
    public Transform Transform => _transform ? _transform : transform;
    public List<I_BE2_Block> BlocksList { get; set; }
    public BE2_TargetObject targetObject;
    public I_BE2_TargetObject TargetObject => targetObject;

    void Awake()
    {
        _transform = transform;
        _rectTransform = GetComponent<RectTransform>();
        UpdateBlocksList();
    }

    //void Start()
    //{
    //
    //}

    //void Update()
    //{
    //    
    //}

    public void UpdateBlocksList()
    {
        BlocksList = new List<I_BE2_Block>();
        foreach (Transform child in Transform)
        {
            if (child.gameObject.activeSelf)
            {
                I_BE2_Block childBlock = child.GetComponent<I_BE2_Block>();
                if (childBlock != null)
                    BlocksList.Add(childBlock);
            }
        }
    }

    public void OpenContextMenu()
    {
        BE2_UI_ContextMenuManager.instance.OpenContextMenu(1, this);
    }

    public void ClearBlocks()
    {
        BlocksList = new List<I_BE2_Block>();
        foreach (Transform child in Transform)
        {
            if (child.gameObject.activeSelf)
            {
                I_BE2_Block childBlock = child.GetComponent<I_BE2_Block>();
                if (childBlock != null)
                    Destroy(childBlock.Transform.gameObject);
            }
        }

        UpdateBlocksList();
    }
}
