using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BE2_InputFieldDynamicResize : MonoBehaviour
{
    RectTransform _rectTransform;
    InputField _inputField;
    float _minWidth = 70;
    float _offset = 35;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _inputField = GetComponent<InputField>();
    }

    void Start()
    {

    }

    //#if UNITY_EDITOR
    //    void Update()
    //    {
    //        if (!EditorApplication.isPlaying)
    //        {
    //            Resize();
    //        }
    //    }
    //#endif

    void OnEnable()
    {
        _inputField.onValueChanged.AddListener(delegate { Resize(); });
    }

    void OnDisable()
    {
        _inputField.onValueChanged.RemoveAllListeners();
    }

    public void Resize()
    {
        float width = _offset + _inputField.preferredWidth;
        if (width < _minWidth)
            width = _minWidth;

        _rectTransform.sizeDelta = new Vector2(width, _rectTransform.sizeDelta.y);
    }
}
