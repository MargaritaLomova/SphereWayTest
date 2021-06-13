using UnityEngine;

public class LetController : MonoBehaviour
{
    [Header("Child Objects"), SerializeField]
    private GameObject explosion;

    private LevelController levelController;

    private void Start()
    {
        explosion.SetActive(false);
        levelController = FindObjectOfType<LevelController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            explosion.SetActive(true);
            Destroy(gameObject, 0.2f);
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            levelController.Losing();
        }
    }
}