using UnityEngine;

public static class Utils
{
    public static Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        LayerMask layerMask = (1 << Layers.Ground);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                return hit.point;
        return Vector3.zero;
    }


    public static Vector3 GetRandomPositionInSquare(float left, float right, float down, float up)
    {
        float xPos = left + Random.value * (right - left);
        float zPos = down + Random.value * (up - down);
        return new Vector3(xPos, zPos);
    }
}