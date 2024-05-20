using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameResult : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gameIdText; //게임 번호 텍스트
    [SerializeField]
    private TextMeshProUGUI[] winloseText = new TextMeshProUGUI[2]; //팀 승패 텍스트
    [SerializeField]
    private TextMeshProUGUI[] rankingText = new TextMeshProUGUI[2]; //팀 순위
    [SerializeField]
    private TeamResult[] teamResults = new TeamResult[3];   //팀 통계
    private Color winColor = new Color(255f, 220f, 0);  //승리팀 텍스트 색깔
    private Color loseColor = new Color(200f, 0, 0);    //패배팀 텍스트 색깔

    private void OnEnable() {
        //GameManager.instance.sql_Requester.SaveTeamStats();
        //GameManager.instance.sql_Requester.DebugAll();
        GameManager.instance.sql_Requester.SaveTeamsStats();
        SetGameResult();
    }

    public void SetGameResult() {   //게임 결과 표시하기
        Team leftTeam = GameManager.instance.leftTeam;
        Team rightTeam = GameManager.instance.rightTeam;
        gameIdText.text = "GAME#" + 1;  //임시
        if(leftTeam.ReturnIsWinner()) {
            winloseText[0].text = "WIN";
            winloseText[0].color = winColor;
            winloseText[1].text = "LOSE";
            winloseText[1].color = loseColor;
        }
        else {
            winloseText[0].text = "LOSE";
            winloseText[0].color = loseColor;
            winloseText[1].text = "WIN";
            winloseText[1].color = winColor;
        }
        //teamResults[0].SetTeamResult(leftTeam);
        //teamResults[1].SetTeamResult(rightTeam);
        GameRecord averageRecord = GameManager.instance.sql_Requester.GetAverageRecord();
        int gameId = int.Parse(averageRecord.id);
        GameRecord leftTeamRecord = GameManager.instance.sql_Requester.GetTeamRecord(gameId, leftTeam);
        GameRecord rightTeamRecord = GameManager.instance.sql_Requester.GetTeamRecord(gameId, rightTeam);
        teamResults[2].SetRecordResult(averageRecord, true);
        teamResults[0].SetRecordResult(leftTeamRecord);
        teamResults[1].SetRecordResult(rightTeamRecord);
    }
}