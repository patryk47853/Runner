using UnityEngine;
using UnityEngine.SceneManagement;


namespace UI
{
    public class ChooseLevelManager : MonoBehaviour
    {
        public void LoadLevel(int levelIndex)
        {
            SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
        }
    }
}