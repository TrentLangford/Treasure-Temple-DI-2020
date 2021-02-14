using UnityEngine.SceneManagement;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Player")) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
