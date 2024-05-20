using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speedometer : MonoBehaviour
{
    [SerializeField]
    protected int maxValue;
    [SerializeField]
    protected TextMeshProUGUI valueText;
    [SerializeField]
    protected Image outerGauge;
    [SerializeField]
    protected Image innerGauge;

    public virtual void SetGauge(float value) {
        valueText.text = ((int)value).ToString();
        outerGauge.fillAmount = value / maxValue * 0.65f;
        innerGauge.fillAmount = value / maxValue * 0.645f;
    }
}
