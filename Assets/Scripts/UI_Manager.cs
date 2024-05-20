using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance; // 싱글톤 인스턴스
    public enum CanvasState {Ready, Handicap, Status, Result, Ranking}; //장면 상태 enum
    public GameStatus gameStatus;   //게임 현황판

    [SerializeField]
    private GameObject[] canvasObjs;    //장면 오브젝트
    private CanvasState canvasState = CanvasState.Ranking;
    private ButtonUI currentButtons;    //버튼 입력 UI
    // Start is called before the first frame update
    void Awake()
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

    public void ChangeScene() { //장면 전환
        if(++canvasState > CanvasState.Ranking)
            canvasState = CanvasState.Ready;

        GameObject currentCanvasObj = canvasObjs[(int)canvasState];
        for(int i = 0; i < canvasObjs.Length; ++i) {
            canvasObjs[i].SetActive(false);
        }
        currentCanvasObj.SetActive(true);
        ProcessSceneData();
    }

    private void ProcessSceneData() {   //장면마다 필요한 과정 실행
        switch(canvasState) {
        case CanvasState.Ready:
            GameManager.instance.InitializeTeams();
            break;
        case CanvasState.Handicap:
            break;
        case CanvasState.Status:
            GameManager.instance.SetEngineLevel();
            SoundManager.instance.PlayRacingMusic();
            break;
        case CanvasState.Result:
            SoundManager.instance.PlayNonRacingMusic();
            break;
        }
        //이하 버튼 UI 초기화
        GameManager.instance.ResetButtonState();
        if(canvasState != CanvasState.Status) {
            currentButtons = canvasObjs[(int)canvasState].GetComponent<ButtonUI>();
            if(GameManager.instance.isSingleMode) {
                StartCoroutine(currentButtons.CheckSingleButtonPressedCoroutine());
            }
            else {
                StartCoroutine(currentButtons.CheckButtonPressedCoroutine());
            }
        }
    }
}
