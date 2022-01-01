using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldHandler : MonoBehaviour
{

    public int inputFieldNumber;
    private InputField inputField;
    private Header header;

    void Awake(){
        inputField=GetComponent<InputField>();
        header=transform.parent.gameObject.GetComponent<Header>();
    }

    // Start is called before the first frame update
    void Start()
    {
        inputField.onValueChanged.AddListener(delegate{valueChange();});
    }

    void valueChange(){
        header.changeValueAtIndex(inputField.text,inputFieldNumber);
    }
}
