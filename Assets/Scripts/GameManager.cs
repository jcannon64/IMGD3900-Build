using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int level;

    private void Start() {
        DontDestroyOnLoad(gameObject);
        NewGame();
    }

    private void NewGame() {
        LoadLevel(1);
    }

    private void LoadLevel(int index) {
        level = index;

        Camera camera = Camera.main;
        if(camera != null) {
            camera.cullingMask = 0;
        }

        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadScene() {
        SceneManager.LoadScene(level);
    }

    public void LevelComplete() {
        int nextLevel = level + 1;
        if(nextLevel < SceneManager.sceneCountInBuildSettings) {
            LoadLevel(nextLevel);
        }
        else LoadLevel(1);
    }

    public void LevelFailed() {
        LoadLevel(level);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
