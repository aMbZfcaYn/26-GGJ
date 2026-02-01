using System;
using UnityEngine;
using Utilities;

public class TimeManager : GenericSingleton<TimeManager>
{
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = scale * 0.02f;
    }
}