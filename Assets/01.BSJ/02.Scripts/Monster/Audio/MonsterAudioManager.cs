using System.Collections.Generic;
using UnityEngine;

public class MonsterAudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    private Animator _animator;

    public List<MonsterAudioClipData> audioClips = new List<MonsterAudioClipData>();

    private AudioListener _listener;

    private void ConfigureAudioSource(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            if (BGMAudioManager.Instance != null)
                audioSource.volume = BGMAudioManager.MonsterVolume;
            audioSource.spatialBlend = 1f;
            audioSource.dopplerLevel = 0f;
            audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            audioSource.minDistance = 0.5f;
            audioSource.maxDistance = 20f;
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        _listener = FindAnyObjectByType<AudioListener>();

        ConfigureAudioSource(_audioSource);
    }

    public void PlayAudio(string name)
    {
        AudioClip audioClip = GetRandomAudioClip(name);
        _audioSource.clip = audioClip;

        float horizontalDistance = Vector3.Distance(new Vector3(_listener.transform.position.x, 0, _listener.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
        float verticalDistance = Vector3.Distance(new Vector3(0 ,_listener.transform.position.y, 0), new Vector3(0, transform.position.y, 0));
        float distance = horizontalDistance + verticalDistance;

        if (distance <= _audioSource.maxDistance && audioClip != null)
        {
            PlayCustomAudioClip(audioClip, transform.position);
        }
    }

    private void PlayCustomAudioClip(AudioClip audioClip, Vector3 position)
    {
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;

        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = audioClip;

        ConfigureAudioSource(audioSource);
        audioSource.Play();

        Destroy(gameObject, audioClip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    private AudioClip GetRandomAudioClip(string name)
    {
        foreach (MonsterAudioClipData clipData in audioClips)
        {
            if (clipData.GetAudioClipName() == name)
            {
                return clipData.GetAudioClips()[Random.Range(0, clipData.GetAudioClips().Length)];
            }
        }

        return null;
    }
}
