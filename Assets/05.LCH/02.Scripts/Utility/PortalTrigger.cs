using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // ¸¶À» Æ÷Å»
        if (other.CompareTag("Player") && SceneManager.GetActiveScene().name == "Village")
        {
            SceneController.instance.LoadScene("Abandoned Prison");
        }

        // ´øÀü Æ÷Å»
        if (other.CompareTag("Player") && SceneManager.GetActiveScene().name == "Abandoned Prison")
        {
            SceneController.instance.LoadScene("Boss");
        }
    }
}
