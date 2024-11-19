using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    [field: Header("인벤토리 설정")]
    public InventoryObject inventory;
    public InventoryObject equipment;

    [field: Header("플레이어 스탯")]
    public Attribute[] attributes;

    private Transform boots;
    //private Transform chest;
    //private Transform helmet;
    private Transform offhand;
    private Transform sword;

    // 착용 장비
    private GameObject helmet;
    private GameObject chest;

    private GameObject L_gloves;
    private GameObject R_gloves;

    private GameObject L_boot;
    private GameObject R_boot;

    private GameObject L_shoulder;
    private GameObject R_shoulder;

    // 프리팹 스킨 매쉬
    private SkinnedMeshRenderer helmetMesh;
    private SkinnedMeshRenderer chestMesh;

    private SkinnedMeshRenderer L_glovesMesh;
    private SkinnedMeshRenderer R_glovesMesh;
    
    private SkinnedMeshRenderer L_bootMesh;
    private SkinnedMeshRenderer R_bootMesh;

    private SkinnedMeshRenderer L_sholuderMesh;
    private SkinnedMeshRenderer R_sholuderMesh;

    [field: Header("무기 장착 위치")]
    [SerializeField ] private Transform boneRoot;

    public Transform weaponTransform;
    public Transform offhandWristTransform;
    public Transform offhandHandTransform;

    private void Start()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            equipment.GetSlots[i].OnAfterUpdate += OnAddItem;
        }
    }

    void AttachEquipmentToCharacter(Transform characterBoneRoot, SkinnedMeshRenderer equipmentRenderer)
    {
        Transform[] characterBones = characterBoneRoot.GetComponentsInChildren<Transform>(); 
        Transform[] updatedBones = new Transform[equipmentRenderer.bones.Length];

        for (int i = 0; i < equipmentRenderer.bones.Length; i++)
        {
            string boneName = equipmentRenderer.bones[i].name;

            foreach (Transform characterBone in characterBones)
            {
                if (characterBone.name == boneName)
                {
                    updatedBones[i] = characterBone;
                    break;
                }
            }
        }

        equipmentRenderer.bones = updatedBones;

        equipmentRenderer.rootBone = characterBoneRoot;
    }


    public void OnRemoveItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    }
                }

                if (_slot.ItemObject.characterDisplay != null)
                {
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.Helmet:
                            Destroy(helmet);
                            break;

                        case ItemType.Chest:
                            Destroy(chest);
                            break;

                        case ItemType.Gloves:
                            Destroy(L_gloves);
                            Destroy(R_gloves);
                            break;

                        case ItemType.Boots:
                            Destroy(L_boot);
                            Destroy(R_boot);
                            break;

                        case ItemType.Shoulder:
                            Destroy(L_shoulder);
                            Destroy(R_shoulder);
                            break;

                        case ItemType.Weapon:
                            Destroy(sword.gameObject);
                            break;
                        case ItemType.Shield:
                            Destroy(offhand.gameObject);
                            break;
                    }
                }

                break;
            case InterfaceType.Box:
                break;
            default:
                break;
        }
    }
    public void OnAddItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;

        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }

                if (_slot.ItemObject.characterDisplay != null)
                {
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.Helmet:
                            helmet = Instantiate(_slot.ItemObject.characterDisplay[0]);
                            helmetMesh = GetEquipmentSkinnedMeshRenderer(helmet);
                            AttachEquipmentToCharacter(boneRoot, helmetMesh);
                            break;

                        case ItemType.Chest:
                            chest = Instantiate(_slot.ItemObject.characterDisplay[0]);
                            chestMesh = GetEquipmentSkinnedMeshRenderer(chest);
                            AttachEquipmentToCharacter(boneRoot, chestMesh);
                            break;

                        case ItemType.Gloves:
                            L_gloves = Instantiate(_slot.ItemObject.characterDisplay[0]);
                            L_glovesMesh = GetEquipmentSkinnedMeshRenderer(L_gloves);   
                            AttachEquipmentToCharacter(boneRoot, L_glovesMesh);

                            R_gloves = Instantiate(_slot.ItemObject.characterDisplay[1]);
                            R_glovesMesh = GetEquipmentSkinnedMeshRenderer(R_gloves);
                            AttachEquipmentToCharacter(boneRoot, R_glovesMesh);
                            break;

                        case ItemType.Boots:
                            L_boot = Instantiate(_slot.ItemObject.characterDisplay[0]);
                            L_bootMesh = GetEquipmentSkinnedMeshRenderer(L_boot);
                            AttachEquipmentToCharacter(boneRoot, L_bootMesh);

                            R_boot = Instantiate(_slot.ItemObject.characterDisplay[1]);
                            R_bootMesh = GetEquipmentSkinnedMeshRenderer(R_boot);
                            AttachEquipmentToCharacter(boneRoot, R_bootMesh);
                            break;

                        case ItemType.Shoulder:
                            L_shoulder = Instantiate(_slot.ItemObject.characterDisplay[0]);
                            L_sholuderMesh = GetEquipmentSkinnedMeshRenderer(L_shoulder);
                            AttachEquipmentToCharacter(boneRoot, L_sholuderMesh);

                            R_shoulder = Instantiate(_slot.ItemObject.characterDisplay[1]);
                            R_sholuderMesh = GetEquipmentSkinnedMeshRenderer(R_shoulder);
                            AttachEquipmentToCharacter(boneRoot, R_sholuderMesh);
                            break;

                        case ItemType.Weapon:
                            sword = Instantiate(_slot.ItemObject.characterDisplay[0], weaponTransform).transform;
                            print(string.Concat("장착"));
                            break;

                        case ItemType.Shield:
                            switch (_slot.ItemObject.type)
                            {
                                case ItemType.Weapon:
                                    offhand = Instantiate(_slot.ItemObject.characterDisplay[0], offhandHandTransform)
                                        .transform;
                                    break;
                                case ItemType.Shield:
                                    offhand = Instantiate(_slot.ItemObject.characterDisplay[0], offhandWristTransform)
                                        .transform;
                                    break;
                            }
                            break;
                    }
                }

                break;
            case InterfaceType.Box:
                break;
            default:
                break;
        }
    }

    public SkinnedMeshRenderer GetEquipmentSkinnedMeshRenderer(GameObject equipment)
    {
        if (equipment.GetComponent<SkinnedMeshRenderer>()) // 최상위 오브젝트에 존재 할 경우
        {
            return equipment.GetComponent<SkinnedMeshRenderer>();
        }
        else
        {
            return equipment.GetComponentInChildren<SkinnedMeshRenderer>(); // 하위 오브젝트에 존재 할 경우
        }
    }


    public void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            var Itemobject = other.GetComponent<ItemObjectController>();
            if (Itemobject != null)
            {
                foreach (var item in Itemobject.GetDropItems())
                {
                    Item _item = new Item(item);
                    if (inventory.AddItem(_item, 1))
                    {
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
            equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
            equipment.Load();
        }
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }


    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public Player parent;
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(Player _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }
    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}