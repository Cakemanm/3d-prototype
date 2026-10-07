using UnityEngine;

public class DMG : MonoBehaviour
{
    public int damageValue = 2;
    public bool oneHit = true;
    public float tickRate = 1f; // Time in seconds between damage ticks for continuous damage

    private PlayerStats playerInside;
    private float nextDamageTime;

    void OnTriggerEnter(Collider other) {
        Debug.Log("Entered damage zone: " + other.name);
        PlayerStats playerStats = other.GetComponent<PlayerStats>();

        if (playerStats == null) return;

        playerStats.TakeDamage(damageValue);

        if (oneHit) {
            Destroy(gameObject);
            return; // Exit the method to avoid extra dmg hit
        }

        playerInside = playerStats;
        nextDamageTime = Time.time + tickRate; // Set the next damage time for continuous damage
    }

    void OnTriggerExit(Collider other) {
        Debug.Log("Exited damage zone: " + other.name);
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null) {
            playerInside = null;
        }
    }

    void Update() {
        if (playerInside != null && !oneHit && Time.time >= nextDamageTime) {
            playerInside.TakeDamage(damageValue);
            nextDamageTime = Time.time + tickRate; // Update the next damage time
        }
    }
}
