using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BE2_UI_SelectionPanel : MonoBehaviour
{
    LayoutGroup _layoutGroup;
    RectTransform _rectTransform;

    void OnValidate()
    {
        GetComponent<Image>().raycastTarget = false;
        // v2.1 - using BE2_Text to enable usage of Text or TMP components
        BE2_Text.GetBE2Text(transform.GetChild(0)).raycastTarget = false;
    }

    void Awake()
    {
        _layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        UpdateLayout();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            UpdateLayout();
        }
    }
#endif

    void UpdateLayout()
    {
        StartCoroutine(C_UpdateLayout());
    }
    IEnumerator C_UpdateLayout()
    {
        yield return new WaitForEndOfFrame();
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _layoutGroup.preferredHeight);
    }
}
