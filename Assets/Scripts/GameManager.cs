using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤 인스턴스
    public DB_Requester sql_Requester;
    public bool isSingleMode;
    public bool isRacing = false;
    [HideInInspector]
    public Team leftTeam;
    [HideInInspector]
    public Team rightTeam;

    public int goalLaps;
    public int maxItems;
    public float gaugeReduction;
    public float gaugeIncrement;
    public float boosterDuration;
    public float freezeDuration;
    public float chaosDuration;

    [HideInInspector]
    public DateTime startTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 다른 씬으로 넘어갈 때도 유지
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있다면 새로 생성된 인스턴스 파괴
        }
    }

    private void Start() {  //맨 처음 화면으로 전환
        UI_Manager.instance.ChangeScene();
    }

    public void InitializeTeams() { //새로운 게임 시 팀 초기화
        if(leftTeam != null)
            Destroy(leftTeam);
        if(rightTeam != null)
            Destroy(rightTeam);

        leftTeam = gameObject.AddComponent<Team>();
        leftTeam.Initialize(1);
        rightTeam = gameObject.AddComponent<Team>();
        rightTeam.Initialize(2);
        SerialManager.instance.SetSerialPathTeam();
    }

    public void SetEngineLevel() {  //핸디캡 레벨 설정하기
        SerialManager.instance.SendToLeft("H" + leftTeam.ReturnEngineLevel());
        SerialManager.instance.SendToRight("H" + rightTeam.ReturnEngineLevel());
        //Debug.Log("Send : H" + leftTeam.ReturnEngineLevel());
        //Debug.Log("Send : H" + rightTeam.ReturnEngineLevel());
    }

    public void GameStart() {   //레이싱 시작
        isRacing = true;
        SerialManager.instance.SendToLeft("G");
        SerialManager.instance.SendToRight("G");
        startTime = DateTime.Now;
        leftTeam.GameStart();
        rightTeam.GameStart();
    }

    public void CheckGameOver() {   //레이싱 종료 확인
        if(isSingleMode) {
            if(leftTeam.ReturnIsFinished() || rightTeam.ReturnIsFinished())
                GameOver();
        }
        else {
            if(leftTeam.ReturnIsFinished() && rightTeam.ReturnIsFinished())
                GameOver();
        }
    }

    private void GameOver() {   //레이싱 종료
        isRacing = false;
        rightTeam.GameOver();
        leftTeam.GameOver();
        UI_Manager.instance.gameStatus.StartLoading();

        TimeSpan leftTeamRacingTime = leftTeam.ReturnLapTime(goalLaps - 1);
        TimeSpan rightTeamRacingTime = rightTeam.ReturnLapTime(goalLaps - 1);
        if(leftTeamRacingTime > rightTeamRacingTime) 
            rightTeam.SetWinner();
        else
            leftTeam.SetWinner();
    }

    public void ResetButtonState() {    //양 팀 버튼상태 초기화
        leftTeam.ResetButtonState();
        rightTeam.ResetButtonState();
    }

    public Team GetTeam(int teamId) {   //팀 아이디로 팀 스크립트 가져오기
        if(teamId == 1)
            return leftTeam;
        else
            return rightTeam;
    }
}