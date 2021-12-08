using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour,IPointerDownHandler,IDragHandler,IPointerUpHandler
{
    PointerInSelectionArea pointerInSelectionArea;
    GameObject currentlyDraggedObject;
    Raycaster raycaster;
    GameObject ghostBlock;
    Vector2 pointerPosition;
    Vector2 lastPosition;
    GameObject environment;
    ScrollRect selectionScrollRect;
    float snapDistance;
    bool dragging;
    Vector2 offset;

    void Awake(){
        pointerInSelectionArea=GameObject.FindGameObjectWithTag("BlockSelection").GetComponent<PointerInSelectionArea>();
        selectionScrollRect=GameObject.FindGameObjectWithTag("BlockSelection").GetComponentInParent<ScrollRect>();
        raycaster=GetComponent<Raycaster>();
        ghostBlock=GameObject.FindGameObjectWithTag("GhostBlock");
        environment=GameObject.FindGameObjectWithTag("BlockEnvironment");
        snapDistance=30;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
            currentlyDraggedObject.transform.SetParent(environment.transform);
            offset=(Vector2)data.position;
        }
    }

    public void OnDrag(PointerEventData data){
        if(currentlyDraggedObject!=null){
            Debug.Log("Dragging");
            Vector2 cursorPosition = (Vector2)data.position;
            currentlyDraggedObject.transform.position = cursorPosition;

            //add ghostBlock stuff;
        }
    }

    public void OnPointerUp(PointerEventData data){
        ghostBlock.transform.SetParent(null);
        ghostBlock.SetActive(false);
        if(currentlyDraggedObject!=null){
            selectionScrollRect.enabled=true;
            if(pointerInSelectionArea.getPointerIn()){
                Destroy(currentlyDraggedObject);
            }
            currentlyDraggedObject=null;
        }
    }

    // public void addGhostBlockToBlock(GameObject blockObject){
    //     ghostBlock.gameObject.SetActive(true);
    //     ghostBlock.transform.SetParent(blockObject.transform);

    // }

    // public void removeGhostBlock(){
    //     ghostBlock.gameObject.SetActive(false);
    //     ghostBlock.transform.SetParent(GameObject.FindGameObjectWithTag("BlockEnvironment")
    //                                                 .transform.parent.parent.parent.transform);
    // }
}
