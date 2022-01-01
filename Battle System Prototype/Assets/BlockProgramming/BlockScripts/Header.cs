using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Header : MonoBehaviour
{
    private List<string> inputStrings;

    void Awake(){
        inputStrings=new List<string>();
        foreach(Transform childTransform in transform){
            if(childTransform.GetComponent<InputFieldHandler>()!=null){
                inputStrings.Add("");
            }
        }
    }

    public void changeValueAtIndex(string inputValue,int index){
        inputStrings[index]=inputValue;
    }

    public List<string> getInputStrings(){return inputStrings;}
}
