using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [field: Header("플레이어")]
    public InventoryObject inventory;
    public InventoryObject equipment;

    [field: Header("장비 장착 위치")]
    public Transform boneRoot;
    public Transform swordTransform;
    public Transform bowTransform;
    public GameObject playerHair;

    [field: Header("속성")]
    public Attribute[] attributes;

    // 장착할 장비
    private GameObject helmet;
    private GameObject chest;

    private GameObject L_gloves;
    private GameObject R_gloves;

    private GameObject L_boot;
    private GameObject R_boot;

    private GameObject L_shoulder;
    private GameObject R_shoulder;

    private GameObject sword;
    private GameObject bow;

    // 장비 매쉬 렌더러
    private SkinnedMeshRenderer helmetMesh;
    private SkinnedMeshRenderer chestMesh;

    private SkinnedMeshRenderer L_glovesMesh;
    private SkinnedMeshRenderer R_glovesMesh;
    
    private SkinnedMeshRenderer L_bootMesh;
    private SkinnedMeshRenderer R_bootMesh;

    private SkinnedMeshRenderer L_sholuderMesh;
    private SkinnedMeshRenderer R_sholuderMesh;

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
        if (_slot.ItemObject == null) // Ensure the item object is not null before proceeding
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
                            playerHair.SetActive(true);
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
                            L_boot = null;
                            R_boot = null;
                            break;

                        case ItemType.Shoulder:
                            Destroy(L_shoulder);
                            Destroy(R_shoulder);
                            break;

                        case ItemType.Weapon:
                            Destroy(sword);
                            Destroy(bow);
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
                            playerHair.SetActive(false);
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
                            switch (ClassSelectWindow.classIndex)
                            {
                                case 0:
                                    sword = Instantiate(_slot.ItemObject.characterDisplay[0], swordTransform);
                                    sword.transform.SetParent(swordTransform);
                                    break;

                                case 1:
                                    bow = Instantiate(_slot.ItemObject.characterDisplay[0], bowTransform);
                                    bow.transform.SetParent(bowTransform);
                                    break;
                            }
                            break;

                            /*case ItemType.Shield:
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
                                break;*/
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
        if (equipment.GetComponent<SkinnedMeshRenderer>()) // 최상위 오브젝트 탐색
        {
            return equipment.GetComponent<SkinnedMeshRenderer>();
        }
        else
        {
            return equipment.GetComponentInChildren<SkinnedMeshRenderer>(); // 하위 오브젝트 탐색
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

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ItemObjectController itemObjectController = other.gameObject.GetComponent<ItemObjectController>();

            if (itemObjectController == null) return;

            List<ItemObject> itemObjects = itemObjectController.GetDropItems();
            foreach (ItemObject itemobj in itemObjects)
            {
                Item _item = new Item(itemobj);
                inventory.AddItem(_item, 1);
            }

            Destroy(other.gameObject);
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