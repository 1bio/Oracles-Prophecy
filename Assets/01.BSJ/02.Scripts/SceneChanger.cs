using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string SceneName;
    [SerializeField] private Player player;

    public void ChangeScene()
    {
        DataManager.instance.Initialized();
        player.equipment.Clear();
        player.inventory.Clear();
        SceneController.instance.LoadScene(SceneName);
    }
}
