using UnityEngine;
using DG.Tweening;

public class PossessionProcess : MonoBehaviour
{
    [Header("配置")]
    public Camera mainCamera;
    public LayerMask focusLayer; // 在Inspector中选择新建的 "PossessionFocus" 层
    
    [Header("动画参数")]
    public float focusDuration = 0.5f;    // 移向玩家耗时
    public float pauseDuration = 0.5f;    // 停顿耗时
    public float transferDuration = 0.8f; // 移向敌人耗时
    
    [Header("镜头缩放")]
    public float closeUpSize = 3.0f; // 特写时的镜头大小 (越小放得越大)
    
    private float originalSize; // 记录原始镜头大小
    private float originalZ;    // 记录摄像机原本的Z轴深度
    private int defaultMask;
    private int focusLayerIndex;

    private void Start()
    {
        GameEventManager.Instance.onPossessionTrigger.AddListener(StartPossessionSequence);
        if (mainCamera == null) mainCamera = Camera.main;
        
        // 缓存Layer索引
        // 注意：focusLayer.value 是掩码值，我们需要转换成 Layer 的 int 索引 (0-31)
        focusLayerIndex = (int)Mathf.Log(focusLayer.value, 2); 
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.onPossessionTrigger.RemoveListener(StartPossessionSequence);
    }

    private void StartPossessionSequence(GameObject enemy, GameObject player)
    {
        // 1. 暂停游戏
        Time.timeScale = 0;

        // 2. 记录当前状态
        originalSize = mainCamera.orthographicSize;
        originalZ = mainCamera.transform.position.z; // 这一点非常重要，保持Z轴不变！
        defaultMask = mainCamera.cullingMask;
        int originalEnemyLayer = enemy.layer;

        // 3. 隔离渲染 (Culling Mask)
        SetLayerRecursively(player, focusLayerIndex);
        SetLayerRecursively(enemy, focusLayerIndex);
        mainCamera.cullingMask = focusLayer;

        // 4. 构建 DOTween 序列
        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true); // 忽略 TimeScale

        // --- 步骤 A: 聚焦玩家 ---
        // 目标位置：玩家的X,Y + 摄像机原本的Z
        Vector3 playerTargetPos = new Vector3(player.transform.position.x, player.transform.position.y, originalZ);
        
        // 移动摄像机
        seq.Append(mainCamera.transform.DOMove(playerTargetPos, focusDuration).SetEase(Ease.OutQuart));
        // 同时缩小镜头尺寸（放大画面）
        seq.Join(mainCamera.DOOrthoSize(closeUpSize, focusDuration).SetEase(Ease.OutQuart));

        // --- 步骤 B: 停顿 ---
        seq.AppendInterval(pauseDuration);

        // --- 步骤 C: 移向敌人 ---
        Vector3 enemyTargetPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y, originalZ);
        
        // 移动到敌人
        seq.Append(mainCamera.transform.DOMove(enemyTargetPos, transferDuration).SetEase(Ease.InOutQuad));
        // 保持特写大小，或者你可以再次调整大小
        // seq.Join(mainCamera.DOOrthoSize(closeUpSize, transferDuration)); 

        // --- 步骤 D: 完成回调 ---
        seq.OnComplete(() => 
        {
            // 恢复敌人Layer
            SetLayerRecursively(enemy, originalEnemyLayer);
            
            // 恢复摄像机
            mainCamera.cullingMask = defaultMask;
            
            // 恢复镜头大小 (可选：如果你希望附身后视角变回正常)
            mainCamera.DOOrthoSize(originalSize, 0.5f).SetUpdate(true);
            
            // 恢复时间
            Time.timeScale = 1;
            
            // 执行逻辑
            PerformPossessionLogic(player, enemy);
        });
    }

    private void PerformPossessionLogic(GameObject oldPlayer, GameObject newBody)
    {
        Debug.Log("2D 附身完成");
        Destroy(oldPlayer);
        newBody.tag = "Player";
        
        // 如果你有类似于 Cinemachine 或者 CameraFollow 的脚本
        // 记得在这里更新它的 Target，否则摄像机会停在原地
        // var camScript = mainCamera.GetComponent<CameraFollow>();
        // if(camScript) camScript.target = newBody.transform;
    }
    
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}