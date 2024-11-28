using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _buttonSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ClickedButton()
    {
        _audioSource.clip = _buttonSound;
        _audioSource.Play();
    }
}
