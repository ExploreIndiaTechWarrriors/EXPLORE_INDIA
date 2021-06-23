using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInputScript : MonoBehaviour
{
    public GameObject goAR;                   
    public GameObject BackBtn;

    public bool isPlacedOverUI;

    private void OnMouseDown()
    {
        if(!IsPointOverUIObject(Input.mousePosition))
        {
            Debug.Log("onmouseDown entered");
            goAR.SetActive(true);
            BackBtn.SetActive(true);
        }
    }

    bool IsPointOverUIObject(Vector2 pos)
    {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(pos.x, pos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
        
    }

}
