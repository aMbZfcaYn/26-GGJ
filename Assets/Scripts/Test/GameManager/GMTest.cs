using Management;
using UnityEngine;

public class GMTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEventManager.Instance.onLevelClear.AddListener(EveryEnemyDies);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EveryEnemyDies()
    {
        Debug.Log("We are Done.");
    }
}