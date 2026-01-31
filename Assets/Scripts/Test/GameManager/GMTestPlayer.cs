using Management;
using UnityEngine;

public class GMTestPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.RegisterPlayer(gameObject);
        GameEventManager.Instance.onEnemyKilled.Invoke(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}