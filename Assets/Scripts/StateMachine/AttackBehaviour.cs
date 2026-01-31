using UnityEngine;

namespace StateMachine
{
    public class AttackBehaviour : StateMachineBehaviour
    {
        public GameObject attackEntityPrefab;

        public float forwardOffset = 1.0f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Transform attackerTransform = animator.gameObject.transform;
            Vector2 spawnPosition = attackerTransform.position + attackerTransform.forward * forwardOffset;
            Instantiate(attackEntityPrefab, spawnPosition, animator.gameObject.transform.rotation);
        }
    }
}