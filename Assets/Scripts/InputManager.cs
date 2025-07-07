using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
    public Camera mainCamera;
    public LayerMask mask;
    public UiCard selectedUiCard;

    // Update is called once per frame
    void Update()
    {
        TouchInput();
    }

    private void TouchInput()
    {
        if (Input.touchCount != 1 || GameManager.Instance.state != GameManager.GameState.Play) return;
        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            selectedUiCard.OnBeginDrag();
        }


        if (touch.phase is TouchPhase.Stationary or TouchPhase.Moved)
        {
            selectedUiCard.OnDrag(touch);
        }

        if (touch.phase == TouchPhase.Ended)
        {
            selectedUiCard.OnEndDrag();
        }
    }
}