using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _buttonSound;
    [SerializeField] private AudioClip _equipmentSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ClickedButton()
    {
        _audioSource.clip = _buttonSound;
        _audioSource.Play();
    }

    public void Equipment()
    {
        _audioSource.clip = _equipmentSound;
        _audioSource.Play();
    }
}
