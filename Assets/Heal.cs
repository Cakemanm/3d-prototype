using UnityEngine;

public class Heal : MonoBehaviour
{
    public int healValue = 5;
    void OnTriggerEnter(Collider other) {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null) {
            playerStats.Heal(healValue);
            Destroy(gameObject);
        }
    }
}
