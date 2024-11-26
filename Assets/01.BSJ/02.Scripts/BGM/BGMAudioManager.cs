using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMAudioManager : MonoBehaviour
{
    public static BGMAudioManager Instance { get; private set; }

    [SerializeField] private List<BGMAudioClipData> _bgmAudioClips = new List<BGMAudioClipData>();

    private AudioSource _audioSource;

    [Range(0, 1f)]
    [SerializeField] private float _bgmVolume;
    [Range(0, 1f)]
    [SerializeField] private float _monsterVolume;
    [Range(0, 1f)]
    [SerializeField] private float _playerVolume;

    private string _currentSceneName;
    private BGMAudioName _lastPlayBGM = BGMAudioName.None;
    private bool _isSwitching = false;

    public static float BGMVolume { get => Instance._bgmVolume; set => Instance._bgmVolume = value; }
    public static float MonsterVolume { get => Instance._monsterVolume; set => Instance._monsterVolume = value; }
    public static float PlayerVolume { get => Instance._playerVolume; set => Instance._playerVolume = value; }


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
            return;
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.spatialBlend = 0f;
        _audioSource.loop = true;
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _currentSceneName = scene.name;
        SetBGMForScene();
    }

    private void Update()
    {
        if (_audioSource != null)
        {
            _audioSource.volume = _bgmVolume;
            SetBGMForScene();
        }
    }

    public void SetBGMForScene()
    {
        switch (_currentSceneName)
        {
            case "Abandoned Prison":
                PlayBGM(BGMAudioName.Dungeon);
                break;
            case "Village":
                PlayBGM(BGMAudioName.Village);
                break;
            case "MainMenu":
                PlayBGM(BGMAudioName.MainMenu);
                break;
            case "Boss":
                PlayBGM(BGMAudioName.Boss);
                break;
        }
    }

    private void PlayBGM(BGMAudioName bgmName)
    {
        if (_lastPlayBGM == bgmName || _isSwitching) return;

        _lastPlayBGM = bgmName;

        StartCoroutine(SwitchBGM(bgmName.ToString(), 0.5f));
    }

    private IEnumerator SwitchBGM(string bgmName, float transitionTime)
    {
        if (_isSwitching) yield break;

        _isSwitching = true;
        float startVolume = _bgmVolume;

        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            _bgmVolume = Mathf.Lerp(startVolume, 0, t / transitionTime);
            yield return null;
        }

        _audioSource.clip = GetAudioClip(bgmName);
        _audioSource.Play();

        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            _bgmVolume = Mathf.Lerp(0, startVolume, t / transitionTime);
            yield return null;
        }

        _isSwitching = false;
    }

    private AudioClip GetAudioClip(string name)
    {
        foreach (BGMAudioClipData bgmClip in _bgmAudioClips)
        {
            if (bgmClip.GetSceneName() == name)
            {
                return bgmClip.GetAudioClip();
            }
        }
        Debug.LogError($"Audio clip not found for BGM: {name}");
        return null;
    }

    private enum BGMAudioName
    {
        None,
        Dungeon,
        Village,
        MainMenu,
        Boss
    }
}
