using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideWindow : MonoBehaviour
{
    public GameObject slideWindow;
    [SerializeField] private float slideSpeed;

    public void SlideWindow_On()
    {
        slideWindow.GetComponent<RectTransform>().DOAnchorPosX(-670f, slideSpeed);
    }

    public void SlideWindow_Off()
    {
        slideWindow.GetComponent<RectTransform>().DOAnchorPosX(-1200, slideSpeed);
    }
}
