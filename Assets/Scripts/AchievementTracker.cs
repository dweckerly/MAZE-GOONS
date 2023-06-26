using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementTracker : Interactable
{
    public UIManager uIManager;
    public TMP_Text timeText;
    public TMP_Text slimeText;
    public TMP_Text offeringText;
    public TMP_Text fountainText;
    public TMP_Text cacheText;

    public Attributes blackSlime;
    public EnemySpawner[] fountains;
    public OfferingUI offering;
    public CacheFound cache;

    int playTime;

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

    bool CheckSlime()
    {
        return !blackSlime.alive;
    }

    bool CheckFountains()
    {
        foreach(EnemySpawner e in fountains)
        {
            if (e.canSpawn) return false;
        }
        return true;
    }

    bool CheckOffering()
    {
        return offering.madeOffering;
    }

    bool CheckCache()
    {
        return cache.found;
    }

    public override void Interact(PlayerStateMachine stateMachine)
    {
        slimeText.text = CheckSlime().ToString();
        offeringText.text = CheckOffering().ToString();
        fountainText.text = CheckFountains().ToString();
        cacheText.text = CheckCache().ToString();
        uIManager.ShowAchievmentsUI();
    }
}
