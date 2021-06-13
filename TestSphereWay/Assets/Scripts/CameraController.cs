using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Scene Objects"), SerializeField]
    private Transform player;

    private Vector3 distance;

    private void Start()
    {
        distance =  transform.position - player.position;
    }

    private void Update()
    {
        transform.position = player.position + distance;
    }
}