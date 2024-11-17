using System.Collections;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [Header("Start VFX")]
    [SerializeField] private GameObject[] s_prefabs; 

    [Header("Continuous VFX")]
    [SerializeField] private GameObject[] c_prefabs;

    [Header("Setting")]
    [SerializeField] private Targeting Targeting;
    [SerializeField] private Transform ShootingTransform;

    private int hitCount = 5;


    // 화살비
    public void VFX_SkyFall()
    {
        if (!s_prefabs[0].GetComponent<ParticleSystem>().isPlaying || !s_prefabs[1].GetComponent<ParticleSystem>().isPlaying)
        {
            s_prefabs[0].GetComponent<ParticleSystem>().Stop();
            s_prefabs[1].GetComponent<ParticleSystem>().Stop();
        }

        // 목표물 없으면 취소
        if (Targeting.CurrentTarget == null)
            return;

        s_prefabs[0].GetComponent<ParticleSystem>().Play();
        s_prefabs[1].GetComponent<ParticleSystem>().Play();

        if (s_prefabs[1].GetComponent<AudioSource>() != null)
        {
            AudioSource soundComponentCast = s_prefabs[1].GetComponent<AudioSource>();
            AudioClip clip = soundComponentCast.clip;
            soundComponentCast.PlayOneShot(clip);

            StartCoroutine(Attack(0));
        }
    }

    // 정조준
    public void VFX_AimingShot()
    {
        if (!s_prefabs[2].GetComponent<ParticleSystem>().isPlaying)
        {
            s_prefabs[2].GetComponent<ParticleSystem>().Stop();
        }

        s_prefabs[2].GetComponent<ParticleSystem>().Play();

        if (s_prefabs[2].GetComponent<AudioSource>() != null)
        {
            AudioSource soundComponentCast = s_prefabs[2].GetComponent<AudioSource>();
            AudioClip clip = soundComponentCast.clip;
            soundComponentCast.PlayOneShot(clip);

            StartCoroutine(Attack(1));
        }
    }

    // 빙결
    public void VFX_Frost()
    {
        if (!s_prefabs[3].GetComponent<ParticleSystem>().isPlaying)
        {
            s_prefabs[3].GetComponent<ParticleSystem>().Stop();
        }

        s_prefabs[3].transform.position = transform.position;
        s_prefabs[3].transform.SetParent(null);
        //s_prefabs[3].GetComponent<ParticleSystem>().Play();

        s_prefabs[3].SetActive(true);

        if (s_prefabs[3].GetComponent<AudioSource>() != null)
        {
            AudioSource soundComponentCast = s_prefabs[3].GetComponent<AudioSource>();
            AudioClip clip = soundComponentCast.clip;
            soundComponentCast.PlayOneShot(clip);

            StartCoroutine(Attack(2));
        }
    }
    
    IEnumerator Attack(int EffectNumber)
    {
        // 화살비
        if(EffectNumber == 0)
        {
            yield return new WaitForSeconds(1.2f);

            c_prefabs[0].transform.parent = null;
            c_prefabs[0].transform.position = Targeting.CurrentTarget.gameObject.transform.position;
            c_prefabs[0].GetComponent<ParticleSystem>().Play();

            if (c_prefabs[0].GetComponent<AudioSource>() != null)
            {
                AudioSource soundComponentCast = c_prefabs[0].GetComponent<AudioSource>();
                AudioClip clip = soundComponentCast.clip;
                soundComponentCast.PlayOneShot(clip);

                // 데미지 처리
                float damage = DataManager.instance.playerData.skillData[2].damage;
                for (int i = 0; i < hitCount; i++)
                {
                    yield return new WaitForSeconds(0.2f);
                    Targeting.CurrentTarget.GetComponent<Health>()?.TakeDamage(damage, false);
                }
            }
        }
        
        // 정조준
        if(EffectNumber == 1)
        {
            yield return null;

            c_prefabs[EffectNumber].transform.parent = null;
            c_prefabs[EffectNumber].transform.position = ShootingTransform.transform.position;
            c_prefabs[EffectNumber].transform.rotation = ShootingTransform.transform.rotation;
            c_prefabs[EffectNumber].GetComponent<ParticleSystem>().Play();
        }

        // 빙결
        if(EffectNumber == 2)
        {
            yield return new WaitForSeconds(2.5f);

            if (s_prefabs[3].GetComponent<ParticleSystem>().isPlaying == false)
            {
                s_prefabs[3].SetActive(false);
            }
        }
    }

}
