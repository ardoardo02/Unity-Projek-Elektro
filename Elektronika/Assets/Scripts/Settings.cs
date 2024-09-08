using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [Header("Sensitivity")]
    [SerializeField] Slider camSensitivitySlider;
    [SerializeField] TMP_Text camSensitivityText;
    [Header("Audio")]
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] TMP_Text sfxVolumeText;

    const string MIXER_SFX = "SFXVolume";

    private void Start()
    {
        camSensitivitySlider.onValueChanged.AddListener(SetCamSensitivity);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        LoadSettings();
    }

    public void ConfirmChanges()
    {
        PlayerPrefs.SetFloat("CamSens", camSensitivitySlider.value);
        PlayerPrefs.SetFloat(MIXER_SFX, sfxVolumeSlider.value);
    }

    private void LoadSettings()
    {
        float camSensitivity = PlayerPrefs.GetFloat("CamSens", 10f);
        float sfxVolume = PlayerPrefs.GetFloat(MIXER_SFX, 1f);

        camSensitivitySlider.value = camSensitivity;
        sfxVolumeSlider.value = sfxVolume;
        SetCamSensitivity(camSensitivity);
        SetSFXVolume(sfxVolume);
    }

    private void SetCamSensitivity(float value)
    {
        camSensitivityText.text = value.ToString();
    }

    private void SetSFXVolume(float value)
    {
        sfxVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
}
