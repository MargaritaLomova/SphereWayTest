using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool isScreenTapped { get; private set; }

    private void FixedUpdate()
    {
#if UNITY_EDITOR

        isScreenTapped = Input.GetMouseButton(0);

#else
        isScreenTapped = Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Stationary && Input.touches[0].phase != TouchPhase.Began;
#endif
    }
}