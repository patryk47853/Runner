using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Screens")] [SerializeField] private GameObject chooseLevelScreen;
    [SerializeField] private GameObject defaultScreen;

    private void Awake()
    {
        chooseLevelScreen.SetActive(false);
        defaultScreen.SetActive(true);
        // gameOverScreen.SetActive(false);
        // pauseScreen.SetActive(false);
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //If pause screen already active unpause and viceversa
        // PauseGame(!pauseScreen.activeInHierarchy);
        // }
    }

    //Start the game
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }


    //Choose level
    public void ChooseLevel()
    {
        print("Choosing level");
        chooseLevelScreen.SetActive(true);
        defaultScreen.SetActive(false);
    }

    public void GetBackToMainMenu()
    {
        print("Get back to main menu");
        chooseLevelScreen.SetActive(false);
        defaultScreen.SetActive(true);
    }

    //Quit the game
    public void Quit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
}