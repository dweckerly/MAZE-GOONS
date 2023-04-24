using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsButtons : MonoBehaviour
{
    public TMP_Text TotalPoints;
    public TMP_Text BrawnPoints;
    public TMP_Text BrainPoints;
    public TMP_Text GutsPoints;
    public TMP_Text GuilePoints;

    int points = 9;

    int brawn = 1;
    int brains = 1;
    int guts = 1;
    int guile = 1;

    private void Start() 
    {
        TotalPoints.text = points.ToString();
        BrawnPoints.text = brawn.ToString();
        BrainPoints.text = brains.ToString();
        GutsPoints.text = guts.ToString();
        GuilePoints.text = guile.ToString();
    }

    public void AddBrawn()
    {
        if (points <= 0) return;
        brawn++;
        BrawnPoints.text = brawn.ToString();
        points--;
        TotalPoints.text = points.ToString();
    }

    public void AddBrains()
    {
        if (points <= 0) return;
        brains++;
        BrainPoints.text = brains.ToString();
        points--;
        TotalPoints.text = points.ToString();
    }

    public void AddGuts()
    {
        if (points <= 0) return;
        guts++;
        GutsPoints.text = guts.ToString();
        points--;
        TotalPoints.text = points.ToString();
    }

    public void AddGuile()
    {
        if (points <= 0) return;
        guile++;
        GuilePoints.text = guile.ToString();
        points--;
        TotalPoints.text = points.ToString();
    }

    public void SubtractBrawn()
    {
        if (brawn <= 1) return;
        brawn--;
        BrawnPoints.text = brawn.ToString();
        points++;
        TotalPoints.text = points.ToString();
    }

    public void SubtractBrains()
    {
        if (brains <= 1) return;
        brains--;
        BrainPoints.text = brains.ToString();
        points++;
        TotalPoints.text = points.ToString();
    }

    public void SubtractGuts()
    {
        if (guts <= 1) return;
        guts--;
        GutsPoints.text = guts.ToString();
        points++;
        TotalPoints.text = points.ToString();
    }

    public void SubtractGuile()
    {
        if (guile <= 1) return;
        guile--;
        GuilePoints.text = guile.ToString();
        points++;
        TotalPoints.text = points.ToString();
    }
}
