using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    public GameObject CharacterPanel;
    bool activeCharacterPanel = false;
    void Start()
    {
        CharacterPanel.SetActive(activeCharacterPanel);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            activeCharacterPanel = !activeCharacterPanel;
            CharacterPanel.SetActive(activeCharacterPanel);
        }
    }
}
