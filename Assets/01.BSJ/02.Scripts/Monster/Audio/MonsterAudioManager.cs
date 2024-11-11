using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MonsterAudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    private Animator _animator;

    public List<Monster_AudioClips> audioClips = new List<Monster_AudioClips>();

    private AudioListener _listener;

    [System.Serializable]
    public class Monster_AudioClips
    {
        public string name; 
        public AudioClip[] audioClips;
    }

    private void ConfigureAudioSource()
    {
        if (_audioSource != null)
        {
            _audioSource.volume = 0.2f;
            _audioSource.spatialBlend = 1f;
            _audioSource.dopplerLevel = 0f;
            _audioSource.rolloffMode = AudioRolloffMode.Linear;
            _audioSource.minDistance = 1f;
            _audioSource.maxDistance = 10f;
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        _listener = FindAnyObjectByType<AudioListener>();

        ConfigureAudioSource();
    }

    public void PlayAudio(string name)
    {
        int index = AudioClipIndex(name);

        AudioClip audioClip = audioClips[index].audioClips[Random.Range(0, audioClips[index].audioClips.Length)];
        _audioSource.clip = audioClip;

        ConfigureAudioSource();

        float horizontalDistance = Vector3.Distance(new Vector3(_listener.transform.position.x, 0, _listener.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
        float verticalDistance = Vector3.Distance(new Vector3(0 ,_listener.transform.position.y, 0), new Vector3(0, transform.position.y, 0));
        float distance = horizontalDistance + verticalDistance;

        if (distance <= _audioSource.maxDistance)
        {
            AudioSource.PlayClipAtPoint(audioClip, transform.position, _audioSource.volume);
        }
    }

    private int AudioClipIndex(string name)
    {
        for (int i = 0; i < audioClips.Count; i++)
        {
            if (audioClips[i].name == name)
            {
                return i;
            }
        }
        Debug.LogError("Did not find an audioClips[] entry named \"" + name + "\".");
        return 0;
    }
}
