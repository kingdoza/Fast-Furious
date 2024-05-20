using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Booster : Speedometer
{
    public override void SetGauge(float value) {
        valueText.text = Math.Round(value).ToString();
        float gaugeFill = value / maxValue;
        outerGauge.fillAmount = gaugeFill * 0.65f;
        innerGauge.fillAmount = gaugeFill * 0.645f;
        outerGauge.color = new Color(1f, 1f - gaugeFill, 0f);
    } 
}
