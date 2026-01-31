using System;
using UnityEngine;

public class PrefabLibrary : MonoBehaviour
{
    public static PrefabLibrary Instance { get; private set; }

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

    public GameObject FlyMaskPrefab;
}
