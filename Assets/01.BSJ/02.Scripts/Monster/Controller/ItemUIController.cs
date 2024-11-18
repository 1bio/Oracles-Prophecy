using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemUIController : MonoBehaviour
{
    [SerializeField] private GameObject _itemNearbyMessage = null;
    private GameObject _itemPickupMessage;

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

    private void OnDisable()
    {
        
    }
}
