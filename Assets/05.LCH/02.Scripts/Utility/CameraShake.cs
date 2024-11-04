using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;

    private float shakeTime;

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel = virtualCam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannel.m_AmplitudeGain = intensity; // 흔들림 강도
        shakeTime = time; // 흔들림 지속 시간
    }

    public void ShakeCamera_Event()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel = virtualCam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannel.m_AmplitudeGain = 1f; // 흔들림 강도
        shakeTime = 0.2f; // 흔들림 지속 시간
    }

    private void Update()
    {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            if (shakeTime <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel = virtualCam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannel.m_AmplitudeGain = 0; // 흔들림 강도를 0으로 초기화
            }
        }
    }
}
