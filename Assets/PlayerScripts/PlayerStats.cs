using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float invincibilityTime = 0.5f;
    private float lastHitTime = -999f;
    
    public int maxHealth = 20;
    public int health;
    public int coins = 0;

    public int vit = 0;

    [Header("Death")]
    public GameObject deathScreen;
    public float respawnDelay = 1f;


    public int GetMaxHealth() {
        return maxHealth + (vit * 2); // Each point of vitality increases max health by 2
    }

    private CharacterController controller;
    private PlayerMovement movement;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        movement = GetComponent<PlayerMovement>();
        health = GetMaxHealth();
        deathScreen.SetActive(false); // Ensure the death screen is hidden at the start
    }

    public void TakeDamage(int amount) {
        if (health <= 0) {
            return; // Player is already dead
        }

        if (Time.time < lastHitTime + invincibilityTime) {
            return; // Player is still invincible
        }
        lastHitTime = Time.time;
        
        health = Mathf.Max(health - amount, 0);
        if (health <= 0) {
            Die();
        }
    }

    public void Heal(int amount) {
        health = Mathf.Min(health + amount, GetMaxHealth());
    }

    void Die() {
        Debug.Log("Player has died.");
        // TODO: Add death logic here (e.g., respawn, game over screen, etc.)
        movement.enabled = false; // Disable player movement
        deathScreen.SetActive(true); // Show the death screen
        Invoke(nameof(Respawn), respawnDelay); // Respawn after a delay
    }

    void Respawn() {
        transform.position = new Vector3(0f, 1f, 0f); // Reset position to a default spawn point (you can change this)
        controller.enabled = true; // Enable player controls
        movement.enabled = true; // Enable player movement
        deathScreen.SetActive(false); // Hide the death screen
        health = GetMaxHealth(); // Reset health on respawn
    }

    public void AddCoins(int amount) {
        coins += amount;
    }
}
