using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public PlayerStats stats;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinsText;
    void Update()
    {
        healthText.text = "HP: " + stats.health + " / " + stats.GetMaxHealth();
        coinsText.text = "Coins: " + stats.coins;
    }
}
