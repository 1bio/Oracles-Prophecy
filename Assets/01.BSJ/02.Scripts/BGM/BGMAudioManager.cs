using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BGMAudioManager : MonoBehaviour
{
    public static BGMAudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    [SerializeField] private List<BGMAudioClips> _bgmAudioClips = new List<BGMAudioClips>();

    [System.Serializable]
    public class BGMAudioClips
    {
        public string name;
        public AudioClip[] audioClips;
    }

    private AudioSource _audioSource;

    [Range(0, 1f)]
    [SerializeField] private float _bgmVolume = 0.2f;
    [Range(0, 1f)]
    [SerializeField] private float _monsterVolume = 0.2f;
    [Range(0, 1f)]
    [SerializeField] private float _playerVolume = 0.2f;

    public float GetMonsterVolume() { return _monsterVolume; }
    public float GetPlayerVolume() { return _playerVolume; }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.spatialBlend = 0f;
        _audioSource.loop = true;
    }
        
    private void Update()
    {
        if (_audioSource != null)
        {
            _audioSource.volume = _bgmVolume;
        }
    }
}
