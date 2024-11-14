using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void OnPlayPress() {
        FindAnyObjectByType<GameManager>().LoadLevel(1);
    }

    public void OnMenuPress() {
        FindAnyObjectByType<GameManager>().LoadLevel(0);
   }

    public void OnQuitPress() {
        Application.Quit();
   }
}
