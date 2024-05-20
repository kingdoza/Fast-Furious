using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    public Image leftButtonImage;   //왼쪽팀 버튼 상태
    public Image rightButtonImage;  //오른쪽팀 버튼 상태
    private Color nonActivatedColor = new Color(0f, 255f, 0f);  //버튼 비활성화 색깔
    private Color activatedColor = new Color(0f, 255f, 255f);   //버튼 활성화 색깔
    void OnEnable() {   //버튼 색깔 초기화
        leftButtonImage.color = nonActivatedColor;
        rightButtonImage.color = nonActivatedColor;
    }

    public IEnumerator CheckSingleButtonPressedCoroutine() {    //테스트 모드 버튼 감지
        Team leftTeam = GameManager.instance.leftTeam;
        Team rightTeam = GameManager.instance.rightTeam;
        bool leftButtonPressed = false;
        bool rightButtonPressed = false;
        while(!(leftButtonPressed || rightButtonPressed)) {
            leftButtonPressed = leftTeam.ReturnButtonState();
            rightButtonPressed = rightTeam.ReturnButtonState();
            yield return null;
        }
        UI_Manager.instance.ChangeScene();
        yield break;
    }

    public IEnumerator CheckButtonPressedCoroutine() {  //실제 모드 버튼 감지
        Team leftTeam = GameManager.instance.leftTeam;
        Team rightTeam = GameManager.instance.rightTeam;
        bool leftButtonPressed = false;
        bool rightButtonPressed = false;
        while(!(leftButtonPressed && rightButtonPressed)) {
            leftButtonPressed = leftTeam.ReturnButtonState();
            rightButtonPressed = rightTeam.ReturnButtonState();

            if(leftButtonPressed) 
                leftButtonImage.color = activatedColor;
            if(rightButtonPressed) 
                rightButtonImage.color = activatedColor;
            yield return null;
        }
        UI_Manager.instance.ChangeScene();
        yield break;
    }
}