//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class Clock {

//    private DateTime time;

//    public event Action OnSecondsChanged;
//    public event Action OnMinutesChanged;
//    public event Action OnHoursChanged;

//    public bool IsPaused;

//    public void Update(float deltaTime)
//    {
//        if (IsPaused) return;

//        time.AddSeconds(deltaTime);

//    }

//    public void Unpause()
//    {
//        IsPaused = false;
//    }

//    public void Pause()
//    {
//        IsPaused = true;
//    }
//}
