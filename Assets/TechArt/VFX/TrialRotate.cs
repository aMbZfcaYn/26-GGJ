using UnityEngine;

public class AlignWithVelocity : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("最小移动距离阈值，防止静止时乱转")]
    public float moveThreshold = 0.001f;

    [Tooltip("旋转平滑度 (数值越大越快，0为瞬间朝向)")]
    public float turnSpeed = 20f;

    private Vector3 _lastPosition;
    private Transform _parentTransform;

    void Start()
    {
        // 获取父物体
        _parentTransform = transform.parent;

        if (_parentTransform == null)
        {
            Debug.LogWarning("此物体没有父物体，脚本无法通过父物体位置计算速度！");
            enabled = false;
            return;
        }

        // 初始化位置
        _lastPosition = _parentTransform.position;
    }

    void LateUpdate()
    {
        if (_parentTransform == null) return;

        // 1. 获取当前父物体的位置
        Vector3 currentPos = _parentTransform.position;
        
        // 2. 计算位移向量 (这就是速度方向)
        Vector3 moveDirection = currentPos - _lastPosition;

        // 3. 只有当移动距离超过阈值时才旋转 (避免静止时抖动)
        if (moveDirection.sqrMagnitude > moveThreshold * moveThreshold)
        {
            // 计算目标旋转角度 (Z轴朝向移动方向)
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            if (turnSpeed <= 0)
            {
                // 瞬间朝向 (适合大多数子弹、刀光)
                transform.rotation = targetRotation;
            }
            else
            {
                // 平滑旋转 (适合飞船、鸟类拖尾)
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            }
        }

        // 4. 更新上一帧位置
        _lastPosition = currentPos;
    }
}
