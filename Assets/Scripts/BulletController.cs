using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Variables"), SerializeField]
    private float speedOfScaling;
    [SerializeField]
    private float speedOfTransform;

    private Transform door;

    private PlayerController player;
    private LevelController levelController;
    private InputController inputController;

    private float percentOfParent;

    private void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        player = FindObjectOfType<PlayerController>();
        inputController = FindObjectOfType<InputController>();

        door = FindObjectOfType<DoorController>().GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (inputController.isScreenTapped)
        {
            //Enlarge the bullet while holding the screen
            StartCoroutine(BulletMagnification());

            //If the bullet was pumped - a loss
            if (player.transform.localScale.x <= 0.2f)
            {
                levelController.Losing();
            }
        }
        else
        {
            player.UpdateStartSize();

            transform.position = Vector3.MoveTowards(transform.position, door.position, Time.fixedDeltaTime * speedOfTransform);

            if (transform.localScale.x < 0.2f)
            {
                DestroyMyself();
            }

            Invoke("DestroyMyself", 5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Let"))
        {
            var newScale = transform.localScale.x - (transform.localScale.x * (1 - percentOfParent));
            if (levelController.isGameInteractable)
            {
                transform.localScale = new Vector3(newScale, newScale, newScale);
            }
        }
    }

    #region Custom Methods

    private void DestroyMyself()
    {
        player.BulletWasDestroyed();
        Destroy(gameObject);
    }

    private void ReducingPlayerSize(float newBulletSize)
    {
        var newPlayerSize = player.startSize - newBulletSize;
        player.transform.localScale = new Vector3(newPlayerSize, newPlayerSize, newPlayerSize);
        percentOfParent = transform.localScale.x / player.transform.localScale.x;
        levelController.ReducingWaySize(newPlayerSize * 2);
    }

    private IEnumerator BulletMagnification()
    {
        while (transform.localScale.x < player.startSize && inputController.isScreenTapped)
        {
            if (transform.localScale.x >= (player.startSize * 0.98f))
            {
                speedOfScaling *= 1000;
            }
            var newScale = Mathf.Lerp(transform.localScale.x, player.startSize, Time.deltaTime * speedOfScaling);
            transform.localScale = new Vector3(newScale, newScale, newScale);
            ReducingPlayerSize(newScale);
            yield return null;
        }
    }

    #endregion
}