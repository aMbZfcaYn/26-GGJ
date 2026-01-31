using Management;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.RegisterEnemy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}