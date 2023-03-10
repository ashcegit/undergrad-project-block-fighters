using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Raycaster : MonoBehaviour
{

    BlockProgrammerScript blockProgrammerScript;
    public GraphicRaycaster[] raycasters;
    public EventSystem eventSystem;

    void Awake(){
        blockProgrammerScript=GameObject.FindGameObjectWithTag("BlockProgrammer").GetComponent<BlockProgrammerScript>();
    }

    public GameObject getObjectAtPosition(Vector2 position){
        PointerEventData pointerEventData=new PointerEventData(eventSystem);
        pointerEventData.position=position;

        List<RaycastResult> raycastResults=new List<RaycastResult>();
        int raycastersLength=raycasters.Length;
        for(int i=0;i<raycastersLength;i++){
            List<RaycastResult> tempResults=new List<RaycastResult>();
            raycasters[i].Raycast(pointerEventData,tempResults);
            raycastResults.AddRange(tempResults);
        }

        int resultsCount=raycastResults.Count;
        for(int i=0;i<resultsCount;i++){
            GameObject raycastedObject=raycastResults[i].gameObject;
            if(raycastedObject!=null){return raycastedObject;}
        }
        return null;
    }

    public GameObject getObjectOfTypeAtPosition<T>(Vector2 position){
        PointerEventData pointerEventData=new PointerEventData(eventSystem);
        pointerEventData.position=position;

        List<RaycastResult> raycastResults=new List<RaycastResult>();
        int raycastersLength=raycasters.Length;
        for(int i=0;i<raycastersLength;i++){
            List<RaycastResult> tempResults=new List<RaycastResult>();
            raycasters[i].Raycast(pointerEventData,tempResults);
            raycastResults.AddRange(tempResults);
        }

        int resultsCount=raycastResults.Count;
        for(int i=0;i<resultsCount;i++){
            GameObject raycastedObject=raycastResults[i].gameObject;
            if(raycastedObject!=null&&raycastedObject.GetComponentInParent<T>()!=null){
                while(raycastedObject.GetComponent<T>()==null){
                    raycastedObject=raycastedObject.transform.parent.gameObject;
                }
                return raycastedObject;
            }
        }
        return null;
    }

    public T getNearestSpaceToPosition<T>(List<T> spaces,
                                                    Vector2 position,
                                                    float maxDistance) where T: I_Space,new(){
        T nearestSpace=new T();
        float minDistance=Mathf.Infinity;
        int spaceCount=spaces.Count;
        foreach(T space in spaces){
            float distance=Vector2.Distance(position,space.getPosition());
            if(distance<=maxDistance&&distance<minDistance&&space.getActive()){
                nearestSpace=space;
                minDistance=distance;
            }
        }
        return nearestSpace;
    }
}
