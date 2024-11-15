using UnityEngine;

public class Hitboxes : MonoBehaviour
{
    void Start() {
        Invoke(nameof(DestroyHitbox), 0.25f);
    }

    private void DestroyHitbox() {
        Destroy(gameObject);
    }
}
