using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlide : MonoBehaviour
{
    public Slider slider;
    public string parameter;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float mutiplier;

    public void SliderValue(float _value) => audioMixer.SetFloat(parameter,Mathf.Log10(_value)* mutiplier);
    
    public void LoadSlider(float _value)
    {
        if(_value >= 0.001f)
            slider.value = _value;
    }

}
