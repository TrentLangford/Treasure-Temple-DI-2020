using UnityEngine.SceneManagement;
using UnityEngine;

public class LavaComponent : MonoBehaviour
{
    public bool isActive;
    public GameObject[] actives;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Player")) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Update()
    {
        if (isActive)
        {
            actives[0].SetActive(true);
            actives[1].SetActive(false);
        }
        if (!isActive)
        {
            actives[0].SetActive(false);
            actives[1].SetActive(true);
        }
    }
}
