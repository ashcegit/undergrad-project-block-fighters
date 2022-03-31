using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Header : MonoBehaviour
{
    private List<InputFieldHandler> inputFieldHandlers;
    private Vector2 initialSizeDelta;

    void Awake(){
        RectTransform rectTransform=GetComponent<RectTransform>();
        initialSizeDelta=rectTransform.sizeDelta;
        inputFieldHandlers=new List<InputFieldHandler>();
        foreach(Transform childTransform in transform){
            InputFieldHandler inputFieldHandler=childTransform.gameObject.GetComponent<InputFieldHandler>();
            if(inputFieldHandler!=null){
                inputFieldHandlers.Add(inputFieldHandler);
                inputFieldHandler.Awake();
            }
        }
    }

    public void addInputFieldHandler(InputFieldHandler inputFieldHandler){
        inputFieldHandlers.Add(inputFieldHandler);
    }

    public List<string> getInputStrings(){
        List<string> inputStrings=new List<string>();
        foreach(InputFieldHandler inputFieldHandler in inputFieldHandlers){
            inputStrings.Add(inputFieldHandler.getText());
        }
        return inputStrings;
    }

    public void insertInputBlock(GameObject inputBlock,InputFieldHandler inputFieldHandler){
        int index=inputFieldHandler.transform.GetSiblingIndex();
        inputBlock.transform.SetParent(transform);
        inputBlock.transform.SetSiblingIndex(index+1);
    }

    public void setInputSpacesActive(bool active){
        foreach(Transform childTransform in transform){
            InputFieldHandler inputFieldHandler=childTransform.gameObject.GetComponent<InputFieldHandler>();
            if(inputFieldHandler!=null){
                inputFieldHandler.setInputSpaceActive(active);
            }
        }
    }

    public void updateInputSpacePositions(){
        foreach(Transform childTransform in transform){
            InputFieldHandler inputFieldHandler=childTransform.gameObject.GetComponent<InputFieldHandler>();
            if(inputFieldHandler!=null){
                inputFieldHandler.updateInputSpacePosition();
            }
        }
    }

    public InputFieldHandler getInputFieldHandlerForInputBlock(GameObject inputBlock){
        InputFieldHandler rtn=null;
        foreach(InputFieldHandler inputFieldHandler in inputFieldHandlers){
            if(inputFieldHandler.getInputBlock()==inputBlock){
                rtn=inputFieldHandler;
            }
        }
        return rtn;
    }

    public List<InputFieldHandler> getInputFieldHandlers(){return inputFieldHandlers;}

    public bool areInputFieldHandlersEmpty() {
        bool flag = false;
        foreach(InputFieldHandler inputFieldHandler in inputFieldHandlers) {
            if (inputFieldHandler.getInputBlock() == null&&!inputFieldHandler.textAllowed) {
                flag = true;
            }
        }
        return flag;
    }

    public Vector2 updateBlockLayouts(){
        RectTransform rectTransform=gameObject.GetComponent<RectTransform>();
        Vector2 sizeVector;
        if(transform.parent.transform.GetSiblingIndex()==0){
            Block thisBlock=GetComponentInParent<Block>();
            if(thisBlock!=null){
                if(thisBlock.getBlockType()==BlockType.Method){
                    return gameObject.GetComponent<RectTransform>().sizeDelta;
                }
            }
            float width=15f;
            float height=40f;
            float heightDelta=0f;
            if(inputFieldHandlers.Count(inputFieldHandler=>inputFieldHandler.getInputBlock()!=null)>0){
                width+=GameObject.Find("Text").GetComponent<RectTransform>().sizeDelta.x;
                foreach(InputFieldHandler inputFieldHandler in inputFieldHandlers){
                    inputFieldHandler.updateInputSpacePosition();
                    GameObject? inputBlock=inputFieldHandler.getInputBlock();
                    if(inputBlock!=null){
                        Vector2 inputBlockSize=inputBlock.GetComponent<Block>().getSections()[0].getHeader().updateBlockLayouts();
                        width+=inputBlockSize.x;
                        width+=10f;
                        if(inputBlockSize.y>heightDelta){
                            heightDelta=inputBlockSize.y;
                        }
                    }else{
                        width+=inputFieldHandler.gameObject.GetComponent<RectTransform>().sizeDelta.x;
                        if(initialSizeDelta.y>heightDelta){
                            heightDelta=initialSizeDelta.y;
                        }
                    }
                }
                height+=heightDelta;
            }else{
                width=initialSizeDelta.x;
                height=initialSizeDelta.y;
            }
            sizeVector=new Vector2(width,height);
            rectTransform.sizeDelta=sizeVector;
        }else{
            sizeVector=new Vector2(rectTransform.sizeDelta.x,rectTransform.sizeDelta.y);
        }
        
        return sizeVector;
    }
}
