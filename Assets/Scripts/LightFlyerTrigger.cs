using UnityEngine;

public class LightFlyerTrigger : MonoBehaviour
{
    public GameObject lightFlyerPrefab;
    public Transform spawnPoint;

    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;
            var flyer = Instantiate(lightFlyerPrefab, spawnPoint.position, spawnPoint.rotation)
                        .GetComponent<LightFlyer>();

            flyer.OnFlyerDestroyed += () =>
            {
                triggered = false;
                Debug.Log("[MazeLightFlyerTrigger] LightFlyer mati, trigger aktif lagi.");
            };

            Debug.Log("[MazeLightFlyerTrigger] Light Flyer muncul!");
        }
    }
}
