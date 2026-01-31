using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParameter", menuName = "Scriptable Objects/EnemyParameter")]
public class EnemyParameter : ScriptableObject
{
    public float moveSpeed;

    public int atkRange;

    public Animator animator;
}
