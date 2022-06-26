using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewTypes;
using UnityEngine.EventSystems;

public class InputSystem
{
    private int _fakeTouchId = 99;
    private TouchInfo _touchInfo;
    public TouchInfo TouchInfo {get => _touchInfo;}

    public InputSystem()
    {
        _touchInfo = new TouchInfo(Vector3.zero, Vector3.zero, false, TouchPhase.Canceled);
    }

    public TouchInfo ReadInput()
    {
        _touchInfo.Phase = TouchPhase.Canceled;
        //If on mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            TouchEvents(touch.fingerId, touch.position, touch.phase);
        }
        //else emulate with mouse button
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                TouchEvents(_fakeTouchId, Input.mousePosition, TouchPhase.Began);
            }
            else if (Input.GetMouseButton(0))
            {
                TouchEvents(_fakeTouchId, Input.mousePosition, TouchPhase.Moved);
            }
            if (Input.GetMouseButtonUp(0))
            {
                TouchEvents(_fakeTouchId, Input.mousePosition, TouchPhase.Ended);
            }
        }

        return _touchInfo;
    }

    private void TouchEvents(int touchId, Vector3 touchPos, TouchPhase touchPhase)
    {    
        _touchInfo.Phase = touchPhase;
        _touchInfo.IsInteractableUI = false;
        switch (touchPhase)
        {
            case TouchPhase.Began:
                _touchInfo.StartPos = touchPos;
                _touchInfo.StartPosWorld = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, Camera.main.farClipPlane)).normalized;

                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = touchPos;
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, raycastResults);
                foreach(RaycastResult raycastResult in raycastResults)
                {
                    if (raycastResult.gameObject.tag == "UI_Interactable")
                    {
                        _touchInfo.IsInteractableUI = true;
                    }
                } 
                break;
            case TouchPhase.Stationary:
            case TouchPhase.Moved:
                _touchInfo.Direction = touchPos - _touchInfo.StartPos;
                _touchInfo.DirectionWorld = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, Camera.main.farClipPlane)).normalized - _touchInfo.StartPosWorld;       
                break;
            case TouchPhase.Ended:
                
                break;
        }
    }
}
