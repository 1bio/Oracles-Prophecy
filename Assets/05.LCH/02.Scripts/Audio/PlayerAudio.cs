using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public Animator animator;

    [Header("FootStep")]
    [SerializeField] public AudioClip[] footStep;
    private bool isLeftFootStep = true;

    [Range(0f, 1f)]
    [SerializeField] private float footStepVolume;

    public void FootStep()
    {
        audioSource.volume = footStepVolume;

        if (animator.GetFloat("Velocity") > 0.8f || animator.GetFloat("RangeVelocity") > 0.8f)
        {
            if (isLeftFootStep)
            {
                audioSource.PlayOneShot(footStep[0]);
            }
            else
            {
                audioSource.PlayOneShot(footStep[1]);
            }

            isLeftFootStep = !isLeftFootStep;
        }
    }
}
