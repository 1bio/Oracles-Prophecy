using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

public abstract class UserInterface : MonoBehaviour
{
    public Sprite emptySlot;
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    private void Awake()
    {
        CreateSlots();

        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    //void Start()
    //{
    //    CreateSlots();

    //    for (int i = 0; i < inventory.GetSlots.Length; i++)
    //    {
    //        inventory.GetSlots[i].parent = this;
    //        inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
    //    }
    //    AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
    //    AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    //}

    private void OnSlotUpdate(InventorySlot _slot)
    {
        // null 체크: 슬롯, 아이템, 아이템 오브젝트가 모두 유효한지 확인
        if (_slot == null || _slot.item == null || _slot.ItemObject == null || _slot.slotDisplay == null)
        {
            Debug.LogWarning("Slot, item, ItemObject, or slotDisplay is null.");
            return;
        }

        // 아이템 ID가 유효한 경우 (ID >= 0)
        if (_slot.item.Id >= 0)
        {
            var image = _slot.slotDisplay.transform.GetChild(0)?.GetComponentInChildren<Image>();
            if (image != null)
            {
                image.sprite = _slot.ItemObject.uiDisplay;
                image.color = new Color(1, 1, 1, 1);
            }

            var text = _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
            }
        }
        // 아이템 ID가 유효하지 않은 경우 (ID < 0)
        else
        {
            var image = _slot.slotDisplay.transform.GetChild(0)?.GetComponentInChildren<Image>();
            if (image != null)
            {
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0);
            }

            var text = _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = "";
            }
        }
    }


    void Update()
    {
        slotsOnInterface.UpdateSlotDisplay();
    }

    public abstract void CreateSlots();


    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }

    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }

        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData); // 두 개의 아이템 슬롯 교환
        }
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 100);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }
}

public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}


public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface)
        {
            if (_slot.Key != null && _slot.Value != null)
            {
                if (_slot.Value.item != null && _slot.Value.item.Id >= 0)
                {
                    // 이미지 설정
                    var image = _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>();
                    if (image != null)
                    {
                        image.sprite = _slot.Value.ItemObject.uiDisplay;
                        image.color = new Color(1, 1, 1, 1);
                    }

                    // 텍스트 설정
                    var text = _slot.Key.GetComponentInChildren<TextMeshProUGUI>();
                    if (text != null)
                    {
                        text.text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
                    }
                }
                else
                {
                    // 이미지 초기화
                    var image = _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>();
                    if (image != null)
                    {
                        image.sprite = null;
                        image.color = new Color(1, 1, 1, 0);
                    }

                    // 텍스트 초기화
                    var text = _slot.Key.GetComponentInChildren<TextMeshProUGUI>();
                    if (text != null)
                    {
                        text.text = "";
                    }
                }
            }
        }
    }
}

