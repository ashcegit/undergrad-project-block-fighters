using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BE2_Raycaster : MonoBehaviour, I_BE2_Raycaster
{
    BE2_DragDropManager _dragDropManager;
    PointerEventData m_PointerEventData;

    public GraphicRaycaster[] m_Raycasters;
    public EventSystem m_EventSystem;

    void Awake()
    {
        _dragDropManager = GetComponent<BE2_DragDropManager>();
    }

    //void Start()
    //{
    //
    //}

    //void Update()
    //{
    //    
    //}

    public I_BE2_Drag GetDragAtPosition(Vector2 position)
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = position;

        List<RaycastResult> globalResults = new List<RaycastResult>();
        int rayCount = m_Raycasters.Length;
        for (int i = 0; i < rayCount; i++)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycasters[i].Raycast(m_PointerEventData, results);
            //EventSystem.current.RaycastAll(m_PointerEventData, results);
            globalResults.AddRange(results);
        }

        int resultCount = globalResults.Count;
        for (int i = 0; i < resultCount; i++)
        {
            GameObject resultGameObject = globalResults[i].gameObject;

            I_BE2_Drag drag = resultGameObject.GetComponentInParent<I_BE2_Drag>();
            if (drag != null)
            {
                return drag;
            }
        }

        return null;
    }

    public I_BE2_Spot GetSpotAtPosition(Vector2 position)
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = position;

        List<RaycastResult> globalResults = new List<RaycastResult>();
        int rayCount = m_Raycasters.Length;
        for (int i = 0; i < rayCount; i++)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycasters[i].Raycast(m_PointerEventData, results);
            globalResults.AddRange(results);
        }

        int resultCount = globalResults.Count;
        for (int i = 0; i < resultCount; i++)
        {
            RaycastResult result = globalResults[i];
            if (result.gameObject.activeSelf)
            {
                I_BE2_Spot spot = result.gameObject.GetComponent<I_BE2_Spot>();
                if (spot != null)
                {
                    return spot;
                }
            }
        }

        return null;
    }

    public I_BE2_Spot FindClosestSpotOfType<T>(I_BE2_Drag drag, float maxDistance)
    {
        float minDistance = Mathf.Infinity;
        I_BE2_Spot foundSpot = null;
        int spotsCount = _dragDropManager.SpotsList.Count;
        for (int i = 0; i < spotsCount; i++)
        {
            I_BE2_Spot spot = _dragDropManager.SpotsList[i];
            if (spot is T && spot.Transform.gameObject.activeSelf)
            {
                I_BE2_Drag d = spot.Transform.GetComponentInParent<I_BE2_Drag>();

                if (d != drag && !drag.ChildBlocks.Contains(d.Block))
                {
                    float distance = Vector2.Distance(drag.RayPoint, spot.DropPosition);
                    if (distance < minDistance && distance <= maxDistance)
                    {
                        foundSpot = spot;
                        minDistance = distance;
                    }
                }
            }
        }

        return foundSpot;
    }

    public I_BE2_Spot FindClosestSpotForBlock(I_BE2_Drag drag, float maxDistance)
    {
        float minDistance = Mathf.Infinity;
        I_BE2_Spot foundSpot = null;
        int spotsCount = _dragDropManager.SpotsList.Count;
        for (int i = 0; i < spotsCount; i++)
        {
            I_BE2_Spot spot = _dragDropManager.SpotsList[i];
            if ((spot is BE2_SpotBlockBody || (spot is BE2_SpotOuterArea && spot.Block.ParentSection != null)) && spot.Transform.gameObject.activeSelf)
            {
                I_BE2_Drag d = spot.Transform.GetComponentInParent<I_BE2_Drag>();
                if (d != drag)
                {
                    float distance = Vector2.Distance(drag.RayPoint, spot.DropPosition);
                    if (distance < minDistance && distance <= maxDistance)
                    {
                        foundSpot = spot;
                        minDistance = distance;
                    }
                }
            }
        }

        return foundSpot;
    }
}
