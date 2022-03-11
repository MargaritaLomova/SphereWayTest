using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Header("Objects From Scene"), SerializeField]
    private PlayerController player;
    [SerializeField]
    private DoorController door;
    [SerializeField]
    private GameObject plane;
    [SerializeField]
    private GameObject way;
    [SerializeField]
    private UIController uiController;

    [Header("Prefabs"),SerializeField]
    private LetController letPrefab;

    [Header("Variables To Level Control"), SerializeField]
    private int minLetCount;
    [SerializeField]
    private int maxLetCount;
    [SerializeField]
    private float indentFromDoor;
    [SerializeField]
    private float indentFromPlayerStart;

    public bool isGameInteractable { get; private set; }

    private Bounds planeArea;
    private Bounds doorArea;
    private Bounds playerStartArea;
    private Scene currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        planeArea = new Bounds(plane.transform.localPosition, new Vector3(plane.transform.localScale.x * 4.5f, 1, plane.transform.localScale.z *6) /*plane.transform.localScale * 4.5f*/);
        doorArea = new Bounds(door.transform.localPosition, new Vector3(door.transform.localScale.x + indentFromDoor, door.transform.localScale.y + indentFromDoor, door.transform.localScale.z + indentFromDoor));
        playerStartArea = new Bounds(player.transform.localPosition, new Vector3(player.transform.localScale.x + indentFromPlayerStart, door.transform.localScale.y + indentFromPlayerStart, door.transform.localScale.z + indentFromPlayerStart));

        DestroyAllExistingBullets();

        DestroyAllExistingLets();
        SpawnLets();
    }

    #region Custom Methods

    #region Public

    public void ReducingWaySize(float newSize)
    {
        way.transform.localScale = new Vector3(newSize, way.transform.localScale.y, way.transform.localScale.z);
    }

    public void Win()
    {
        uiController.ShowWinText();
        player.DisableMove();
    }

    public void Losing()
    {
        isGameInteractable = false;

        uiController.ShowLoseText();
        player.DisableMove();
        Invoke("ReloadLevel", 2f);
    }

    #endregion

    #region Private

    private void ReloadLevel()
    {
        DestroyAllExistingBullets();
        DestroyAllExistingLets();

        isGameInteractable = true;

        SceneManager.LoadScene(currentScene.buildIndex);
    }

    private void DestroyAllExistingBullets()
    {
        foreach (var bullet in FindObjectsOfType<BulletController>(true))
        {
            Destroy(bullet.gameObject);
        }
    }

    private void DestroyAllExistingLets()
    {
        foreach(var let in FindObjectsOfType<LetController>(true))
        {
            Destroy(let.gameObject);
        }
    }

    private void SpawnLets()
    {
        var randomCountOfLets = Random.Range(minLetCount, maxLetCount);
        for (int i = 0; i < randomCountOfLets; i++)
        {
            var newLet = Instantiate(letPrefab);
            newLet.transform.position = CreateSuitableVector();
        }
    }

    private Vector3 CreateSuitableVector()
    {
        var randomX = Random.Range(-planeArea.size.x, planeArea.size.x);
        var randomZ = Random.Range(-planeArea.size.z, planeArea.size.z);

        var newLetPosition = new Vector3(randomX, 1, randomZ);

        bool vectorIsSuitable = !doorArea.Contains(newLetPosition) && !playerStartArea.Contains(newLetPosition) && planeArea.Contains(newLetPosition);
        while(!vectorIsSuitable)
        {
            randomX = Random.Range(planeArea.min.x, planeArea.max.x);
            randomZ = Random.Range(planeArea.min.z, planeArea.max.z);

            newLetPosition = new Vector3(randomX, 0.5f, randomZ);

            vectorIsSuitable = !doorArea.Contains(newLetPosition) && !playerStartArea.Contains(newLetPosition);
        }

        return newLetPosition;
    }

    #endregion

    #endregion
}