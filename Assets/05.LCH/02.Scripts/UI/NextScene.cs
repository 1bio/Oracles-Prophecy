using UnityEngine;

public class NextScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneController.instance.LoadScene("Stage1");
        }
    }
}
