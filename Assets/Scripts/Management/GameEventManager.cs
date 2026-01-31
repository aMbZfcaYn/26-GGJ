using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }
    
    public PossessionEvent onPossessionTrigger = new  PossessionEvent();


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}

[System.Serializable]
public class PossessionEvent : UnityEvent<GameObject, GameObject> {} // Target, Player
