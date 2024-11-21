using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemUIController : MonoBehaviour
{
    [SerializeField] private GameObject _itemNearbyMessage = null;
    private GameObject _itemPickupMessage;
    private ItemObjectController _itemObjectController;
    [SerializeField] private GameObject _itemPickupPrefab;

    public static List<GameObject> PickupMessageObj = new List<GameObject>();

    private void OnEnable()
    {
        _itemPickupMessage = GameObject.Find("PickupMessage");
        _itemObjectController = GetComponent<ItemObjectController>();
        _itemObjectController.ItemGetEvent += FlotingItemPickupMessage;
    }

    private void OnDisable()
    {
        _itemObjectController.ItemGetEvent -= FlotingItemPickupMessage;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GameLayers.Player.ToString()) && _itemNearbyMessage != null)
        {
            _itemNearbyMessage.SetActive(true);
            _itemNearbyMessage.transform.rotation = Quaternion.Euler(30, 45, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GameLayers.Player.ToString()) && _itemNearbyMessage != null)
        {
            _itemNearbyMessage.SetActive(false); 
        }
    }

    private void FlotingItemPickupMessage(string itemName)
    {
        GameObject pickupItemNameObj = null;
        foreach (GameObject obj in PickupMessageObj)
        {
            if (!obj.activeSelf)
            {
                pickupItemNameObj = obj;
                obj.SetActive(true);
                break;
            }
        }

        if (pickupItemNameObj == null)
        {
            pickupItemNameObj = GameObject.Instantiate(_itemPickupPrefab);
            PickupMessageObj.Add(pickupItemNameObj);
        }

        TextMeshProUGUI text = pickupItemNameObj.GetComponent<TextMeshProUGUI>();

        text.SetText(itemName);
        Transform parentTransform = _itemPickupMessage?.transform;
        pickupItemNameObj.transform.SetParent(parentTransform);
        pickupItemNameObj.transform.localScale = Vector3.one;
    }
}
