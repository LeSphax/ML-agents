using UnityEngine;

//source https://forum.unity.com/threads/screen-shake-effect.22886/

public class ScreenShake : MonoBehaviour
{

    private float ShakeY = 0.0f;
    private float ShakeYSpeed = 0.8f;

    public void setShake(float someY)
    {
        transform.position = transform.position.z * Vector3.forward;
        ShakeY = Mathf.Abs(ShakeY) + someY;
    }

    void Update()
    {
        Vector2 _newPosition = new Vector2(0, ShakeY);
        if (ShakeY < 0)
        {
            ShakeY *= ShakeYSpeed;
        }
        ShakeY = -ShakeY;
        transform.Translate(_newPosition, Space.World);
    }

}
