using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public PlayerStats stats;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI jumpText;
    public TextMeshProUGUI dashText;
    void Update()
    {
        healthText.text = "HP: " + stats.health + " / " + stats.GetMaxHealth();
        coinsText.text = "Coins: " + stats.coins;
        jumpText.text = "Extra Jumps Remaining: " + stats.extraJumps;
        dashText.text = "Can Dash: " + stats.canDash;
    }
}
