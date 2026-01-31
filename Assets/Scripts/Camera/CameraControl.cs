using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("CameraSetting")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

    [Header("MoveSetting")]
    [SerializeField] private float specialMoveSpeed = 2.0f;
    [SerializeField] private float maxDistanceFromTarget = 5.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float backSpeed = 2.0f;

    private Vector3 defaultPosition;
    void LateUpdate()
    {
        if (target == null) return;

        defaultPosition = target.position + offset;

        if (InputManager.CameraMoveIsheld)
        {
            HandleSpecialMovement();
        }
        else
        {
            transform.position = defaultPosition;
        }
    }

    private void HandleSpecialMovement()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();

        Vector3 directionToMouse = mouseWorldPos - target.position;
        directionToMouse.z = 0;

        if (directionToMouse.magnitude > maxDistanceFromTarget)
        {
            directionToMouse = directionToMouse.normalized * maxDistanceFromTarget;
        }
        Vector3 specialPosition = target.position + directionToMouse + offset;

        transform.position = Vector3.Lerp(transform.position, specialPosition,
                                        specialMoveSpeed * Time.deltaTime);

        if (InputManager.CameraMoveIsReleased)
        {
            transform.position = Vector3.Lerp(transform.position, defaultPosition, backSpeed);

        }
    }

    private Vector3 GetMouseWorldPosition()
    {

        Vector3 mouseScreenPos = Input.mousePosition;

        mouseScreenPos.z = Camera.main.transform.position.y - target.position.y;

        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}
