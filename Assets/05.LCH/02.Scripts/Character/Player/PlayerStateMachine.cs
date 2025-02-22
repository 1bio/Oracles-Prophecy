﻿using MasterRealisticFX;
using MedievalKingdomUI.Scripts.Window;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: Header("클래스")]
    [field: SerializeField] public Player Player { get; private set; }

    [field: SerializeField] public InputReader InputReader { get; private set; }

    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField] public Health Health { get; private set; }

    [field: SerializeField] public MeleeComponenet MeleeComponenet { get; private set; }

    [field: SerializeField] public WeaponToggle WeaponToggle { get; private set; }

    [field: SerializeField] public RangeComponent RangeComponent { get; private set; }

    [field: SerializeField] public Targeting Targeting { get; private set; }

    [field: SerializeField] public CameraShake CameraShake { get; private set; }

    [field: SerializeField] public VFXController VFXController { get; private set; }
    
    [field: SerializeField] public SimpleWeaponTrail WeaponTrail { get; private set; }

    [field: SerializeField] public ParticleEventHandler ParticleEventHandler { get; private set; }

    [field: SerializeField] public PlayerAudio PlayerAudio { get; private set; }


    [field: Header("컴포넌트")]
    [field: SerializeField] public Animator Animator { get; private set; }

    [field: SerializeField] public CharacterController Controller { get; private set; }

    [field: SerializeField] public Transform CameraTransform { get; private set; }

    //[field: SerializeField] public GameObject[] WeaponPrefabs { get; private set; }


    [field: Header("플레이어 세팅")]
    [field: SerializeField] public float RotationSpeed { get; private set; }

    [field: SerializeField] public float DodgeDuration { get; private set; }

    [field: SerializeField] public float DodgeLenght { get; private set; }

    [field: SerializeField] public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;

    [field: SerializeField] public float DodgeCooldown { get; private set; }


    [field: Header("기본 무기")]
    [field: SerializeField] public ItemObject[] BaseWeapon { get; private set; }


    private void OnEnable()
    {
        Health.ImpactEvent += OnHandleTakeDamage;
        Health.DeadEvent += OnHandleDie;
    }

    private void OnDisable()
    {
        Health.ImpactEvent -= OnHandleTakeDamage;
        Health.DeadEvent -= OnHandleDie;
    }

    private void Start()
    {
        SetPlayerClass();

        DataManager.instance.SaveData();
    }


    #region Main Methods
    public void SetDodgeTime(float dodgeTime)
    {
        PreviousDodgeTime = dodgeTime;
    }

    // 플레이어 클래스 설정
    public void SetPlayerClass()
    {
        // 기본 상태 전환
        switch (AnimatedWindowController.choiceIndex)
        {
            case 0: // 전사
                UIManager.instance.SetIsMelee(true);
                Item sword = new Item(BaseWeapon[0]);
                Player.inventory.AddItem(sword, 1);
                ChangeState(new PlayerFreeLookState(this));
                Debug.Log("warrior");
                break;

            case 1: // 궁수
                UIManager.instance.SetIsMelee(false);
                Item bow = new Item(BaseWeapon[1]);
                Player.inventory.AddItem(bow, 1);
                ChangeState(new PlayerRangeFreeLookState(this));
                Debug.Log("archer");
                break;
        }
    }

    // 스킬 애니메이션 이벤트
    public void CoolDownEvent(string skillName)
    {
        SkillManager.instance.StartCooldown(skillName);
    }
    #endregion

    #region Event Methods
    // Impact
    public void OnHandleTakeDamage()
    {
        Health.SetHealth(DataManager.instance.playerData.statusData.currentHealth);

        if (Health.hitCount > 3)
        {
            ChangeState(new PlayerImpactState(this));
            Health.hitCount = 0;
        }
    }

    // Dead
    public void OnHandleDie()
    {
        ChangeState(new PlayerDeadState(this));

        Player.equipment.Clear();
        Player.inventory.Clear();

        DataManager.instance.Initialized();

        UIManager.instance.GameoverUI();
    }
    #endregion

}
