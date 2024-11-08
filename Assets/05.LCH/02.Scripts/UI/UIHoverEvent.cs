using UnityEngine;

public class UIHoverEvent : MonoBehaviour
{
    public GameObject toolTip;

    public void HoverEnter()
    {
        toolTip.SetActive(true);
    }

    public void HoverExit()
    {
        toolTip.SetActive(false);
    }
}
