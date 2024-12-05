using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private E_SoundType _sound;
    [SerializeField] private TMP_Text _controlText;
    [SerializeField] private TMP_Text _ValueText;
    [SerializeField] private Slider _slider;
    
    private float _min = SoundManager.MIN_VOLUME_DB;
    private float _max = SoundManager.MAX_VOLUME_DB;

    private float _value;

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(ChangeVolumeText); // onenable
        _slider.onValueChanged.AddListener(ChangeVolume);

        _value = SoundManager.Instance.GetDBVolume(_sound);
    }

    private void Start()
    {
        _slider.value = ConvertDBToSlider(_value); // 초기화과정
    }

    private float ConvertSliderToDB(float sliderValue)
    {
        return _min + ((sliderValue / 100) * (_max - _min));
    }

    private float ConvertDBToSlider(float dbValue)
    {
        return (dbValue - _min) / (_max - _min) * 100;
    }

    private void ChangeVolumeText(float value)
    {
        _ValueText.text = ((int)value).ToString();
    }

    private void ChangeVolume(float value)
    {
        // input 0~100

        float convertedValue =
            _min + ((value - 0) * (_max - _min)) / 100;

        SoundManager.Instance.SetDBVolume(_sound, convertedValue);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveAllListeners();
    }
}
