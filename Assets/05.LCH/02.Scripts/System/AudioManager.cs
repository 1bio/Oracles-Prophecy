using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource playerAudio;
    public Animator playerAnimator;

    [Header("FootStep")]
    [SerializeField] private AudioClip[] footStep;
    [Range(0f, 1f)][SerializeField] private float footStepVolume;
    private bool isLeftFootStep = true;

    [Header("Sword Swing")]
    [SerializeField] private AudioClip[] swordSwing;
    [Range(0f, 1f)][SerializeField] private float swordSwingVolume;

    [Header("Slash")]
    [SerializeField] private AudioClip slash;
    [Range(0f, 1f)][SerializeField] private float slashVolume;

    public void Init()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Awake()
    {
        Init();
    }

    public void PlayFootStepSound()
    {
        playerAudio.volume = footStepVolume;

        switch (SceneController.instance.ReturnCurrentSceneName())
        {
            case "Viliage":
                if (playerAnimator.GetFloat("Velocity") > 0.8f || playerAnimator.GetFloat("RangeVelocity") > 0.8f)
                {
                    if (isLeftFootStep)
                    {
                        playerAudio.PlayOneShot(footStep[0]);
                    }
                    else
                    {
                        playerAudio.PlayOneShot(footStep[1]);
                    }

                    isLeftFootStep = !isLeftFootStep;
                }
                break;

            case "Dungeon":
                if (playerAnimator.GetFloat("Velocity") > 0.8f || playerAnimator.GetFloat("RangeVelocity") > 0.8f)
                {
                    if (isLeftFootStep)
                    {
                        playerAudio.PlayOneShot(footStep[2]);
                    }
                    else
                    {
                        playerAudio.PlayOneShot(footStep[3]);
                    }

                    isLeftFootStep = !isLeftFootStep;
                }
                break;
        }
    }

    public void PlaySwingSound(int index)
    {
        playerAudio.volume = swordSwingVolume;

        switch (index)
        {
            case 0: playerAudio.PlayOneShot(swordSwing[0]); break;
            case 1: playerAudio.PlayOneShot(swordSwing[1]); break;
            case 2: playerAudio.PlayOneShot(swordSwing[2]); break;
        }
    }

    public void PlaySlashSound()
    {
        playerAudio.volume = slashVolume;
        playerAudio.PlayOneShot(slash);
    }
}
