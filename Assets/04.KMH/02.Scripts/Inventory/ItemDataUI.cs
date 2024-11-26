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
            // UI 생성
            Debug.Log("마우스 온");
            instantiatedUI = Instantiate(uiPrefab, transform.position, Quaternion.identity, transform.parent);

            // RectTransform 설정
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
            // UI 제거
            Debug.Log("마우스 오프");
            Destroy(instantiatedUI);
        }
    }

}