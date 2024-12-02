using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour, IManager
{
    // TODO
    /*
     * 효과음의 경우 사용 빈도가 빈번하므로 리소스에서 매번 불러오지 않게 한다.
     * 스테이지가 달라도 사용되는 효과음은 같음
     * 배경음은 그냥 써도 될것같음
     * 아무래도 저작권 음원, 쓸것같네 = 리소스에 등록금지
     */

    public const string MASTER_VOLUME_KEY = "Master";
    public const string BGM_VOLUME_KEY = "BGM";
    public const string SFX_VOLUME_KEY = "SFX";
    public const float MAX_VOLUME_DB = 0f;
    public const float MIN_VOLUME_DB = -60f;
    private const float FADE_DELAY = 0.1f;
    
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _BGM;
    [SerializeField] private AudioSource _SFX;

    private BGMList _bgmList;
    private SFXList _sfxList;

    private Coroutine _bgmCleaner;
    private Coroutine _bgmChanger;
    private Coroutine _fadeRoutine;

    private WaitForSeconds _delaySec = new WaitForSeconds(FADE_DELAY);

    private SaveValues _soundValue = new SaveValues();
    private string _tempKey;

    #region 임시

    [SerializeField] private float FadeMaxVolume;
    [SerializeField] private float FadeMinVolume;

    #endregion

    public void Init()
    {
        Instance = this;
        ConnectSources();
        LoadVolumes();
    }


    private void Start()
    {
        LoadVolumes(); // mixer 초기화 버그로 인해 2번 호출
    }

    private void LoadVolumes()
    {
        float value;
        
        value = _soundValue.LoadData(MASTER_VOLUME_KEY);
        if (MIN_VOLUME_DB <= value && value <= MAX_VOLUME_DB)
            _mixer.SetFloat(MASTER_VOLUME_KEY, value);

        value = _soundValue.LoadData(BGM_VOLUME_KEY);
        if (MIN_VOLUME_DB <= value && value <= MAX_VOLUME_DB)
            _mixer.SetFloat(BGM_VOLUME_KEY, value);

        value = _soundValue.LoadData(SFX_VOLUME_KEY);
        if (MIN_VOLUME_DB <= value && value <= MAX_VOLUME_DB)
            _mixer.SetFloat(SFX_VOLUME_KEY, value);

    }

    private void SaveVolumes()
    {
        float value;

        _mixer.GetFloat(MASTER_VOLUME_KEY,out value);
        _soundValue.SaveData(MASTER_VOLUME_KEY, value);

        _mixer.GetFloat(BGM_VOLUME_KEY, out value);
        _soundValue.SaveData(BGM_VOLUME_KEY, value);

        _mixer.GetFloat(SFX_VOLUME_KEY, out value);
        _soundValue.SaveData(SFX_VOLUME_KEY, value);
    }

    private void ConnectSources()
    {
        _bgmList = GetComponentInChildren<BGMList>();
        _sfxList = GetComponentInChildren<SFXList>();

        _BGM = _bgmList.Source;
        _SFX = _sfxList.Source;
    }

    /// <summary>
    /// 해당 오디오믹서의 데시벨을 설정합니다.
    /// </summary>
    public void SetDBVolume(E_SoundType type, float value)
    {
        switch (type)
        {
            case E_SoundType.Master:
                _tempKey = MASTER_VOLUME_KEY;
                break;
            case E_SoundType.BGM:
                _tempKey = BGM_VOLUME_KEY;
                break;
            case E_SoundType.SFX:
                _tempKey = SFX_VOLUME_KEY;
                break;
            default:
                _tempKey = "";
                break;
        }

        _mixer.SetFloat(_tempKey, value);
    }

    public float GetDBVolume(E_SoundType type)
    {
        float volume;

        switch (type)
        {
            case E_SoundType.Master:
                _mixer.GetFloat(MASTER_VOLUME_KEY,out volume);
                return volume;

            case E_SoundType.BGM:
                _mixer.GetFloat(BGM_VOLUME_KEY, out volume);
                return volume;
                
            case E_SoundType.SFX:
                _mixer.GetFloat(SFX_VOLUME_KEY, out volume);
                return volume;
        }

        return float.MinValue;
    }

    /// <summary>
    /// sfx 타입의 음원을 재생합니다.
    /// </summary>
    public void Play(E_SFX sfxClip)
    {
        _SFX.PlayOneShot(_sfxList[sfxClip]);
    }

    /// <summary>
    /// 공간계 SFX 타입의 음원을 재생합니다.
    /// networkType.private = 개인만 진행, public = 공용 진행
    /// </summary>
    public void Play(Vector3 requestPos, E_SFX sfxClip, E_NetworkType networkType = E_NetworkType.Private)
    {
        if(networkType == E_NetworkType.Private)
        {
            SFXObject newSfx = ObjPoolManager.Instance.GetObject<SFXObject>(E_Object.Sfx);

            newSfx.Init(_sfxList[sfxClip], requestPos);
            newSfx.Play();
        }
        else
        {
            RPCDelegate.Instance.PlaySFX(requestPos, sfxClip);
        }
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

        _BGM.PlayOneShot(clip);
        _bgmCleaner = StartCoroutine(DelayClean(clip.length));
    }

    private IEnumerator DelayClean(float delay)
    {
        yield return new WaitForSeconds(delay);
        _BGM.clip = null;
    }

    private IEnumerator ClipChange(AudioClip clip,float fadeInTime,float fadeOutTime)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        yield return  FadeRoutine(fadeOutTime,false);
        _BGM.PlayOneShot(clip);
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

                gap = (FadeMaxVolume - _BGM.volume) / duration;

                while (true)
                {
                    if (_BGM.volume >= FadeMaxVolume)
                    {
                        _BGM.volume = FadeMaxVolume;
                        break;
                    }

                    _BGM.volume += gap * FADE_DELAY;

                    yield return _delaySec;
                }

                break;

            case false: // Out

                gap = (_BGM.volume - FadeMinVolume) / duration;

                while (true)
                {
                    if (FadeMinVolume >= _BGM.volume)
                    {
                        _BGM.volume = FadeMinVolume;
                        break;
                    }

                    _BGM.volume -= gap * FADE_DELAY;

                    yield return _delaySec;
                }

                break;
        }
    }

    private void OnDisable()
    {
        SaveVolumes();
    }
}

public class SaveValues
{
    public float LoadData(string key)
    {
        if (PlayerPrefs.HasKey(key) == true)
        {
            return PlayerPrefs.GetFloat(key);
        }

        return float.MinValue;
    }

    public void SaveData(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
}