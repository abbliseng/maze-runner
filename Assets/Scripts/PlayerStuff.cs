using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerStuff : MonoBehaviour
{
    public TMP_Text displayText;
    public bool runTimer = true;
    
    private float startTime;

    private float timeOffset = 0f;// 45f + 59*60 + 23*60*60;

    private void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (runTimer)
        {
            // Debug.Log(Timer());
            displayText.text = Timer();
        }
    }

    public void ResetTimer()
    {
        runTimer = true;
        startTime = Time.time;
    }

    public string Timer()
    {
        string timerString = "";

        float totalSeconds = Mathf.Floor(Time.time - startTime) + timeOffset;
        float seconds = totalSeconds % 60;
        float totalMinutes = totalSeconds / 60f;
        float minutes = Mathf.Floor(totalMinutes % 60);
        float totalHours = totalMinutes / 60f;
        float hours = Mathf.Floor(totalHours % 24f);
        float totalDays = totalHours / 24f;
        float days = Mathf.Floor(totalDays);

        string secondsString = seconds < 10 ? "0" + seconds : seconds.ToString();
        string minutesString = minutes < 10 ? "0" + minutes : minutes.ToString();
        string hoursString = hours < 10 ? "0" + hours : hours.ToString();
        string daysString = days < 10 ? "0" + days : days.ToString();
        if (days >= 1)
        {
            timerString = daysString + ":"+ hoursString + ":"+ minutesString + ":"+ secondsString;
        } else
        {
            timerString = hoursString + ":" + minutesString + ":" + secondsString;
        }

        return timerString;
    }

}
