using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BGMData", menuName = "Data/Audio/BGMAudioClipData")]
public class BGMAudioClipData : ScriptableObject
{
    [SerializeField] private string _sceneName;
    [SerializeField] private AudioClip _audioClip;

    public string GetSceneName() { return _sceneName; }
    public AudioClip GetAudioClip() { return _audioClip; }
}
