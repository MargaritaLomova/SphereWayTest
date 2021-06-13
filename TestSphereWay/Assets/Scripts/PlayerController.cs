using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Variables"), SerializeField]
    private float speed = 10f;

    [Header("Prefabs"), SerializeField]
    private BulletController bulletPrefab;

    [Header("Objects From Scene"), SerializeField]
    private LevelController levelController;
    [SerializeField]
    private Transform door;

    private bool isCanMove = true;
    private bool isScreenTapped = false;
    private bool isHaveBulletOnScreen = false;
    private Rigidbody rb;
    private float startSize;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        startSize = transform.localScale.x;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isCanMove)
        {
            if (!isHaveBulletOnScreen && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Stationary && Input.touches[0].phase != TouchPhase.Began)
            {
                isScreenTapped = true;
                Shoot();
            }
            else if (Input.touchCount == 0)
            {
                isScreenTapped = false;
            }
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

    public bool GetIsScreenTapped()
    {
        return isScreenTapped;
    }

    public float GetStartSize()
    {
        return startSize;
    }

    public void BulletWasDestroyed()
    {
        isHaveBulletOnScreen = false;
    }

    public void DisableMove()
    {
        isScreenTapped = false;
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