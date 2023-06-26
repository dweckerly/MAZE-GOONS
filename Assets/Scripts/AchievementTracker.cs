using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementTracker : Interactable
{
    int playTime;

    public TMP_Text timeText;

    public override InteractableType type { get { return InteractableType.GameTracker; } }

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

    public override void Interact(PlayerStateMachine stateMachine)
    {
        throw new NotImplementedException();
    }
}
