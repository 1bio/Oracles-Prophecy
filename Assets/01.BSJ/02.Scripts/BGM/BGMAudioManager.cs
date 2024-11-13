using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMAudioManager : MonoBehaviour
{
    public static BGMAudioManager Instance { get; private set; }

    [SerializeField] private List<BGMAudioClips> _bgmAudioClips = new List<BGMAudioClips>();

    [System.Serializable]
    public class BGMAudioClips
    {
        public string name;
        public AudioClip[] audioClips;
    }

    private AudioSource _audioSource;

    [Range(0, 1f)]
    [SerializeField] private float _bgmVolume;
    [Range(0, 1f)]
    [SerializeField] private float _monsterVolume;
    [Range(0, 1f)]
    [SerializeField] private float _playerVolume;

    private BGMAudioName _lastPlayBGM = BGMAudioName.None;

    private string _currentSceneName;
    private bool _isBGMSwitched = false;

    // Dungeon
    private static bool _isCombat = false;

    public static float GetMonsterVolume() { return Instance._monsterVolume; }
    public static float GetPlayerVolume() { return Instance._playerVolume; } 
    public static void SetIsCombat(bool isCombat) { _isCombat = isCombat; }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }

        int index = scene.name.IndexOf(' ');

        if (index != -1)
            _currentSceneName = scene.name.Substring(0, index);
        else
            _currentSceneName = scene.name;

        _isBGMSwitched = true;
        SetBGMForScene();
    }

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

    private void LateUpdate()
    {
        SetBGMForScene();
    }

    private void SetBGMForScene()
    {
        switch (_currentSceneName)
        {
            case "BSJ":
                DungeonBGM();
                break;
            default:
                break;
        }
    }


    private void DungeonBGM()
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
        StartCoroutine(SwitchBGM(BGMAudioName.Dungeon.ToString(), 1.5f, 0));
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
