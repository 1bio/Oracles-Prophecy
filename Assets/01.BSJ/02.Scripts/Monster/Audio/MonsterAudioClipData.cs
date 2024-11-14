using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster AudioClip Data", menuName = "Data/Audio/Monster AudioClip Data")]
public class MonsterAudioClipData : ScriptableObject
{
    [SerializeField] private string _audioClipName;
    [SerializeField] private AudioClip[] _audioClips;

    public string GetAudioClipName() { return _audioClipName; }
    public AudioClip[] GetAudioClips() { return _audioClips; }
}
