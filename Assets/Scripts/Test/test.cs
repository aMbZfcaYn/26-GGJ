
using InputNamespace;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.DefaultAttackWasPressed)
        {
            Debug.Log("click");
        }
    }
}
