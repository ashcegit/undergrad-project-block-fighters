using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Pointer : MonoBehaviour
{
    Transform _transform;
    Vector3 _mousePos;

    void Awake()
    {
        _transform = transform;
    }

    //void Start()
    //{
    //
    //}

    void Update()
    {
        UpdatePointerPosition();
    }

    public void UpdatePointerPosition()
    {
        _mousePos = Input.mousePosition;
        _transform.position = new Vector3(_mousePos.x, _mousePos.y, _transform.position.z);
    }
}
