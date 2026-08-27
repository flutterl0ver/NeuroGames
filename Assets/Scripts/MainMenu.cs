using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel = null;

    private void Start()
    {
        settingsPanel?.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();

#endif
    }

    public void Settings()
    {
        settingsPanel?.SetActive(true);
    }
}
