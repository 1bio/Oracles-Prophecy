using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public void FootStepEvent() => AudioManager.instance.PlayFootStepSound();

    public void PlaySwordSwingEvent(int index) => AudioManager.instance.PlaySwingSound(index);

    public void PlaySlashEvent() => AudioManager.instance.PlaySlashSound();
}
