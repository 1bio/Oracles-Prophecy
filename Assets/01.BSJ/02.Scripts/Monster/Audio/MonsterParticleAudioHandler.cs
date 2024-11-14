using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParticleAudioHandler : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;

    private bool _isPlayed;

    void Start()
    {
        _particleSystem = this.gameObject.GetComponentInParent<ParticleSystem>();
        _audioSource = this.gameObject.GetComponent<AudioSource>();

        _audioSource.volume = BGMAudioManager.GetMonsterVolume();
    }

    void Update()
    {
        if (_particleSystem != null && _particleSystem.isPlaying)
        {
            if (!_isPlayed)
            {
                _audioSource.Play();
                _isPlayed = true;
            }
        }
        else
        {
            _audioSource.Stop();
            _isPlayed = false;
        }
    }
}
