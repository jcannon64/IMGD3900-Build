using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int level;

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void NewGame() {
        Manager.health = 100f;
        Manager.chips = 0f;
        Manager.spadesAmmo = 4f;
        Manager.heartsAmmo = 4f;
        Manager.clubsAmmo = 4f;
        Manager.diamondsAmmo = 4f;
        Manager.spadesUpgrade = false;
        Manager.heartsUpgrade = false;
        Manager.clubsUpgrade = false;
        Manager.diamondsUpgrade = false;
        Manager.loops = 0;
        LoadLevel(3);
    }

    public void LoadLevel(int index) {
        level = index;

        Camera camera = Camera.main;
        if(camera != null) {
            camera.cullingMask = 0;
        }

        LoadScene();
    }

    public int currLevel() {
        return level;
    }

    private void LoadScene() {
        SceneManager.LoadScene(level);
    }

    public void LevelComplete() {
        if(level == 6) {
            Manager.loops++;
        }
        
        int nextLevel = level + 1;
        if(nextLevel < SceneManager.sceneCountInBuildSettings - 1) {
            LoadLevel(nextLevel);
        }
        else {
            LoadLevel(3);
        }
    }

    public void LevelFailed() {
        LoadLevel(1);
    }
}
