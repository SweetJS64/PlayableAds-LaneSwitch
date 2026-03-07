using UnityEngine;

public static class InputHelper
{
    private static bool _touchWasUsed;

    public static bool WasTap(out float screenX)
    {
        if (Input.touchCount > 0)
        {
            _touchWasUsed = true;
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                screenX = touch.position.x;
                return true;
            }
            screenX = 0f;
            return false;
        }

        if (!_touchWasUsed && Input.GetMouseButtonDown(0))
        {
            screenX = Input.mousePosition.x;
            return true;
        }

        screenX = 0f;
        return false;
    }

    public static bool WasTap() => WasTap(out _);
}
