using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    private bool isChanging = false;

    private void Awake()
    {
        isChanging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GameLayers.Player.ToString()) && !isChanging)
        {
            SceneController.instance.LoadScene("Abandoned Prison");
            isChanging = true;
        }
    }
}
