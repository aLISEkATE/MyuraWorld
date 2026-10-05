using System;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{   [SerializeField] GameObject GameController;
    public static Action onMinuteChanged;
    public static Action onHourChanged;
    public static Action onDayChanged;
    public static Action TimePassed;
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int Day { get; private set; }

    public float MinuteToRealTime = 0.1f;
    private float timer;
    void Start()
    {
      Minute = 0;
      Hour = 6;
      Day = 1;

      timer = MinuteToRealTime;  
    }
    public void SetTime(int day, int hour, int minute)
    {
        Day = day;
        Hour = hour;
        Minute = minute;
    }
        // Update is called once per frame
    void Update()
    {
        if (PauseController.IsGamePaused)
             return;
       
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            Minute++;
            onMinuteChanged?.Invoke();

            if (Minute >= 60)
            {
                Hour++;
                onHourChanged?.Invoke();
                Minute = 0;
            } 
            
            if(Hour >= 24)
            {
                Day++;
                onDayChanged?.Invoke();
                Hour = 0;
                Minute = 0;  
                GameController.GetComponent<SaveController>().SaveGame();
            }
            timer = MinuteToRealTime;
        }
    }
}
