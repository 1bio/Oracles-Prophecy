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


    // ȭ���
    public void VFX_SkyFall()
    {
        if (!s_prefabs[0].GetComponent<ParticleSystem>().isPlaying || !s_prefabs[1].GetComponent<ParticleSystem>().isPlaying)
        {
            s_prefabs[0].GetComponent<ParticleSystem>().Stop();
            s_prefabs[1].GetComponent<ParticleSystem>().Stop();
        }

        // ��ǥ�� ������ ���
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

    // ������
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

    // ����
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
        // ȭ���
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

                // ������ ó��
                float damage = DataManager.instance.playerData.skillData[2].damage;
                for (int i = 0; i < hitCount; i++)
                {
                    yield return new WaitForSeconds(0.2f);
                    Targeting.CurrentTarget.GetComponent<Health>()?.TakeDamage(damage, false);
                }
            }
        }
        
        // ������
        if(EffectNumber == 1)
        {
            yield return null;

            c_prefabs[EffectNumber].transform.parent = null;
            c_prefabs[EffectNumber].transform.position = ShootingTransform.transform.position;
            c_prefabs[EffectNumber].transform.rotation = ShootingTransform.transform.rotation;
            c_prefabs[EffectNumber].GetComponent<ParticleSystem>().Play();
        }

        // ����
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
