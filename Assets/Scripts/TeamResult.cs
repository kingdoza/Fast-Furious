using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TeamResult : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI titleText;  //팀: 순위, 평균: 게임아이디
    [SerializeField]
    private TextMeshProUGUI racingTimeText; //레이싱 시간 텍스트
    [SerializeField]
    private TextMeshProUGUI scoreText;  //근전도점수 텍스트
    [SerializeField]
    private TextMeshProUGUI intensityText;  //운동강도 텍스트
    [SerializeField]
    private TextMeshProUGUI caloriesText;   //소모칼로리 텍스트

    public void SetTeamResult(Team myTeam) {
        TimeSpan racingTime = myTeam.ReturnLapTime(GameManager.instance.goalLaps - 1);
        racingTimeText.text = string.Format("{0:D2}:{1:D2}.{2:D3}", racingTime.Minutes, racingTime.Seconds, racingTime.Milliseconds);
        scoreText.text = string.Format("{0:#,##0}", myTeam.ReturnScore());
        float intensity = (float)myTeam.ReturnScore() / myTeam.ReturnRepeats();
        if(float.IsNaN(intensity)) intensity = 0;
        intensityText.text = string.Format("{0:#,##0}", intensity);
        float calories = myTeam.ReturnScore() * 0.0008f;
        caloriesText.text = string.Format("{0:#,##0}", calories);
    }

    public void SetRecordResult(GameRecord gameRecord, bool isAverage = false) {
        TimeSpan racingTime = TimeSpan.FromSeconds(double.Parse(gameRecord.racingTime));
        racingTimeText.text = string.Format("{0:D2}:{1:D2}.{2:D3}", racingTime.Minutes, racingTime.Seconds, racingTime.Milliseconds);

        float score = float.Parse(gameRecord.score);
        scoreText.text = string.Format("{0:#,##0}", score);

        float repeats = float.Parse(gameRecord.repeats);
        float intensity = score / repeats;
        if(float.IsNaN(intensity)) intensity = 0;
        intensityText.text = string.Format("{0:#,##0}", intensity);

        float calories = score * 0.016f;
        caloriesText.text = string.Format("{0:#,##0}", calories);

        if(isAverage)
            titleText.text = "GAME#" + gameRecord.id;
        else
            titleText.text = gameRecord.myRank + "/" + gameRecord.gamesCount;
    }
}
