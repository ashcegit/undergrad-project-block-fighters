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
    const float SNAPDISTANCE=60f;
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

    // Start is called before the first frame update
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
        }else if(draggedSelectionBlock!=null){
            selectionScrollRect.StopMovement();
            selectionScrollRect.enabled=false;
            currentlyDraggedObject=Instantiate(draggedSelectionBlock.GetComponent<SelectionBlock>().prefabBlock);
            blockProgrammerScript.addBlockObject(currentlyDraggedObject);
            currentlyDraggedObject.transform.SetParent(environment.transform);
            if(currentlyDraggedObject.GetComponent<Block>().getBlockType()==BlockType.Action){
                currentlyDraggedObject.GetComponent<Block>().initActionInputFieldHandler();
            }
            offset=(Vector2)data.position;
        }
    }

    public void OnDrag(PointerEventData data){
        if(currentlyDraggedObject!=null){
            BlockType currentBlockType=currentlyDraggedObject.GetComponent<Block>().getBlockType();
            Vector2 cursorPosition = (Vector2)data.position;
            if(currentlyDraggedObject.transform.parent.gameObject!=environment){
                int currentlyDraggedSiblingIndex=currentlyDraggedObject.transform.GetSiblingIndex();
                if(currentBlockType==BlockType.Action||currentBlockType==BlockType.Control){
                    Body parentBody=currentlyDraggedObject.GetComponentInParent<Body>();
                    parentBody.removeBlockSpaceByIndex(currentlyDraggedSiblingIndex);
                }else if(currentBlockType!=BlockType.Method){
                    Header parentHeader=currentlyDraggedObject.GetComponentInParent<Header>();
                    InputFieldHandler inputFieldHandler=parentHeader.getInputFieldHandlerForInputBlock(currentlyDraggedObject);
                    inputFieldHandler.clearInputBlock();
                    inputFieldHandler.setInputSpaceActive(true);
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
                }else{
                    removeGhostBlock();
                }
            }else if(currentBlockType!=BlockType.Method){
                nearestInputSpace=raycaster.getNearestSpaceToPosition<InputSpace>(blockProgrammerScript.getInputSpaces(),
                                                                                    data.position,
                                                                                    SNAPDISTANCE);
                InputFieldHandler inputFieldHandler=nearestInputSpace.getInputFieldHandler();
                if(inputFieldHandler!=null){
                    Debug.Log("Found");
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
                        // Debug.Log(nearestBlockSpace.getIndex());
                        GameObject nearestBlockSpaceBody=nearestBlockSpace.getParentBody();
                        nearestBlockSpaceBody.GetComponent<Body>().insertBlock(currentlyDraggedObject,nearestBlockSpace.getIndex());
                        nearestBlockSpaceBody.GetComponentInParent<Block>().updateSpacePositions();
                    }else{
                        currentlyDraggedObject.GetComponent<Block>().updateSpacePositions();
                    }
                    currentlyDraggedObject.GetComponent<Block>().setSpacesActive(true);
                }else if(currentBlockType!=BlockType.Method){
                    if(highlightActive){
                        nearestInputSpace.getInputFieldHandler().addInputBlock(currentlyDraggedObject);
                    }else{
                        currentlyDraggedObject.GetComponent<Block>().updateSpacePositions();
                    }
                    currentlyDraggedObject.GetComponent<Block>().setSpacesActive(true);
                }else{
                    currentlyDraggedObject.GetComponent<Block>().setSpacesActive(true);
                }   
            }
            nearestBlockSpace=null;
            nearestInputSpace=null;
            currentlyDraggedObject=null;
            removeGhostBlock();
            removeInputFieldHighlight();
        }
    }

    public void addGhostBlockToBlockAtSiblingIndex(GameObject blockObject,int siblingIndex){
        ghostBlockActive=true;
        ghostBlock.SetActive(true);
        ghostBlock.transform.SetParent(blockObject.transform);
        ghostBlock.transform.SetSiblingIndex(siblingIndex);
    }

    public void removeGhostBlock(){
        ghostBlockActive=false;
        ghostBlock.SetActive(false);
        ghostBlock.transform.SetParent(null);
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
