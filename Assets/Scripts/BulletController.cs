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
    private Rigidbody rb;
    private Vector3 startPosition;
    private float percentOfParent;

    private void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        player = FindObjectOfType<PlayerController>();
        door = FindObjectOfType<DoorController>().GetComponent<Transform>();
        startPosition = transform.position;
    }

    private void Update()
    {
        if (player.GetIsScreenTapped())
        {
            //����������� ���� ���� ����� �����
            StartCoroutine(BulletMagnification());

            //���� ���� ���������� - ��������
            if (player.transform.localScale.x <= 0.2f)
            {
                levelController.Losing();
            }
        }
        else
        {
            //��������� rigidbody, ��������� ��������� ������ ������
            if (!rb)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                player.UpdateStartSize();
            }

            //����������� ����
            Vector3 movement = -Vector3.MoveTowards(startPosition, door.position, 0);
            rb.AddForce(movement * Time.deltaTime * speedOfTransform);

            //���� ���� �����������, ���������� �
            if (transform.localScale.x < 0.2f)
            {
                DestroyMyself();
            }

            //���������� ���� ������ ��������� �����
            Invoke("DestroyMyself", 5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Let"))
        {
            var newScale = transform.localScale.x - (transform.localScale.x * (1 - percentOfParent));
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    #region Custom Methods

    private void DestroyMyself()
    {
        player.BulletWasDestroyed();
        Destroy(gameObject);
    }

    private IEnumerator BulletMagnification()
    {
        while (transform.localScale.x < player.GetStartSize() && player.GetIsScreenTapped())
        {
            if (transform.localScale.x >= (player.GetStartSize() * 0.98f))
            {
                speedOfScaling *= 1000;
            }
            var newScale = Mathf.Lerp(transform.localScale.x, player.GetStartSize(), Time.deltaTime * speedOfScaling);
            transform.localScale = new Vector3(newScale, newScale, newScale);
            ReducingPlayerSize(newScale);
            yield return null;
        }
    }

    private void ReducingPlayerSize(float newBulletSize)
    {
        var newPlayerSize = player.GetStartSize() - newBulletSize;
        player.transform.localScale = new Vector3(newPlayerSize, newPlayerSize, newPlayerSize);
        percentOfParent = transform.localScale.x / player.transform.localScale.x;
        levelController.ReducingWaySize(newPlayerSize * 2);
    }

    #endregion
}