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
    GameObject ghostBlock;
    Vector2 offset;
    GameObject environment;
    ScrollRect selectionScrollRect;
    const float SNAPDISTANCE=50f;
    bool dragging;
    bool ghostBlockActive;

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
        GameObject draggedExisingSelectionBlock=raycaster.getObjectOfTypeAtPosition<SelectionBlock>((Vector2)data.position);
        if(draggedExistingBlock!=null){
            currentlyDraggedObject=draggedExistingBlock;
            offset=(Vector2)data.position;
        }else if(draggedExisingSelectionBlock!=null){
            selectionScrollRect.StopMovement();
            selectionScrollRect.enabled=false;
            currentlyDraggedObject=Instantiate(draggedExisingSelectionBlock.GetComponent<SelectionBlock>().prefabBlock);
            blockProgrammerScript.addBlockObject(currentlyDraggedObject);
            currentlyDraggedObject.transform.SetParent(environment.transform);
            offset=(Vector2)data.position;
        }
    }

    public void OnDrag(PointerEventData data){
        if(currentlyDraggedObject!=null){
            Vector2 cursorPosition = (Vector2)data.position;
            if(currentlyDraggedObject.transform.parent.gameObject!=environment){
                int currentlyDraggedSiblingIndex=currentlyDraggedObject.transform.GetSiblingIndex();
                Body parentBody=currentlyDraggedObject.GetComponentInParent<Body>();
                parentBody.removeBlockSpaceByIndex(currentlyDraggedSiblingIndex);
            }
            currentlyDraggedObject.transform.SetParent(environment.transform);
            currentlyDraggedObject.GetComponent<Block>().setPosition(cursorPosition);
            currentlyDraggedObject.GetComponent<Block>().setBlockSpacesActive(false);

            if(currentlyDraggedObject.GetComponent<Block>().getBlockType()!=BlockType.Method){
                nearestBlockSpace=raycaster.getNearestBlockSpaceToPosition(data.position,SNAPDISTANCE);
                if(nearestBlockSpace.getParentBody()!=null){
                    addGhostBlockToBlockAtSiblingIndex(nearestBlockSpace.getParentBody(),nearestBlockSpace.getIndex());
                }else{
                    removeGhostBlock();
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
                if(ghostBlockActive){
                    // Debug.Log(nearestBlockSpace.getIndex());
                    GameObject nearestBlockSpaceBody=nearestBlockSpace.getParentBody();
                    nearestBlockSpaceBody.GetComponent<Body>().insertBlock(currentlyDraggedObject,nearestBlockSpace.getIndex());
                    nearestBlockSpaceBody.GetComponentInParent<Block>().updateBlockSpacePositions();
                }else{
                    currentlyDraggedObject.GetComponent<Block>().updateBlockSpacePositions();
                }
                currentlyDraggedObject.GetComponent<Block>().setBlockSpacesActive(true);
            }
            nearestBlockSpace=null;
            currentlyDraggedObject=null;
            removeGhostBlock();
        }
    }

    public void addGhostBlockToBlockAtSiblingIndex(GameObject blockObject,int siblingIndex){
        ghostBlockActive=true;
        ghostBlock.SetActive(true);
        ghostBlock.transform.SetParent(blockObject.transform);
        ghostBlock.transform.SetSiblingIndex(siblingIndex);
        // Debug.Log((Vector2)blockObject.transform.position);
        // Debug.Log((Vector2)ghostBlock.transform.position);

    }

    public void removeGhostBlock(){
        ghostBlockActive=false;
        ghostBlock.SetActive(false);
        ghostBlock.transform.SetParent(null);
    }
}
