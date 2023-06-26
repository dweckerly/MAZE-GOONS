using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementTracker : MonoBehaviour
{
    int playTime;

    public TMP_Text timeText;

    private void Start() 
    {
        StartCoroutine(RecordTime());    
    }

    public IEnumerator RecordTime()
    {
        TimeSpan ts;
        while (true)
        {
            yield return new WaitForSeconds(1);
            playTime += 1;
            ts = TimeSpan.FromSeconds(playTime);
            timeText.text = ((int)ts.TotalHours).ToString() + ":" + ts.Minutes.ToString() + ":" + ts.Seconds.ToString();
        }
    }
}
