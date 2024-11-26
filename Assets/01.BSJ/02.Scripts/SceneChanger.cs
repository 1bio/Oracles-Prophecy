using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string SceneName;

    public void ChangeScene()
    {
        SceneController.instance.LoadScene(SceneName);
    }
}
