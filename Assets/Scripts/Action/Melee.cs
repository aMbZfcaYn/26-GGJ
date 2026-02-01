using System;
using Management;
using UnityEngine;
using Management.Tag;

public class Melee : MonoBehaviour
{
    public bool isblunk = false;
    private GameObject Owner;
    private Taggable ownertaggable;

    private Taggable _taggable;

    private void Awake()
    {
        _taggable = GetComponent<Taggable>();
        _taggable?.TryAddTag(TagUtils.Type_AttckEntity);
    }

    public void changeOwner(GameObject owner)
    {
        Owner = owner;
        ownertaggable = owner.GetComponent<Taggable>();
    }

    public void changeblunk(bool blunk)
    {
        isblunk = blunk;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("创建攻击碰撞体" + other.name);
        GameObject enemyObject = other.gameObject;
        Taggable taggable = enemyObject.GetComponent<Taggable>();
        if (ownertaggable.HasTag(TagUtils.Type_Player))
        {
            if (taggable.HasTag(TagUtils.Type_Enemy))
            {
                EnemyFSM otherFSM = other.GetComponent<EnemyFSM>();
                if (isblunk)
                {
                    Debug.Log("钝器击中敌人: " + other.name);
                    otherFSM.TransitionState(new Stun(otherFSM, transform));
                }
                else
                {
                    otherFSM.TransitionState(new Dead(otherFSM));
                    Debug.Log("锐器击中敌人: " + other.name);
                    GameEventManager.Instance.onEnemyKilled.Invoke(other.gameObject);
                    taggable.TryRemoveTag(TagUtils.Type_Enemy);
                }
            }
        }
        else if (ownertaggable.HasTag(TagUtils.Type_Enemy))
        {
            if (taggable.HasTag(TagUtils.Type_Player))
            {
                // PlayerFSM otherFSM = other.GetComponent<PlayerFSM>();
                Debug.Log("击中玩家: " + other.name);
                // otherFSM.TransitionState(new Stun(otherFSM, transform));
                GameManager.Instance.playerHp--;
                GameEventManager.Instance.OnPlayerHpChanged.Invoke();
                if (GameManager.Instance.playerHp > 0)
                {
                    GameEventManager.Instance.onPossessionTrigger.Invoke(Owner, other.gameObject);
                }
                else
                {
                    GameEventManager.Instance.onLevelFail.Invoke();
                }
            }
        }
    }
}