using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour, IManager
{
    // TODO
    /*
     * 효과음의 경우 사용 빈도가 빈번하므로 리소스에서 매번 불러오지 않게 한다.
     * 스테이지가 달라도 사용되는 효과음은 같음
     * 배경음은 그냥 써도 될것같음
     * 아무래도 저작권 음원, 쓸것같네 = 리소스에 등록금지
     */

    private const float FADE_DELAY = 0.1f;

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource SFX;

    private BGMList _bgmList;
    private SFXList _sfxList;

    private Coroutine _bgmCleaner;
    private Coroutine _bgmChanger;
    private Coroutine _fadeRoutine;

    private WaitForSeconds _delaySec = new WaitForSeconds(FADE_DELAY);

    #region 임시

    [SerializeField] private float MaxVolume;
    [SerializeField] private float MinVolume;

    #endregion

    public void Init()
    {
        Instance = this;
        ConnectSources();
        SetForceVolume(E_SoundType.BGM , MinVolume);
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //        Play(E_BGM.BG_Test_1,10f,0);
    //}

    private void ConnectSources()
    {
        _bgmList = GetComponentInChildren<BGMList>();
        _sfxList = GetComponentInChildren<SFXList>();

        BGM = _bgmList.Source;
        SFX = _sfxList.Source;
    }

    /// <summary>
    /// 해당 오디오소스의 볼륨을 설정합니다.
    /// </summary>
    public void SetForceVolume(E_SoundType type, float volume)
    {
        switch (type)
        {
            case E_SoundType.BGM:
                BGM.volume = volume;
                break;
            case E_SoundType.SFX:
                SFX.volume = volume;
                break;
        }
    }

    /// <summary>
    /// sfx 타입의 음원을 재생합니다.
    /// </summary>
    public void Play(E_SFX sfxClip)
    {
        SFX.PlayOneShot(_sfxList[sfxClip]);
    }

    /// <summary>
    /// bgm 타입의 음원을 재생합니다.
    /// </summary>
    /// <param name="fadeInTime"> 현재 설정된 볼륨값까지 올리는데 걸리는 시간 </param>
    /// <param name="fadeOutTime"> 현재 설정된 볼륨값에서 0까지 내리는데 걸리는 시간 </param>
    public void Play(E_BGM bgmClip, float fadeInTime = 0, float fadeOutTime = 0)
    {
        AudioClip clip = _bgmList[bgmClip];

        if (fadeInTime + fadeOutTime != 0)
        {
            if (fadeInTime <= 0)
                fadeInTime = 0;

            if (fadeOutTime <= 0)
                fadeOutTime = 0;

            if (_bgmChanger != null)
                StopCoroutine(_bgmChanger);

            if (_bgmCleaner != null)
                StopCoroutine(_bgmCleaner);

            _bgmChanger = StartCoroutine(ClipChange(clip, fadeInTime, fadeOutTime));

            return;
        }

        BGM.PlayOneShot(clip);
        _bgmCleaner = StartCoroutine(DelayClean(clip.length));
    }

    private IEnumerator DelayClean(float delay)
    {
        yield return new WaitForSeconds(delay);
        BGM.clip = null;
    }

    private IEnumerator ClipChange(AudioClip clip,float fadeInTime,float fadeOutTime)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        yield return  FadeRoutine(fadeOutTime,false);
        BGM.PlayOneShot(clip);
        _bgmCleaner = StartCoroutine(DelayClean(clip.length));
        _fadeRoutine = StartCoroutine(FadeRoutine(fadeInTime, true));
    }

    /// <param name="duration">처리 시간</param>
    /// <param name="FadeInMode">true = FadeIn</param>
    /// <returns></returns>
    private IEnumerator FadeRoutine(float duration, bool FadeInMode)
    {
        float gap;

        switch (FadeInMode)
        {
            case true: // In

                gap = (MaxVolume - BGM.volume) / duration;

                while (true)
                {
                    if (BGM.volume >= MaxVolume)
                    {
                        BGM.volume = MaxVolume;
                        break;
                    }

                    BGM.volume += gap * FADE_DELAY;

                    yield return _delaySec;
                }

                break;

            case false: // Out

                gap = (BGM.volume - MinVolume) / duration;

                while (true)
                {
                    if (MinVolume >= BGM.volume)
                    {
                        BGM.volume = MinVolume;
                        break;
                    }

                    BGM.volume -= gap * FADE_DELAY;

                    yield return _delaySec;
                }

                break;
        }
    }
}