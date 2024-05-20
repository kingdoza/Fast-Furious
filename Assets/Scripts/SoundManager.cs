using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // 싱글톤 인스턴스
    [SerializeField]
    private AudioClip[] racingClips;
    [SerializeField]
    private AudioClip[] nonRacingClips;
    [SerializeField]
    private AudioClip countdownClip;
    [SerializeField]
    private AudioClip racingStartClip;
    [SerializeField]
    private AudioClip itemObtainClip;
    [SerializeField]
    private AudioClip itemActiveClip;
    [SerializeField]
    private float scrollSpeed;
    [SerializeField]
    private float bgmFadeOutTime;
    [SerializeField]
    private SoundSetting soundSetting;
    [SerializeField]
    private float nonRacingVolume;
    [SerializeField]
    private float racingVolume;
    private int racingPreviousIndex = -1;
    private int nonRacingPreviousIndex = -1;
    private GameObject bgmObject;
    private AudioSource bgm;
    private bool isRacingBgm = true;
    private float soundVolume = 1;
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


    private void Start() {
        bgmObject = new GameObject("BgmObject");
        bgmObject.transform.SetParent(transform);
        bgm = bgmObject.AddComponent<AudioSource>();
        PlayNonRacingMusic();
        ApplySoundVolume();
    }

    private void Update() {
        CheckMouseScroll();
        CheckBgmFinished();
    }

    private void CheckBgmFinished() {
        if(bgm.isPlaying == false)
            PlayBackgroundMusic();
    }

    private void CheckMouseScroll() {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue != 0f) {
            soundVolume += scrollValue * scrollSpeed;
            soundVolume = Mathf.Clamp01(soundVolume);
            ApplySoundVolume();
        }
    }

    private void ApplySoundVolume() {
        SetBgmVolume(soundVolume);
        soundSetting.SetSoundSlider(soundVolume);
    }

    private void SetBgmVolume(float volume) {
        float maxVolume = nonRacingVolume;
        if(isRacingBgm)
            maxVolume = racingVolume;
        bgm.volume = maxVolume * volume;
    }

    private IEnumerator AudioDestroyDelayCoroutine(GameObject audioObject) {
        float delayTime = audioObject.GetComponent<AudioSource>().clip.length;
        yield return new WaitForSeconds(delayTime);
        Destroy(audioObject);
        yield break;
    }

    private IEnumerator BgmFadeOutCoroutine(bool playNext = false) {
        float remainedFadeOutTime = bgmFadeOutTime;
        float volumeRate = remainedFadeOutTime / bgmFadeOutTime;
        while(remainedFadeOutTime >= 0.01f && bgm.isPlaying == true) {
            volumeRate = remainedFadeOutTime / bgmFadeOutTime;
            SetBgmVolume(volumeRate * soundVolume);
            remainedFadeOutTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        bgm.Stop();
        if(playNext == true) {
            isRacingBgm = !isRacingBgm;
            PlayBackgroundMusic();
        }
        yield break;
    }

    private void PlayAudioClip(AudioClip audioClip) {
        GameObject audioObject = new GameObject("AudioObject");
        audioObject.transform.SetParent(transform);
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = soundVolume;
        audioSource.Play();
        StartCoroutine(AudioDestroyDelayCoroutine(audioObject));
    }

    private void PlayBackgroundMusic() {
        AudioClip[] backgroundClips = nonRacingClips;
        ref int previousIndex = ref nonRacingPreviousIndex;
        if(isRacingBgm) {
            backgroundClips = racingClips;
            previousIndex = ref racingPreviousIndex;
        }

        int randomIndex = Random.Range(0, backgroundClips.Length);
        while(randomIndex == previousIndex)
            randomIndex = Random.Range(0, backgroundClips.Length);
        AudioClip backgroundClip = backgroundClips[randomIndex];
        previousIndex = randomIndex;
        bgm.clip = backgroundClip;
        SetBgmVolume(soundVolume);
        bgm.Play();
    }

    public void StopBackgroundMusic() {
        StartCoroutine(BgmFadeOutCoroutine());
    }

    public void PlayNonRacingMusic() {
        racingPreviousIndex = -1;
        if(isRacingBgm == false)
            return;
        StartCoroutine(BgmFadeOutCoroutine(true));
    }

    public void PlayRacingMusic() {
        nonRacingPreviousIndex = -1;
        if(isRacingBgm == true)
            return;
        StartCoroutine(BgmFadeOutCoroutine(true));
    }

    public void CountdownEffect() {
        PlayAudioClip(countdownClip);
    }

    public void RacingStartEffect() {
        PlayAudioClip(racingStartClip);
    }

    public void ItemObtainEffect() {
        PlayAudioClip(itemObtainClip);
    }

    public void ItemActiveEffect() {
        PlayAudioClip(itemActiveClip);
    }
}
