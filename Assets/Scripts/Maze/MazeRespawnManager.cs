using UnityEngine;

public class MazeRespawnManager : MonoBehaviour
{
    public static MazeRespawnManager Instance;

    [Header("Spawn Point")]
    public Transform spawnPoint; 

    private GameObject player;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void RespawnPlayer()
    {
        if (player == null || spawnPoint == null)
        {
            Debug.LogWarning("[MazeRespawnManager] Player atau spawn point belum di-assign!");
            return;
        }

        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;

        Debug.Log("[MazeRespawnManager] Player dikembalikan ke titik awal!");
    }
}
