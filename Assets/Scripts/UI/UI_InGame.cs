using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    private UI_Animator _animatorUi;
    
    [SerializeField] private TextMeshProUGUI healthPointsText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private float waveTimerOffset;

    private void Awake()
    {
        _animatorUi = GetComponentInParent<UI_Animator>();
    }

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
        Transform waveTimerTransform = waveTimerText.transform.parent;
        float yOffset = enable ? -waveTimerOffset : waveTimerOffset;
        Vector3 offset = new Vector3(0, yOffset);
        
        
        
        _animatorUi.ChangePosition(waveTimerTransform, offset);
        // waveTimerText.transform.parent.gameObject.SetActive(enable);
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
