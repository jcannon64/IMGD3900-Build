using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void OnPlayPress() {
        FindAnyObjectByType<GameManager>().NewGame();
    }

    public void OnMenuPress() {
        FindAnyObjectByType<GameManager>().LoadLevel(0);
    }

    public void OnGuidePress() {
        FindAnyObjectByType<GameManager>().LoadLevel(7);
    }

    public void OnQuitPress() {
        Application.Quit();
    }
}
