using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    public GameObject[] boss;

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

        // º¸½º Æ÷Å»
        if (other.CompareTag("Player") && SceneManager.GetActiveScene().name == "Boss")
        {
            if (boss[0].gameObject.activeSelf && boss[1].gameObject.activeSelf)
            {
                SceneController.instance.LoadScene("Village");
            }
        }
    }
}
