using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    Vector3 moveVector;

    void Update()
    {
        Vector2 mouseViewportPos = UnitSelection.Instance.cam.ScreenToViewportPoint(Input.mousePosition);
        moveVector = Vector3.zero;
        if (mouseViewportPos.x <= 0f || mouseViewportPos.x >= 1f)
        {
            moveVector.x = GetLengthByPosition(mouseViewportPos.x);
        }

        if (mouseViewportPos.y <= 0f || mouseViewportPos.y >= 1f)
        {
            moveVector.z = GetLengthByPosition(mouseViewportPos.y);
        }
    }

    void LateUpdate()
    {
        if (moveVector == Vector3.zero)
            return;

        transform.position += moveVector * speed;
    }

    float GetLengthByPosition(float axisPos)
    {
        return (axisPos <= 0f) ? -1f : 1f;
    }
}
