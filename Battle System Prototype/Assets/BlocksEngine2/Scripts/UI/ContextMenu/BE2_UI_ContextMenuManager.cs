using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BE2_UI_ContextMenuManager : MonoBehaviour
{
    I_BE2_UI_ContextMenu[] _contextMenuArray;
    I_BE2_UI_ContextMenu currentContextMenu;

    public static BE2_UI_ContextMenuManager instance;
    public BE2_UI_PanelCancel panelCancel;
    public bool isActive = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _contextMenuArray = new I_BE2_UI_ContextMenu[0];
        foreach (Transform child in transform)
        {
            I_BE2_UI_ContextMenu context = child.GetComponent<I_BE2_UI_ContextMenu>();
            if (context != null)
                BE2_ArrayUtils.Add(ref _contextMenuArray, context);
        }

        CloseContextMenu();
    }

    //void Update()
    //{
    //
    //}

    public void OpenContextMenu<T>(int menuIndex, T target, params string[] options)
    {
        if (!isActive)
        {
            currentContextMenu = _contextMenuArray[menuIndex];
            currentContextMenu.Open(target);
            isActive = true;
            panelCancel.transform.gameObject.SetActive(true);
        }
    }

    public void CloseContextMenu()
    {
        if (isActive)
        {
            if (currentContextMenu != null)
            {
                currentContextMenu.Close();
                currentContextMenu = null;
            }
            isActive = false;
            panelCancel.transform.gameObject.SetActive(false);
        }
    }
}
