using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSetting : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI percentageText;
    [SerializeField]
    private Slider soundSlider;
    [SerializeField]
    private float disableDelay;
    private float lastScrollTime = 0;

    private void Start() {
        gameObject.SetActive(false);
    }

    private IEnumerator SettingDisableCoroutine() {
        lastScrollTime = 0;
        while(lastScrollTime < disableDelay) {
            lastScrollTime += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.SetActive(false);
        yield break;
    }
    
    public void SetSoundSlider(float soundValue) {
        StopAllCoroutines();
        gameObject.SetActive(true);
        soundSlider.value = soundValue;
        percentageText.text = Mathf.RoundToInt(soundValue * 100) + "%";
        StartCoroutine(SettingDisableCoroutine());
    }
}
