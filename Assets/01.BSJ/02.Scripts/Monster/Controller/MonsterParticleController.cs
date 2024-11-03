using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class MonsterParticleController
{
    private MonsterSkillData[] _skillDatas;
    private Transform _parentTransform;
    private Monster _monster;

    public MonsterParticleController(MonsterSkillData[] skillDatas, Transform parentTransform, Monster monster)
    {
        _skillDatas = skillDatas;
        VFX = new Dictionary<string, List<ParticleSystem>>();
        InstantiateVFX(parentTransform, monster);
        _parentTransform = parentTransform;
        _monster = monster;
    }

    public Dictionary<string, List<ParticleSystem>> VFX { get; private set; }
    public Dictionary<string, Transform> VFXTransform { get; private set; } = new Dictionary<string, Transform>();
    public ParticleSystem CurrentParticleSystem { get; set; }

    public void InstantiateVFX(Transform parentTransform, Monster monster)
    {
        for (int i = 0; i < _skillDatas.Length; i++)
        {
            for (int j = 0; j < _skillDatas[i].VFX.Length; j++)
            {
                GameObject vfxPrefab = _skillDatas[i].VFX[j];

                if (!parentTransform.Find(vfxPrefab.name))
                {
                    GameObject vfxInstance = GameObject.Instantiate(vfxPrefab, parentTransform);
                    vfxInstance.transform.localPosition = Vector3.zero;

                    ParticleSystem particleSystem = vfxInstance.GetComponent<ParticleSystem>();
                    MonsterParticleDamageHandler damageHandler = particleSystem.GetComponentInChildren<MonsterParticleDamageHandler>();
                    damageHandler?.SetMonster(monster);

                    particleSystem.Stop();
                    particleSystem.Clear();

                    if (!VFX.ContainsKey(vfxPrefab.name))
                    {
                        VFX[vfxPrefab.name] = new List<ParticleSystem>();
                    }
                    VFX[vfxPrefab.name].Add(particleSystem);
                }
            }
        }
    }

    public void RePlayVFX(string vfxName)
    {
        int index = vfxName.IndexOf('(');

        if (index != -1)
        {
            vfxName = vfxName.Substring(0, index);
        }

        if (VFX.ContainsKey(vfxName))
        {
            CurrentParticleSystem = GetAvailableParticle(vfxName);

            CurrentParticleSystem.Stop();
            CurrentParticleSystem.Clear();
            CurrentParticleSystem.Play();
        }
    }

    public void RePlayVFX(string vfxName, float scaleFactor)
    {
        int index = vfxName.IndexOf('(');

        if (index != -1)
        {
            vfxName = vfxName.Substring(0, index);
        }

        if (VFX.ContainsKey(vfxName))
        {
            CurrentParticleSystem = GetAvailableParticle(vfxName);

            CurrentParticleSystem.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            CurrentParticleSystem.Stop();
            CurrentParticleSystem.Clear();
            CurrentParticleSystem.Play();
        }
    }

    public void RePlayVFX(string vfxName, float _fanAngle, float vfxCount)
    {
        if (VFX.ContainsKey(vfxName))
        {
            for (int i = 0; i < vfxCount; i++)
            {
                float angle = -_fanAngle / 2 + i * (_fanAngle / (vfxCount - 1));
                Vector3 direction = Quaternion.Euler(0, angle, 0) * _monster.transform.forward;

                ParticleSystem particleSystem = GetAvailableParticle(vfxName);

                if (particleSystem != null)
                {
                    particleSystem.Stop();
                    particleSystem.Clear();

                    particleSystem.transform.position = _monster.transform.position + new Vector3(0, 0.5f, 0) + direction;
                    particleSystem.transform.rotation = Quaternion.LookRotation(direction);

                    particleSystem.Play();

                    _monster.StartCoroutine(DelayTime());
                }
            }
        }
    }

    private IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(0.1f);
    }

    public void AllClearVFXs()  
    {
        foreach (KeyValuePair<string, List<ParticleSystem>> vfxs in VFX)
        {
            foreach (ParticleSystem vfx in VFX[vfxs.Key])
            {
                vfx.Stop();
                vfx.Clear();
                vfx.time = 0;
            }
        }
        CurrentParticleSystem = null;
    }

    public ParticleSystem GetAvailableParticle(string vfxName)
    {
        if (VFX.ContainsKey(vfxName))
        {
            ParticleSystem currentParticle = new ParticleSystem();
            for (int i = 0; i < VFX[vfxName].Count; i++)
            {
                if (!VFX[vfxName][i].isPlaying)
                {
                    currentParticle = VFX[vfxName][i];

                    return VFX[vfxName][i];
                }
            }

            if (currentParticle == null)
            {
                GameObject vfxInstance = GameObject.Instantiate(VFX[vfxName][0].gameObject, _parentTransform);
                vfxInstance.transform.position = VFXTransform.ContainsKey(vfxName) ? VFXTransform[vfxName].position : Vector3.zero;
                vfxInstance.transform.rotation = VFXTransform.ContainsKey(vfxName) ? VFXTransform[vfxName].rotation : Quaternion.identity;
                
                ParticleSystem particleSystem = vfxInstance.GetComponent<ParticleSystem>();
                MonsterParticleDamageHandler damageHandler = particleSystem.GetComponentInChildren<MonsterParticleDamageHandler>();
                damageHandler?.SetMonster(_monster);

                particleSystem.Stop();
                particleSystem.Clear();

                VFX[vfxName].Add(particleSystem);

                return particleSystem;
            }
        }
        return null;
    }
}
