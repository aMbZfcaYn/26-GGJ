using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParameter", menuName = "Scriptable Objects/EnemyParameter")]
public class EnemyParameter : ScriptableObject
{
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float huntSpeed;

    [SerializeField] private float atkRange;
    [SerializeField] private float atkAngle;

    [SerializeField] private float stunDuration;
    
    [SerializeField] private float viewAngle;

    public float PatrolSpeed => patrolSpeed;
    public float HuntSpeed => huntSpeed;
    public float AtkRange => atkRange;
    public float AtkAngle => atkAngle;
    public float StunDuration => stunDuration;

    public float ViewAngle => viewAngle;
}
