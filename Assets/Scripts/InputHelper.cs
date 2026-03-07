using UnityEngine;

public static class InputHelper
{
    public static bool WasTap(out float screenX)
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            screenX = Input.GetTouch(0).position.x;
            return true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            screenX = Input.mousePosition.x;
            return true;
        }

        screenX = 0f;
        return false;
    }

    public static bool WasTap() => WasTap(out _);
}
