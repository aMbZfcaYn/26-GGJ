using UnityEngine;
using Management.Tag;
using System.Runtime.CompilerServices;
public class Melee : MonoBehaviour
{
    public bool isblunk = false;
    public void changeblunk(bool blunk)
    {
        isblunk = blunk;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("创建攻击碰撞体" + other.name);
        GameObject enemyObject = other.gameObject;
        Taggable taggable = enemyObject.GetComponent<Taggable>();
        if (taggable.HasTag(TagManager.GetTag("Enemy")))
        {
            if (isblunk)
            {
                Debug.Log("钝器击中敌人: " + other.name);
            }
            else
            {
                Debug.Log("锐器击中敌人: " + other.name);
            }


        }
    }
}
