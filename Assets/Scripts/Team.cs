using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    [SerializeField]
    private TeamDashboard teamDashboard;    //레이싱 중 표시할 팀 현황판
    public void Initialize(int id) {
        this.id = id;
        if(id == 1)
            name = "Citadel Eagle";
        else if(id == 2)
            name = "Black Cypher";
    }
    private string name;
    private int id;
    private bool isReady = false;   //레이싱 준비
    private bool isFinished = false;    //레이싱 완료
    private int engineLevel;    //핸디캡 레벨
    private int score = 0;
    private int speed = 0;
    private float gauge = 0;
    private int laps = 0;
    private int repeats = 0;
    private bool canWork = true; //0 : 운동불가능 , 1 : 운동가능
    private List<int> items = new List<int>();
    private TimeSpan[] lapTimes = new TimeSpan[2];
    private bool buttonPressed = false;
    private bool isWinner = false;

    public void GameStart() {   //레이싱 시작
        teamDashboard = UI_Manager.instance.gameStatus.GetTeamDashboard(id);
        items.Clear();
        StartCoroutine("GaugeCoroutine");
    }

    public void SetSpeed(int speed) {   //속도 설정
        this.speed = speed;
        teamDashboard.UpdateSpeedometer(speed);
    }

    public void IncreScore(int score) { //근전도점수 증가
        if(canWork) {
            this.score += score;
            ++repeats;
            teamDashboard.UpdateScore(this.score);
            gauge += score * GameManager.instance.gaugeIncrement;
        }
    }

    public void IncreLaps() {   //랩 증가
        if(!isFinished) {
            lapTimes[laps] = DateTime.Now - GameManager.instance.startTime;
            int enemyId = (id == 1) ? 2 : 1;
            TimeSpan enemyLapTime = GameManager.instance.GetTeam(enemyId).ReturnLapTime(laps);
            teamDashboard.UpdateLapTime(lapTimes[laps], lapTimes[laps++] - enemyLapTime);
        }
        if(laps >= GameManager.instance.goalLaps) {
            isFinished = true;
            GameManager.instance.CheckGameOver();
        }
    }

    public void PressButton() { //버튼 누름
        if(GameManager.instance.isRacing) {
            UseItem();
        }
        else {
            buttonPressed = true;
        }
    }

    public void GetRandomItem() {
        System.Random random = new System.Random();
        int randomItemId = random.Next(1, 4);
        GetItem(randomItemId);
    }

    public void GetItem(int itemId) {   //아이템 획득
        if(items.Count < GameManager.instance.maxItems) {
            items.Add(itemId);
            teamDashboard.UpdateItems(items);
        }
    }

    private void UseItem() {    //아이템 사용
        if(items.Count > 0) {
            int itemId = items[0];
            items.RemoveAt(0);
            teamDashboard.UpdateItems(items);
            ActivateItem(itemId);
        }
    }

    private void ActivateItem(int itemId) { //아이템 효과 발동
        string message = "A" + itemId;
        int enemyId = (id == 1) ? 2 : 1;
        Team enemyTeam = GameManager.instance.GetTeam(enemyId);
        switch(itemId) {
        case 1:
            this.Boost();
            SerialManager.instance.SendToTeam(id, message);
            break;
        case 2:
            enemyTeam.Freeze();
            SerialManager.instance.SendToTeam(enemyId, message);
            break;
        case 3:
            enemyTeam.Inverse();
            SerialManager.instance.SendToTeam(enemyId, message);
            break;
        }
    }

    public void SetEngineLevel(int level) { //핸디캡 설정
        engineLevel = level;
    }

    public void SetWinner() {
        isWinner = true;
    }

    public void GameOver() {
        StopAllCoroutines();
    }

    public void Boost(bool isGaugeBoost = false) {  //부스트
        teamDashboard.UpdateCondition(1);
        float duration = GameManager.instance.boosterDuration;
        StopAllCoroutines();
        if(isGaugeBoost) {
            StartCoroutine(GaugeVacateCoroutine(duration));
        }
        else {
            StartCoroutine(GaugeStopCoroutine(duration));
        }
    } 

    public void Freeze() {  //정지
        teamDashboard.UpdateCondition(2);
        float duration = GameManager.instance.freezeDuration;
        StopAllCoroutines();
        StartCoroutine(GaugeStopCoroutine(duration));
    }

    public void Inverse() { //조작 반전
        teamDashboard.UpdateCondition(3);
    }

    IEnumerator GaugeCoroutine() {  //기본 상태 : 게이지 꾸준히 감소
        canWork = true;
        while(true) {
            gauge -= GameManager.instance.gaugeReduction * Time.deltaTime;
            if(gauge <= 0.01) gauge = 0;
            else if(gauge > 100) gauge = 100f;
            teamDashboard.UpdateBooster(gauge);
            if(gauge >= 99.9) {
                Boost(true);
                SerialManager.instance.SendToTeam(id, "A1");
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator GaugeStopCoroutine(float duration) {    //부스터, 정지 상태 : 게이지 정지
        canWork = false;
        while(duration > 0.01f) {
            duration -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine("GaugeCoroutine");
        yield break;
    }

    IEnumerator GaugeVacateCoroutine(float duration) {  //게이지로 발동된 부스터 : 게이지 감소
        canWork = false;
        float previousGauge = gauge;
        while(gauge > 0.01f) {
            gauge -= previousGauge / duration * Time.deltaTime;
            teamDashboard.UpdateBooster(gauge);
            yield return null;
        }
        StartCoroutine("GaugeCoroutine");
        yield break;
    }

    //이하 값반환 Get함수
    public bool ReturnButtonState() {
        return buttonPressed;
    }

    public void ResetButtonState() {
        buttonPressed = false;
    }

    public int ReturnScore() {
        return score;
    }

    public int ReturnRepeats() {
        return repeats;
    }

    public bool ReturnIsWinner() {
        return isWinner;
    }

    public int ReturnEngineLevel() {
        return engineLevel;
    }

    public bool ReturnIsReady() {
        return isReady;
    }

    public bool ReturnCanWork() {
        return canWork;
    }

    public bool ReturnIsFinished() {
        return isFinished;
    }

    public string ReturnName() {
        return name;
    }

    public TimeSpan ReturnLapTime(int targetLap) {
        if(laps <= targetLap) 
            return TimeSpan.Zero;
        return lapTimes[targetLap];
    }
}