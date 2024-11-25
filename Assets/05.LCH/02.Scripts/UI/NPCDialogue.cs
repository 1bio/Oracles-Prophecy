using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float scrollSpeed;

    private void Start()
    {
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private void Update()
    {
        if(scrollRect.verticalNormalizedPosition > 0f)
        {
            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;
        }        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.R))
        {
            scrollRect.verticalNormalizedPosition = 1f;
            dialogue.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        dialogue.SetActive(false);
    }
}
