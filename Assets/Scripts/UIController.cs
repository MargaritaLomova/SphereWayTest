using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("UI Components"), SerializeField]
    private GameObject loseText;
    [SerializeField]
    private GameObject winText;

    [Header("Objects From Scene"), SerializeField]
    private PlayerController player;

    private void Start()
    {
        loseText.SetActive(false);
        winText.SetActive(false);
    }

    public void ShowLoseText()
    {
        loseText.SetActive(true);
    }

    public void ShowWinText()
    {
        winText.SetActive(true);
    }

    public void OnMoveButtonClicked()
    {
        StartCoroutine(player.MoveToDoor());
    }
}