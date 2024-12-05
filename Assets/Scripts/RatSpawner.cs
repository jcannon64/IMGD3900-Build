using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int max = 3;

    private void Start() {
        Spawn();
    }
    
    private void Spawn() {
        if(max > 0) {
            Instantiate(prefab, transform.position, Quaternion.identity);
            prefab.SetActive(true);
            Invoke(nameof(Spawn), Random.Range(5, 10));
            max -= 1;
        }
    }
}
