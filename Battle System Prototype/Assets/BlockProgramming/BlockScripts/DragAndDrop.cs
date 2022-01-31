using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour,IPointerDownHandler,IDragHandler,IPointerUpHandler
{
    BlockProgrammerScript blockProgrammerScript;
    PointerInSelectionArea pointerInSelectionArea;
    GameObject currentlyDraggedObject;
    Raycaster raycaster;
    BlockSpace nearestBlockSpace;
    InputSpace nearestInputSpace;
    InputFieldHandler currentInputField;
    GameObject ghostBlock;
    Vector2 offset;
    GameObject environment;
    ScrollRect selectionScrollRect;
    GameObject ghostBlockParent;
    const float SNAPDISTANCE=100f;
    bool dragging;
    bool ghostBlockActive;
    bool highlightActive;

    void Awake(){
        blockProgrammerScript=GameObject.FindGameObjectWithTag("BlockProgrammer").GetComponent<BlockProgrammerScript>();
        pointerInSelectionArea=GameObject.FindGameObjectWithTag("BlockSelection").GetComponent<PointerInSelectionArea>();
        selectionScrollRect=GameObject.FindGameObjectWithTag("BlockSelection").GetComponentInParent<ScrollRect>();
        raycaster=GetComponent<Raycaster>();
        ghostBlock=GameObject.FindGameObjectWithTag("GhostBlock");
        environment=GameObject.FindGameObjectWithTag("BlockEnvironment");
    }

    void Start()
    {
        ghostBlock.SetActive(false);
        ghostBlock.transform.SetParent(null);
    }

    public void OnPointerDown(PointerEventData data){
        GameObject draggedExistingBlock=raycaster.getObjectOfTypeAtPosition<Block>((Vector2)data.position);
        GameObject draggedSelectionBlock=raycaster.getObjectOfTypeAtPosition<SelectionBlock>((Vector2)data.position);
        if(draggedExistingBlock!=null){
            currentlyDraggedObject=draggedExistingBlock;
            offset=(Vector2)data.position;
            currentlyDraggedObject.GetComponent<Block>().setPosition(data.position);
        }else if(draggedSelectionBlock!=null){
            selectionScrollRect.StopMovement();
            selectionScrollRect.enabled=false;
            currentlyDraggedObject=Instantiate(draggedSelectionBlock.GetComponent<SelectionBlock>().prefabBlock);
            currentlyDraggedObject.GetComponent<RectTransform>().localScale=new Vector3(1,1,1);
            blockProgrammerScript.addBlockObject(currentlyDraggedObject);
            currentlyDraggedObject.transform.SetParent(environment.transform);
            offset=(Vector2)data.position;
            currentlyDraggedObject.GetComponent<RectTransform>().position=data.position;
        }
        
    }

    public void OnDrag(PointerEventData data){
        if(currentlyDraggedObject!=null){
            BlockType currentBlockType=currentlyDraggedObject.GetComponent<Block>().getBlockType();
            Vector2 cursorPosition = (Vector2)data.position;
            if(currentlyDraggedObject.transform.parent.gameObject!=environment){
                int currentlyDraggedSiblingIndex=currentlyDraggedObject.transform.GetSiblingIndex();
                if(currentBlockType==BlockType.Action||currentBlockType==BlockType.Control){
                    GameObject parentBlock=currentlyDraggedObject;
                    while(parentBlock.transform.parent.GetComponentInParent<Block>()!=null){
                        parentBlock=parentBlock.transform.parent.GetComponentInParent<Block>().gameObject;
                    }
                    parentBlock.GetComponentInChildren<Body>().removeBlockSpaceByIndex(currentlyDraggedSiblingIndex);
                    currentlyDraggedObject.transform.SetParent(environment.transform);
                    parentBlock.GetComponent<Block>().updateBlockLayouts();
                    parentBlock.GetComponent<Block>().updateSpacePositions();
                }else if(currentBlockType!=BlockType.Method){
                    blockProgrammerScript.addBlockObject(currentlyDraggedObject);
                    Header parentHeader=currentlyDraggedObject.GetComponentInParent<Header>();
                    InputFieldHandler inputFieldHandler=parentHeader.getInputFieldHandlerForInputBlock(currentlyDraggedObject);
                    inputFieldHandler.clearInputBlock();
                    parentHeader.updateBlockLayouts();
                }
            }
            
            currentlyDraggedObject.transform.SetParent(environment.transform);
            currentlyDraggedObject.transform.SetAsLastSibling();
            currentlyDraggedObject.GetComponent<Block>().setPosition(cursorPosition);
            currentlyDraggedObject.GetComponent<Block>().setSpacesActive(false);

            if(currentBlockType==BlockType.Action||currentBlockType==BlockType.Control){
                nearestBlockSpace=raycaster.getNearestSpaceToPosition<BlockSpace>(blockProgrammerScript.getBlockSpaces(),
                                                                                    data.position,
                                                                                    SNAPDISTANCE);
                if(nearestBlockSpace.getParentBody()!=null){
                    addGhostBlockToBlockAtSiblingIndex(nearestBlockSpace.getParentBody(),nearestBlockSpace.getIndex());
                    GameObject parentBlock=nearestBlockSpace.getParentBody().GetComponentInParent<Block>().gameObject;
                    while(parentBlock.transform.parent.GetComponentInParent<Block>()!=null){
                        parentBlock=parentBlock.transform.parent.GetComponentInParent<Block>().gameObject;
                    }
                    parentBlock.GetComponent<Block>().updateBlockLayouts();
                    parentBlock.GetComponent<Block>().updateSpacePositions();
                }else{
                    removeGhostBlock();
                }
            }else if(currentBlockType!=BlockType.Method){
                nearestInputSpace=raycaster.getNearestSpaceToPosition<InputSpace>(blockProgrammerScript.getInputSpaces(),
                                                                                    data.position,
                                                                                    SNAPDISTANCE);
                InputFieldHandler inputFieldHandler=nearestInputSpace.getInputFieldHandler();
                if(inputFieldHandler!=null){
                    if(currentBlockType==inputFieldHandler.inputType||
                        currentBlockType==inputFieldHandler.secondaryInputType){
                            highlightInputField(inputFieldHandler);
                        }
                }else{
                    removeInputFieldHighlight();
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData data){
        if(currentlyDraggedObject!=null){
            selectionScrollRect.enabled=true;
            if(pointerInSelectionArea.getPointerIn()){
                blockProgrammerScript.removeBlockObject(currentlyDraggedObject);
            }else{
                BlockType currentBlockType=currentlyDraggedObject.GetComponent<Block>().getBlockType();
                if(currentBlockType==BlockType.Action||currentBlockType==BlockType.Control){
                    if(ghostBlockActive){
                        removeGhostBlock();
                        GameObject nearestBlockSpaceBody=nearestBlockSpace.getParentBody();
                        nearestBlockSpaceBody.GetComponent<Body>().insertBlock(currentlyDraggedObject,nearestBlockSpace.getIndex());
                        nearestBlockSpaceBody.GetComponentInParent<Block>().setSpacesActive(true);
                        GameObject parentBlock=nearestBlockSpaceBody.GetComponentInParent<Block>().gameObject;
                        while(parentBlock.transform.parent.GetComponentInParent<Block>()!=null){
                            parentBlock=parentBlock.transform.parent.GetComponentInParent<Block>().gameObject;
                        }
                        parentBlock.GetComponent<Block>().updateBlockLayouts();
                        parentBlock.GetComponent<Block>().updateSpacePositions();
                    }else{
                        currentlyDraggedObject.GetComponent<Block>().updateBlockLayouts();
                        currentlyDraggedObject.GetComponent<Block>().updateSpacePositions();
                        currentlyDraggedObject.GetComponent<Block>().setSpacesActive(true);
                    }
                }else if(currentBlockType!=BlockType.Method){
                    if(highlightActive){
                        removeInputFieldHighlight();
                        nearestInputSpace.getInputFieldHandler().addInputBlock(currentlyDraggedObject);
                        nearestInputSpace.getInputFieldHandler().GetComponentInParent<Block>().updateBlockLayouts();
                        nearestInputSpace.getInputFieldHandler().GetComponentInParent<Block>().updateSpacePositions();
                        nearestInputSpace.getInputFieldHandler().GetComponentInParent<Block>().setSpacesActive(true);
                    }else{
                        currentlyDraggedObject.GetComponent<Block>().updateBlockLayouts();
                        currentlyDraggedObject.GetComponent<Block>().updateSpacePositions();
                        currentlyDraggedObject.GetComponent<Block>().setSpacesActive(true);
                    }
                }else{
                    currentlyDraggedObject.GetComponent<Block>().setSpacesActive(true);
                }
            }
            nearestBlockSpace=null;
            nearestInputSpace=null;
            currentlyDraggedObject=null;
        }
    }

    public void addGhostBlockToBlockAtSiblingIndex(GameObject blockObject,int siblingIndex){
        ghostBlockActive=true;
        ghostBlock.SetActive(true);
        ghostBlock.transform.SetParent(blockObject.transform);
        ghostBlock.transform.SetSiblingIndex(siblingIndex);
        ghostBlockParent=blockObject;
        ghostBlockParent.GetComponentInParent<Block>().updateBlockLayouts();
    }

    public void removeGhostBlock(){
        ghostBlockActive=false;
        ghostBlock.SetActive(false);
        ghostBlock.transform.SetParent(null);
        if(ghostBlockParent!=null){
            while(ghostBlockParent.transform.parent.GetComponentInParent<Block>()!=null){
                ghostBlockParent=ghostBlockParent.transform.parent.GetComponentInParent<Block>().gameObject;
            }
            ghostBlockParent.GetComponent<Block>().updateBlockLayouts();
            ghostBlockParent.GetComponent<Block>().updateSpacePositions();
        }
        ghostBlockParent=null;
    }

    public void highlightInputField(InputFieldHandler inputFieldHandler){
        if(currentInputField!=null){
            if(currentInputField!=inputFieldHandler){
                currentInputField.setHighlight(false);
            }
        }
        highlightActive=true;
        currentInputField=inputFieldHandler;
        currentInputField.setHighlight(true);
    }

    public void removeInputFieldHighlight(){
        highlightActive=false;
        if(currentInputField!=null){
            currentInputField.setHighlight(false);
        }
    }
}
