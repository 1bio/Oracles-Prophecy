//using MasterRealisticFX;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public enum MonsterStateType
{
    Spawn,
    Idle,
    Attack,
    Death,
    Walk,
    GotHit,
    Skill,
    Null
}

public class Monster : MonoBehaviour
{
    [Header(" # Stat Data")]
    [SerializeField] protected MonsterStatData p_monsterStatData;

    [Header(" # Skill Data")]
    [SerializeField] protected MonsterSkillData[] p_monsterSkillDatas;

    [Header(" # Loot Item Data")]
    [SerializeField] protected MonsterLootItemData p_monsterLootItemData;

    private static Transform VFXContainerTransform;
    private string _vfxContainerName = "Monster VFX Container";
    private TrailRenderer _objectTrail;

    public MonsterStateType MonsterStateType { get; set; }
    public MonsterStateMachineController StateMachineController { get; private set; }
    public MonsterSkillController SkillController { get; private set; }
    public MonsterLootItemController LootItemController { get; private set; }
    public MonsterMovementController MovementController { get; protected set; }
    public MonsterAnimationController AnimationController { get; protected set; }
    public MonsterCombatController CombatController { get; protected set; }
    public MonsterParticleController ParticleController { get; protected set; }
    public TrailRenderer ObjectTrail { get => _objectTrail; }

    // 넉백 처리
    public CharacterController Controller { get; protected set; }
    public ForceReceiver ForceReceiver { get; protected set; }

    void FindOrCreateVFXContainer()
    {
        if (VFXContainerTransform == null)
        {
            GameObject vfxContainer = GameObject.Find(_vfxContainerName);
            if (vfxContainer == null)
            {
                vfxContainer = new GameObject(_vfxContainerName);
            }
            VFXContainerTransform = vfxContainer.transform;
        }
    }

    protected virtual void Awake()
    {
        FindOrCreateVFXContainer();

        _objectTrail = GetComponentInChildren<TrailRenderer>();
        if (_objectTrail != null)
        {
            _objectTrail.gameObject.SetActive(false);
        }

        StateMachineController = GetComponent<MonsterStateMachineController>();
        SkillController = new MonsterSkillController(p_monsterSkillDatas);
        LootItemController = new MonsterLootItemController(p_monsterLootItemData);
        MovementController = new MonsterMovementController(GetComponent<TargetDetector>(), GetComponent<Astar>(), FindObjectOfType<PointGrid>(), GetComponent<CharacterController>());
        AnimationController = new MonsterAnimationController(GetComponent<Animator>(), GetComponent<ObjectFadeInOut>(),100f);
        CombatController = new MonsterCombatController(p_monsterStatData, GetComponent<Health>());
     
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();

        if (p_monsterSkillDatas.Length > 0)
        {
            ParticleController = new MonsterParticleController(p_monsterSkillDatas, VFXContainerTransform, this);
        }
    }

    /*protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (MonsterSkillController.GetAvailableSkills().Count > 0
                && MonsterSkillController.UpdateCurrentSkillData() != null)
            {
                MonsterStateMachineController.OnSkill();
            }
        }
    }*/


    protected void LateUpdate()
    {
        if (MonsterStateType != MonsterStateType.Skill && ParticleController != null)
        {
            foreach (var vfxs in ParticleController.VFX)
            {
                List<ParticleSystem> vfxToRemove = new List<ParticleSystem>();

                foreach (var vfx in vfxs.Value)
                {
                    if (!vfx.isPlaying)
                    {
                        vfx.Stop();
                        vfx.Clear();
                        vfx.time = 0;

                        if (vfx.time == 0)
                        {
                            vfxToRemove.Add(vfx);
                        }
                    }
                }

                if (vfxs.Value.Count >= 10)
                {
                    foreach (var vfx in vfxToRemove)
                    {
                        vfxs.Value.Remove(vfx);
                        Destroy(vfx.gameObject);

                        if (vfxs.Value.Count < 10)
                            break;
                    }
                }
            }
        }
    }


    // Animation Event
    public void EnableWeapon()
    {
        CombatController.MonsterCombatAbility.MonsterAttack.IsEnableWeapon = true;
    }

    public void DisableWeapon()
    {
        CombatController.MonsterCombatAbility.MonsterAttack.IsEnableWeapon = false;
    }

    public void UnlockAnimationTransition()
    {
        AnimationController.IsLockedInAnimation = false;
    }

    public void RePlayVFX(string vfxNameWithScale)
    {
        if (vfxNameWithScale.Contains('_'))
        {
            string[] parts = vfxNameWithScale.Split('_');
            string vfxName = parts[0];
            float scaleFactor = float.Parse(parts[1]);

            ParticleController.RePlayVFX(vfxName, scaleFactor);
        }
        else
        {
            ParticleController.RePlayVFX(vfxNameWithScale);
        }
    }
}