using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Components"), SerializeField]
    private Rigidbody rb;

    [Header("Variables"), SerializeField]
    private float speed = 10f;

    [Header("Prefabs"), SerializeField]
    private BulletController bulletPrefab;

    [Header("Objects From Scene"), SerializeField]
    private LevelController levelController;
    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private Transform door;

    public float startSize { get; private set; }

    private bool isCanMove = true;
    private bool isHaveBulletOnScreen = false;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        startSize = transform.localScale.x;
    }

    private void FixedUpdate()
    {
        if (isCanMove && !isHaveBulletOnScreen && inputController.isScreenTapped)
        {
            Shoot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            levelController.Win();
        }
    }

    #region Custom Methods

    #region Public

    public void BulletWasDestroyed()
    {
        isHaveBulletOnScreen = false;
    }

    public void DisableMove()
    {
        isCanMove = false;
    }

    public void UpdateStartSize()
    {
        startSize = transform.localScale.x;
    }

    public IEnumerator MoveToDoor()
    {
        while (isCanMove)
        {
            Vector3 movement = -Vector3.MoveTowards(startPosition, door.position, 0);
            rb.AddForce(movement * Time.deltaTime * speed);
            yield return null;
        }
    }

    #endregion

    #region Private

    private void Shoot()
    {
        isHaveBulletOnScreen = true;
        var bullet = Instantiate(bulletPrefab);
        bullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2.5f);
    }

    #endregion

    #endregion
}