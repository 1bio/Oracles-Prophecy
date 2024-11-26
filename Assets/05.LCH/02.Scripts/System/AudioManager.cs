using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Fire Sword Swing")]
    [SerializeField] private AudioClip[] fireSwordSwing;
    [Range(0f, 1f)][SerializeField] private float fireSwordSwingVolume;

    [Header("Swing Hit")]
    [SerializeField] private AudioClip[] swingHit;
    [Range(0f, 1f)][SerializeField] private float swingHitVolume;
    private int count;

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

        switch (SceneManager.GetActiveScene().name)
        {
            case "Abandoned Prison":
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

            case "Village":
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

    public void PlayFireSwordSwingSound(int index)
    {
        playerAudio.volume = fireSwordSwingVolume;

        switch (index)
        {
            case 0: playerAudio.PlayOneShot(fireSwordSwing[0]); break;
            case 1: playerAudio.PlayOneShot(fireSwordSwing[1]); break;
            case 2: playerAudio.PlayOneShot(fireSwordSwing[2]); break;
        }
    }

    public void PlaySwingHitSound()
    {
        playerAudio.volume = swingHitVolume;
        playerAudio.PlayOneShot(swingHit[count]);   

        if(count >= 2)
        {
            count = 0;
            return;
        }

        count++;
    }

    public void PlaySlashSound()
    {
        playerAudio.volume = slashVolume;
        playerAudio.PlayOneShot(slash);
    }
}
