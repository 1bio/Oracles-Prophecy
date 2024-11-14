using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

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

    private void ConfigureAudioSource(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            if (BGMAudioManager.Instance != null)
                audioSource.volume = BGMAudioManager.GetMonsterVolume();
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
        int index = AudioClipIndex(name);

        AudioClip audioClip = audioClips[index].audioClips[Random.Range(0, audioClips[index].audioClips.Length)];
        _audioSource.clip = audioClip;

        float horizontalDistance = Vector3.Distance(new Vector3(_listener.transform.position.x, 0, _listener.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
        float verticalDistance = Vector3.Distance(new Vector3(0 ,_listener.transform.position.y, 0), new Vector3(0, transform.position.y, 0));
        float distance = horizontalDistance + verticalDistance;

        if (distance <= _audioSource.maxDistance)
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

        Object.Destroy(gameObject, audioClip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
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
