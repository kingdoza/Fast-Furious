using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Countdown : MonoBehaviour
{
    [SerializeField]
    private double countdownTime;
    [SerializeField]
    private Image timerCircle;
    [SerializeField]
    private TextMeshProUGUI timerText;

    public IEnumerator CountdownCoroutine() {
        gameObject.SetActive(true);
        double currentCountdown = countdownTime;
        timerText.text = "READY";
        timerCircle.fillAmount = 0;
        yield return new WaitForSeconds(2f);
        while(currentCountdown >= 0.005) {
            if(currentCountdown % 1 <= 0.01f)
                SoundManager.instance.CountdownEffect();
            currentCountdown -= 0.01;
            timerText.text = ((int)currentCountdown + 1).ToString();
            timerCircle.fillAmount = (float)currentCountdown % 1;
            yield return new WaitForSeconds(0.01f);
        }
        GameManager.instance.GameStart();
        gameObject.SetActive(false);
        SoundManager.instance.RacingStartEffect();
        yield break;
    }
}
