using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthPointsText;

    public void UpdateHealthPointsUI(int currentHealth, int maxHealth)
    {
        int value = maxHealth - currentHealth;
        healthPointsText.text = "Threat "  + value + "/" + maxHealth;
    }
}
