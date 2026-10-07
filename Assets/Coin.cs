using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    void OnTriggerEnter(Collider other) {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null) {
            playerStats.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }
}
