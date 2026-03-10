using UnityEngine;

public static class InputHelper
{
    private static bool _touchWasUsed;
    private static int _consumedFrame = -1;
    private static float _consumedScreenX;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        _touchWasUsed = false;
        _consumedFrame = -1;
    }

    public static bool WasTap(out float screenX)
    {
        if (_consumedFrame == Time.frameCount)
        {
            screenX = _consumedScreenX;
            return true;
        }

        if (Input.touchCount > 0)
        {
            _touchWasUsed = true;
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                screenX = touch.position.x;
                Consume(screenX);
                return true;
            }
            screenX = 0f;
            return false;
        }

        if (!_touchWasUsed && Input.GetMouseButtonDown(0))
        {
            screenX = Input.mousePosition.x;
            Consume(screenX);
            return true;
        }

        screenX = 0f;
        return false;
    }

    private static void Consume(float screenX)
    {
        _consumedFrame = Time.frameCount;
        _consumedScreenX = screenX;
    }

    public static bool WasTap() => WasTap(out _);
}
