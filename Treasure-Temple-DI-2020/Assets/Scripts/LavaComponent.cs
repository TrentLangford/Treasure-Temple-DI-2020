using UnityEngine.SceneManagement;
using UnityEngine;

public class LavaComponent : MonoBehaviour
{
    // A small class responible for the flaming gorge components.
    public bool isActive;
    public GameObject[] actives;
    // respawns the player if it enters its collision box. Sadly, because of a lack of time, the collision boxes were removed
    // so that you could actually make it through the flaming gorge
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
