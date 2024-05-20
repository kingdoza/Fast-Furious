using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RankingBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI racingTimeText;
    [SerializeField]
    private TextMeshProUGUI teamText;
    public void SetTopRecord(GameRecord gameRecord) {
        TimeSpan racingTime = TimeSpan.FromSeconds(double.Parse(gameRecord.racingTime));
        racingTimeText.text = string.Format("{0:D2}:{1:D2}.{2:D3}", racingTime.Minutes, racingTime.Seconds, racingTime.Milliseconds);

        teamText.text = string.Format("GAME#{0} - {1}", gameRecord.id, gameRecord.team);
    }
}
