using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class BossCutscene : MonoBehaviour
{
    private VideoPlayer m_VideoPlayer;
    private AudioSource m_BGMAudioSource;

    private void OnEnable()
    {
        m_VideoPlayer = GetComponent<VideoPlayer>();

        m_VideoPlayer.Play();

        m_VideoPlayer.loopPointReached += VideoEnd;
    }
    private void OnDisable()
    {
        m_VideoPlayer.loopPointReached -= VideoEnd;
    }

    private void Start()
    {
        m_BGMAudioSource = BGMAudioManager.Instance.gameObject.GetComponent<AudioSource>();
        m_BGMAudioSource.mute = true;
    }

    private void VideoEnd(UnityEngine.Video.VideoPlayer vp)
    {
        vp.Stop();
        m_BGMAudioSource.mute = false;
        BGMAudioManager.Instance.SetBGMForScene();

        this.gameObject.SetActive(false);
    }
}
