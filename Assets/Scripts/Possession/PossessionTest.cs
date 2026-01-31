using System;
using UnityEngine;

public class PossessionTest : MonoBehaviour
{
    public GameObject target;
    public GameObject player;

    private void Update()
    {
        if(InputManager.SkillWasPressed)
        {
            StartPossessionTest();
        }
    }

    public void StartPossessionTest()
    {
        GameEventManager.Instance.onPossessionTrigger.Invoke(target, player);
    }
}
