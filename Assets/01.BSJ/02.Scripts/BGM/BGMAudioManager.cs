using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SFB_AudioSnapshotExporter;

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

    private bool _isBGMSwitched = false;
    private bool _isCombat = false;

    private BGMAudioName _lastPlayBGM = BGMAudioName.None;

    public float GetMonsterVolume() { return _monsterVolume; }
    public float GetPlayerVolume() { return _playerVolume; }
    public void SetIsCombat(bool isCombat) { _isCombat = isCombat; }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.spatialBlend = 0f;
        _audioSource.loop = true;

        PlayBGM(BGMAudioName.Dungeon.ToString());
    }
        
    private void Update()
    {
        if (_audioSource != null)
        {
            _audioSource.volume = _bgmVolume;
        }
    }

    private void LateUpdate()
    {
        if (_isCombat)
        {
            if (_lastPlayBGM != BGMAudioName.Combat)
                PlayCombatBGM();
        }
        else if (_isBGMSwitched)
        {
            if (_lastPlayBGM != BGMAudioName.Dungeon)
                PlayDungeonBGM();
        }
    }

    private void PlayDungeonBGM()
    {
        StartCoroutine(SwitchBGM(BGMAudioName.Dungeon.ToString(), 2, 0));
        _lastPlayBGM = BGMAudioName.Dungeon;
        _isBGMSwitched = false;
    }
    private void PlayCombatBGM()
    {
        StartCoroutine(SwitchBGM(BGMAudioName.Combat.ToString()));
        _lastPlayBGM = BGMAudioName.Combat;
        _isBGMSwitched = true;
    }

    private void PlayBGM(string bgmName)
    {
        float startVolume = _bgmVolume;

        int index = AudioClipIndex(bgmName);
        _audioSource.clip = _bgmAudioClips[index].audioClips[Random.Range(0, _bgmAudioClips[index].audioClips.Length)];

        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
            _bgmVolume = startVolume;
        }
    }

    private IEnumerator SwitchBGM(string bgmName)    // ±âº» °ª 0
    {
        return SwitchBGM(bgmName, 0f, 0f);
    }

    private IEnumerator SwitchBGM(string bgmName, float delayTime, float targetVolume)
    {
        float startVolume = _bgmVolume;

        for (float t = 0; t < delayTime; t += Time.deltaTime)
        {
            _bgmVolume = Mathf.Lerp(startVolume, targetVolume, t / delayTime);
            yield return null;
        }
        _bgmVolume = targetVolume;

        int index = AudioClipIndex(bgmName);
        _audioSource.clip = _bgmAudioClips[index].audioClips[Random.Range(0, _bgmAudioClips[index].audioClips.Length)];

        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();

            for (float t = 0; t < delayTime; t += Time.deltaTime)
            {
                _bgmVolume = Mathf.Lerp(targetVolume, startVolume, t / delayTime);
                yield return null;
            }
        }
        _bgmVolume = startVolume;
    }

    private int AudioClipIndex(string name)
    {
        for (int i = 0; i < _bgmAudioClips.Count; i++)
        {
            if (_bgmAudioClips[i].name == name)
            {
                return i;
            }
        }
        Debug.LogError("Did not find an audioClips[] entry named \"" + name + "\".");
        return 0;
    }

    enum BGMAudioName
    {
        None,
        Dungeon,
        Combat
    }
}
