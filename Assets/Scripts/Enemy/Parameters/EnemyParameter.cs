using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParameter", menuName = "Scriptable Objects/EnemyParameter")]
public class EnemyParameter : ScriptableObject
{
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float huntSpeed;

    [SerializeField] private int atkRange;

    [SerializeField] private float stunDuration;

    public float PatrolSpeed => patrolSpeed;
    public float HuntSpeed => huntSpeed;
    public int AtkRange => atkRange;
    public float StunDuration => stunDuration;

}
