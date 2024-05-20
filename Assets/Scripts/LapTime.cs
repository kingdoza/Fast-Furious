using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LapTime : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI laptimeText;
    [SerializeField]
    private TextMeshProUGUI laptimeDeltaText;

    private void OnEnable() {
        laptimeText.text = "";
        laptimeDeltaText.text = "";
    }

    public void SetLapTime(TimeSpan laptime, TimeSpan laptimeDelta) {
        laptimeText.text = string.Format("{0:D2}:{1:D2}.{2:D3}", laptime.Minutes, laptime.Seconds, laptime.Milliseconds);
        if(laptime == laptimeDelta) {
            laptimeDeltaText.color = Color.red;
            laptimeDeltaText.text = "Leader";
        }
        else {
            laptimeDeltaText.color = Color.white;
            laptimeDeltaText.text = string.Format("+{0:D2}.{1:D3}", laptimeDelta.Seconds, laptimeDelta.Milliseconds);
        }
    }
}
