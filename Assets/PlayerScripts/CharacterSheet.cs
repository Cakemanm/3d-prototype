using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class CharacterSheet : MonoBehaviour
{
    public GameObject sheetPanel;
    public TextMeshProUGUI statsText;
    public Button vitButton;

    private PlayerStats stats;
    private PlayerMovement movement;
    private PlayerLook look;
    private bool isOpen = false;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        movement = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();

        sheetPanel.SetActive(false);
        vitButton.onClick.AddListener(stats.BuyVit);
    }

    public void OnCharacterSheet(InputValue value)
    {
        if (!value.isPressed) return;
        SetOpen(!isOpen);
    }

    public void SetOpen(bool open)
    {
        isOpen = open;
        sheetPanel.SetActive(open);
        movement.enabled = !open;
        look.enabled = !open;
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = open;
    }

    void Update()
    {
        if (!isOpen) return;

        statsText.text = "Coins: " + stats.coins + "\n" +
                         "Vitality: " + stats.vit + "\n" +
                         "Max Health: " + stats.GetMaxHealth();
        
        vitButton.interactable = stats.coins >= stats.vitCost; // Enable button only if player has enough coins
    }
}
