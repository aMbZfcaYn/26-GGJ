using Management.Tag;
using UnityEngine;

public class RotateAroundPoint : MonoBehaviour
{
    [Header("旋转设置")] [SerializeField] private Transform rotationTarget;
    [SerializeField] private float rotationSpeed = 90f;

    [Header("旋转方向")] [SerializeField] private bool useCollisionDirection = true;
    [SerializeField] private bool defaultClockwise = true;

    [Header("旋转限制")] [SerializeField] private float maxRotationAngle = 180f;
    private bool hasStartedRotating = false;
    private bool hasReachedMax = false;
    private bool rotateClockwise = true;

    private float currentRotationAngle = 0f;

    private int rotator;


    [Header("碰撞检测")] [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private float detectionRadius = 0.5f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Taggable>().HasTag(TagUtils.Type_Enemy))
        {
            rotator = 1;
        }
        else if (other.gameObject.GetComponent<Taggable>().HasTag(TagUtils.Type_Player))
        {
            rotator = 0;
        }

        if ((other.gameObject.GetComponent<Taggable>().HasTag(TagUtils.Type_Player) ||
             other.gameObject.GetComponent<Taggable>().HasTag(TagUtils.Type_Enemy)) &&
            !hasStartedRotating && !hasReachedMax)
        {
            if (useCollisionDirection)
            {
                Vector2 playerPosition = other.transform.position;
                Vector2 doorPosition = transform.position;
                Vector2 rotationCenterPos = rotationTarget.position;

                Vector2 playerToCenter = rotationCenterPos - (Vector2)playerPosition;
                Vector2 doorToCenter = rotationCenterPos - doorPosition;

                float crossProduct = playerToCenter.x * doorToCenter.y - playerToCenter.y * doorToCenter.x;

                rotateClockwise = crossProduct > 0;
            }
            else
            {
                rotateClockwise = defaultClockwise;
            }

            hasStartedRotating = true;
        }
    }

    void Update()
    {
        if (hasStartedRotating && !hasReachedMax && rotationTarget)
        {
            if (rotator == 0)
            {
                CheckForEnemiesDuringRotation();
            }

            RotateDoor();
        }
    }

    private void RotateDoor()
    {
        Vector3 rotationCenter = rotationTarget.position;
        float angle = rotationSpeed * Time.deltaTime;

        if (!rotateClockwise)
        {
            angle = -angle;
        }

        float absCurrentAngle = Mathf.Abs(currentRotationAngle);
        float absNextAngle = Mathf.Abs(currentRotationAngle + angle);

        if (absNextAngle > maxRotationAngle)
        {
            float adjustedAngle = maxRotationAngle - absCurrentAngle;
            if (!rotateClockwise)
            {
                adjustedAngle = -adjustedAngle;
            }

            transform.RotateAround(rotationCenter, Vector3.forward, adjustedAngle);
            currentRotationAngle += adjustedAngle;
            hasReachedMax = true;
        }
        else
        {
            transform.RotateAround(rotationCenter, Vector3.forward, angle);
            currentRotationAngle += angle;
        }
    }

    // 检测旋转路径上是否有敌人
    private void CheckForEnemiesDuringRotation()
    {
        // 获取门的边界信息
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayerMask);

        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Name" + collider.gameObject.name);
            if (collider.gameObject.GetComponent<Taggable>().HasTag(TagUtils.Type_Enemy) &&
                collider.gameObject != gameObject)
            {
                Debug.Log("Door hit an enemy!");
            }
        }

        Vector3 rotationCenter = rotationTarget.position;
        float testAngle = rotationSpeed * Time.deltaTime;
        if (!rotateClockwise) testAngle = -testAngle;

        Vector3 predictedPosition = RotatePointAroundPivot(transform.position, rotationCenter, testAngle);

        Collider2D[] predictedColliders =
            Physics2D.OverlapCircleAll(predictedPosition, detectionRadius, enemyLayerMask);

        foreach (Collider2D collider in predictedColliders)
        {
            if (collider.gameObject.GetComponent<Taggable>().HasTag(TagUtils.Type_Enemy) &&
                collider.gameObject != gameObject)
            {
                Debug.Log("Door hit an enemy!");
            }
        }
    }


    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle)
    {
        Vector3 direction = point - pivot;
        direction = Quaternion.Euler(0, 0, angle) * direction;
        return pivot + direction;
    }

    public void ResetRotation()
    {
        currentRotationAngle = 0f;
        hasStartedRotating = false;
        hasReachedMax = false;
    }

    private bool hasReachedMaxAngle()
    {
        return hasReachedMax || Mathf.Abs(currentRotationAngle) >= maxRotationAngle;
    }

    // 在Scene视图中可视化检测范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (rotationTarget)
        {
            Gizmos.DrawLine(transform.position, rotationTarget.position);
        }
    }
}