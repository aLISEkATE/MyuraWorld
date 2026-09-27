using UnityEngine;
using TMPro;
using System;
public class TimeUIScript : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dayText;


    private void OnEnable()
    {
        TimeManager.onMinuteChanged  += UpdateTime;
        TimeManager.onHourChanged  += UpdateTime;
        TimeManager.onDayChanged  += UpdateTime;

    }

    private void OnDisable()
    {
        TimeManager.onMinuteChanged  -= UpdateTime;
        TimeManager.onHourChanged  -= UpdateTime;
        TimeManager.onDayChanged  -= UpdateTime;
    }

    private void UpdateTime()
    {
        timeText.text = $"{TimeManager.Hour:00}:{TimeManager.Minute:00}";
        dayText.text = $"Day {TimeManager.Day}";
    }
}
