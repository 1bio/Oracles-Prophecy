using System.Collections;
using System.Collections.Generic;
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
    private Transform chest;
    private Transform helmet;
    private Transform offhand;
    private Transform sword;

    [field: Header("아이템 모델(임시)")]
    public GameObject helmetPrefab;
    public GameObject chestPrefab;

    [field: Header("무기 장착 위치")]
    public Transform boneRoot; // 캐릭터 boneRoot
    public Transform weaponTransform;
    public Transform offhandWristTransform;
    public Transform offhandHandTransform;
    //public Transform helmetTransform;

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
        Transform[] characterBones = characterBoneRoot.GetComponentsInChildren<Transform>(); // 캐릭터 boneRoot 하위 객체 가져오기
        Transform[] updatedBones = new Transform[equipmentRenderer.bones.Length]; // 장비의 스킨 매쉬 렌더러의 개수 만큼 

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
                            helmetPrefab.SetActive(false);
                            break;
                        case ItemType.Chest:
                            chestPrefab.SetActive(false);
                            break;
                        case ItemType.Weapon:
                            Destroy(sword.gameObject);
                            break;
                        case ItemType.Shield:
                            Destroy(offhand.gameObject);
                            break;
                        case ItemType.Boots:
                            Destroy(boots.gameObject);
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
                            chestPrefab.SetActive(true);
                            break;
                        case ItemType.Chest:
                            chestPrefab.SetActive(true);
                            break;
                        case ItemType.Weapon:
                            sword = Instantiate(_slot.ItemObject.characterDisplay, weaponTransform).transform;
                            print(string.Concat("장착"));
                            break;
                        case ItemType.Shield:
                            switch (_slot.ItemObject.type)
                            {
                                case ItemType.Weapon:
                                    offhand = Instantiate(_slot.ItemObject.characterDisplay, offhandHandTransform)
                                        .transform;
                                    break;
                                case ItemType.Shield:
                                    offhand = Instantiate(_slot.ItemObject.characterDisplay, offhandWristTransform)
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


    public void OnTriggerEnter(Collider other)
    {
        var groundItem = other.GetComponent<GroundItem>();
        if (groundItem)
        {
            Item _item = new Item(groundItem.item);
            if (inventory.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
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