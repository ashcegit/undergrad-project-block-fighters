using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BE2_DragSelectionBlock : MonoBehaviour, I_BE2_Drag
{
    BE2_DragDropManager _dragDropManager;
    RectTransform _rectTransform;
    BE2_UI_SelectionBlock _uiSelectionBlock;
    ScrollRect _scrollRect;

    Transform _transform;
    public Transform Transform => _transform ? _transform : transform;
    public Vector2 RayPoint => _rectTransform.position;
    public I_BE2_Block Block => null;
    public List<I_BE2_Block> ChildBlocks => null;

    void Awake()
    {
        _transform = transform;
        _rectTransform = GetComponent<RectTransform>();
        _uiSelectionBlock = GetComponent<BE2_UI_SelectionBlock>();
        _scrollRect = GetComponentInParent<ScrollRect>();
    }

    void Start()
    {
        _dragDropManager = BE2_DragDropManager.instance;
        //BE2_DragDropManager.instance.E_OnPointerUpEnd.AddListener(EnableScroll);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypes.OnPointerUpEnd, EnableScroll);
    }

    void OnDisable()
    {
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypes.OnPointerUpEnd, EnableScroll);
    }

    void EnableScroll()
    {
        _scrollRect.enabled = true;
    }

    //void Update()
    //{
    //
    //}

    public void OnPointerDown()
    {

    }

    public void OnRightPointerDownOrHold()
    {
        //BE2_UI_ContextMenuManager.instance.ShowContextMenu(Block);
    }

    public void OnDrag()
    {
        _scrollRect.StopMovement();
        _scrollRect.enabled = false;

        GameObject instantiatedBlock = Instantiate(_uiSelectionBlock.prefabBlock);
        instantiatedBlock.name = _uiSelectionBlock.prefabBlock.name;
        I_BE2_Block block = instantiatedBlock.GetComponent<I_BE2_Block>();
        block.Drag.Transform.SetParent(_dragDropManager.DraggedObjectsTransform, true);

        I_BE2_BlocksStack blocksStack = instantiatedBlock.GetComponent<I_BE2_BlocksStack>();
        if (blocksStack != null)
            blocksStack.MarkToAdd = true;
        instantiatedBlock.transform.localScale = Vector3.one;
        instantiatedBlock.transform.position = transform.position;
        _dragDropManager.CurrentDrag = block.Drag;

        block.Drag.OnPointerDown();
        block.Drag.OnDrag();
    }

    public void OnPointerUp()
    {

    }
}
