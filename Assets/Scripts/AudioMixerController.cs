using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] public Slider m_MusicMasterSlider;

    private void Awake()
    {
        m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    public void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void Mute()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

}
