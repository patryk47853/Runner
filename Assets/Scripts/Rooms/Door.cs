using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    [Header("Last doors handle")] [SerializeField]
    private int nextLevelIndex;

    [SerializeField] private int menuSceneIndex;

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
            {
                if (nextRoom != null)
                {
                    cam.MoveToNewRoom(nextRoom);
                    nextRoom.GetComponent<Room>().ActivateRoom(true);
                    previousRoom.GetComponent<Room>().ActivateRoom(false);
                }
                else
                    LoadNextScene();
            }
            else
            {
                cam.MoveToNewRoom(previousRoom);
                previousRoom.GetComponent<Room>().ActivateRoom(true);
                if (nextRoom != null)
                    nextRoom.GetComponent<Room>().ActivateRoom(false);
            }
        }
    }

    private void LoadNextScene()
    {
        if (nextLevelIndex >= 0)
        {
            SceneManager.LoadScene(nextLevelIndex, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(menuSceneIndex, LoadSceneMode.Single);
        }
    }
}