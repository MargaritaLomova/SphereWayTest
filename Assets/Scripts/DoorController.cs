using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Components"), SerializeField]
    private GameObject doorVisuale;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor(false);
        }
    }

    #region Custom Methods

    private void OpenDoor(bool isNeedToOpen)
    {
        if (isNeedToOpen)
        {
            doorVisuale.transform.rotation = Quaternion.Euler(0, -75, 0);
        }
        else
        {
            doorVisuale.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    #endregion
}