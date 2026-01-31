using DG.Tweening;
using Management;
using Management.Tag;
using UnityEngine;
using UnityEngine.Rendering;

namespace Possession
{
    public class PossessionProcess : MonoBehaviour
    {
        [Header("配置")] public Camera mainCamera;
        public SpriteRenderer darkOverlay;

        [Header("动画参数")] public float focusDuration = 0.5f; // 移向玩家耗时
        public float pauseDuration = 0.5f; // 停顿耗时
        public float transferDuration = 0.8f; // 移向敌人耗时

        [Header("镜头缩放")] public float closeUpSize = 3.0f; // 特写时的镜头大小 (越小放得越大)

        [Header("高亮设置")]
        // 设置一个非常大的排序值，确保盖过黑色遮罩(遮罩假设是100)
        public int highlightSortingOrder = 200;

        public float overlayAlpha = 0.85f; // 背景变黑的程度 (0-1)

        private float originalSize;
        private float originalZ;
        private int originalPlayerOrder;
        private int originalEnemyOrder;
        private SortingGroup playerSG;
        private SortingGroup enemySG;
        private SpriteRenderer playerSR;
        private SpriteRenderer enemySR;

        private CameraControl camScript;

        private void Start()
        {
            GameEventManager.Instance.onPossessionTrigger.AddListener(StartPossessionSequence);
            if (!mainCamera) mainCamera = Camera.main;

            camScript = mainCamera.GetComponent<CameraControl>();
        }

        private void OnDestroy()
        {
            GameEventManager.Instance.onPossessionTrigger.RemoveListener(StartPossessionSequence);
        }

        private void StartPossessionSequence(GameObject enemy, GameObject player)
        {
            TimeManager.Instance.SetTimeScale(0);
            camScript.enabled = false;
            GameEventManager.Instance.onEnemyKilled.Invoke(enemy);

            // 2. 记录原始数据 并 提升渲染层级
            originalSize = mainCamera.orthographicSize;
            originalZ = mainCamera.transform.position.z;

            // --- 处理层级提升 (核心逻辑) ---
            // 优先查找 SortingGroup (处理多部件角色)
            playerSG = player.GetComponent<SortingGroup>();
            enemySG = enemy.GetComponent<SortingGroup>();

            if (playerSG)
            {
                originalPlayerOrder = playerSG.sortingOrder;
                playerSG.sortingOrder = highlightSortingOrder;
            }
            else // 如果没有 SortingGroup，就找 SpriteRenderer
            {
                playerSR = player.GetComponent<SpriteRenderer>();
                if (playerSR)
                {
                    originalPlayerOrder = playerSR.sortingOrder;
                    playerSR.sortingOrder = highlightSortingOrder;
                }
            }

            if (enemySG)
            {
                originalEnemyOrder = enemySG.sortingOrder;
                enemySG.sortingOrder = highlightSortingOrder;
            }
            else
            {
                enemySR = enemy.GetComponent<SpriteRenderer>();
                if (enemySR)
                {
                    originalEnemyOrder = enemySR.sortingOrder;
                    enemySR.sortingOrder = highlightSortingOrder;
                }
            }

            // 3. 将黑色背景图移到摄像机前并淡入
            if (darkOverlay)
            {
                // 确保遮罩在摄像机前面，但在角色后面
                // 比如：角色200，遮罩100，地图0
                darkOverlay.transform.position =
                    new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
                // 让遮罩跟随摄像机(或者直接作为摄像机子物体)
                darkOverlay.transform.SetParent(mainCamera.transform);
                darkOverlay.transform.localPosition = new Vector3(0, 0, 10); // 放在摄像机前方

                darkOverlay.DOFade(overlayAlpha, 0.3f).SetUpdate(true);
            }

            // 4. DOTween 序列 (和之前类似，去掉了Layer部分)
            Sequence seq = DOTween.Sequence().SetUpdate(true);

            // A. 移向玩家
            Vector3 playerTarget = new Vector3(player.transform.position.x, player.transform.position.y, originalZ);
            seq.Append(mainCamera.transform.DOMove(playerTarget, focusDuration).SetEase(Ease.OutQuart));
            seq.Join(mainCamera.DOOrthoSize(closeUpSize, focusDuration).SetEase(Ease.OutQuart));

            // B. 停顿
            seq.AppendInterval(pauseDuration);

            // C. 移向敌人
            Vector3 enemyTarget = new Vector3(enemy.transform.position.x, enemy.transform.position.y, originalZ);
            seq.Append(mainCamera.transform.DOMove(enemyTarget, transferDuration).SetEase(Ease.InOutQuad));

            // D. 结束
            seq.OnComplete(() =>
            {
                // 恢复遮罩
                if (darkOverlay)
                {
                    darkOverlay.DOFade(0, 0.5f).SetUpdate(true).OnComplete(() =>
                    {
                        darkOverlay.transform.SetParent(null); // 解除父子关系
                    });
                }

                // 恢复镜头
                mainCamera.DOOrthoSize(originalSize, 0.5f).SetUpdate(true);

                // 恢复时间
                TimeManager.Instance.SetTimeScale(1);
                camScript.enabled = true;

                // 逻辑交接
                var enemyFsm = enemy.GetComponent<EnemyFSM>();
                enemyFsm.TransitionState(new Dead(enemyFsm));
                
                PerformPossessionLogic(player, enemy);
                GameEventManager.Instance.onPossessionEnd.Invoke();
            });
        }

        private void PerformPossessionLogic(GameObject oldPlayer, GameObject newBody)
        {
            var oldPlayerControl = oldPlayer.GetComponent<PlayerControl>();
            var playerControl = newBody.AddComponent<PlayerControl>();
            var ability = newBody.AddComponent<AbilityManager>();

            playerControl.Init();
            playerControl.PlayerBodyAC = oldPlayerControl.PlayerBodyAC;
            playerControl.PlayerLegAC = oldPlayerControl.PlayerLegAC;
            playerControl.leg = newBody.transform.Find("Leg").gameObject;
            playerControl.
            
            ability.ApplySelectAbility();

            Destroy(oldPlayer);
        }

        void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (!obj) return;
            obj.layer = newLayer;
            foreach (Transform child in obj.transform)
            {
                if (!child) continue;
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}