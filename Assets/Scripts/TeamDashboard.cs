using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamDashboard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private Speedometer speedometer;
    [SerializeField]
    private Booster booster;
    [SerializeField]
    private LapTime[] lapTimes;
    [SerializeField]
    private ItemSlots itemSlots;
    [SerializeField]
    private CarCondition carCondition;
    private int currentLap = 0;

    private void OnEnable() {
        currentLap = 0;
        UpdateScore(0);
        UpdateSpeedometer(0);
        UpdateBooster(0);
        UpdateItems(new List<int>());
    }

    public void UpdateScore(int score) {
        scoreText.text = string.Format("{0:#,##0}", score);
    }
    public void UpdateSpeedometer(int speed) {
        speedometer.SetGauge(speed);
    }

    public void UpdateBooster(float boostValue) {
        booster.SetGauge(boostValue);
    }

    public void UpdateLapTime(TimeSpan laptime, TimeSpan laptimeDelta) {
        if(currentLap >= GameManager.instance.goalLaps)
            return;
        lapTimes[currentLap++].SetLapTime(laptime, laptimeDelta);
    }

    public void UpdateItems(List<int> itemList) {
        itemSlots.SetItemSlots(itemList);
    }

    public void UpdateCondition(int itemId) {
        carCondition.SetCarCondition(itemId);
    }
}
