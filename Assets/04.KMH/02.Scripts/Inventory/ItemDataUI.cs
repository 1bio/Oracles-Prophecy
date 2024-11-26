using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDataUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject uiPrefab;
    private GameObject instantiatedUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiPrefab != null && instantiatedUI == null)
        {
            // UI ����
            Debug.Log("���콺 ��");
            instantiatedUI = Instantiate(uiPrefab, transform.position, Quaternion.identity, transform.parent);

            // RectTransform ����
            //RectTransform rectTransform = instantiatedUI.GetComponent<RectTransform>();
            //if (rectTransform != null)
            //{
            //    rectTransform.anchoredPosition = new Vector2(650, 0); 
            //}
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (instantiatedUI != null)
        {
            // UI ����
            Debug.Log("���콺 ����");
            Destroy(instantiatedUI);
        }
    }

}