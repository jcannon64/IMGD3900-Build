using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    public GameObject prefab;

    private void Start() {
        Spawn();
    }
    
    private void Spawn() {
        Instantiate(prefab, transform.position, Quaternion.identity);
        prefab.SetActive(true);
        Invoke(nameof(Spawn), Random.Range(5, 10));
    }
}
