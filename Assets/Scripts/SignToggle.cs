using UnityEngine;

public class SignToggle : MonoBehaviour
{
    //public GameObject sign;
    private Renderer sign;
    Player player;

    void Start()
    {
        sign = GetComponent<Renderer>();
        sign.enabled = false;
    }

    public void toggle()
    {
        if (FindAnyObjectByType<Player>().sendKillCount() == 0)
        {
            //if (sign.enabled == false)
            //{
            sign.enabled = true;
            //}
            //else
            //{
            //sign.enabled = false;
            //}
        }
    }
}
