using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthPointsText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI waveTimerText;

    public void UpdateHealthPointsUI(int currentHealth, int maxHealth)
    {
        int value = maxHealth - currentHealth;
        healthPointsText.text = "Threat "  + value + "/" + maxHealth;
    }

    public void UpdateCurrencyUI(int currentCurrency)
    {
        currencyText.text = "Currency " + currentCurrency;
    }

    public void UpdateWaveTimerUI(float currentWave)
    {
        waveTimerText.text = "Next wave " + currentWave.ToString("00");
    }

    public void EnableWaveTimer(bool enable)
    {
        waveTimerText.transform.parent.gameObject.SetActive(enable);
    }

    public void ForceWaveButtonAction()
    {
        WaveManager waveManager = FindAnyObjectByType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.ForceNextWave();
        }
    }
}
