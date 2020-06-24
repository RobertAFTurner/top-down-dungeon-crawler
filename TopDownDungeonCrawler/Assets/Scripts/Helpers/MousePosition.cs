using UnityEngine;

public class MousePosition
{
    public static Vector3 GetMouseWorldPosition(Vector3 screenPos, Camera camera)
    {
        var result = camera.ScreenToWorldPoint(screenPos);
        result.z = 0f;
        return result;
    }
}