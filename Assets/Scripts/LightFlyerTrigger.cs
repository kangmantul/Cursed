using UnityEngine;

public class LightFlyerTrigger : MonoBehaviour
{
    public GameObject lightFlyerPrefab;
    public Transform spawnPoint;
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;
        if (lightFlyerPrefab == null || spawnPoint == null) return;

        if (other == null || other.transform == null)
        {
            Debug.LogWarning("[LightFlyerTrigger] Player reference invalid — skipping spawn.");
            return;
        }

        triggered = true;
        var flyerObj = Instantiate(lightFlyerPrefab, spawnPoint.position, spawnPoint.rotation);
        var flyer = flyerObj.GetComponent<LightFlyer>();

        if (flyer != null)
        {
            flyer.Init(other.transform);
        }
        else
        {
            Debug.LogWarning("[LightFlyerTrigger] LightFlyer component missing on prefab.");
        }

        Destroy(gameObject, 1f); 
    }
}
