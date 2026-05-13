using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    private CameraController _camController;
    
    [Header("Keyboard Sensitivity")]
    [SerializeField] private Slider keyboardSensitivitySlider;
    [SerializeField] private string keyboardSenseParameter = "KeyboardSense";
    
    [SerializeField] private float minKeyboardSensitivity = 60;
    [SerializeField] private float maxKeyboardSensitivity = 240;

    private void Awake()
    {
        _camController = FindAnyObjectByType<CameraController>();
    }
    
    public void KeyboardSensitivity(float value)
    {
        float newSensitivity = Mathf.Lerp(minKeyboardSensitivity, maxKeyboardSensitivity, value);
        _camController.AdjustKeyboardSensitivity(newSensitivity);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(keyboardSenseParameter, keyboardSensitivitySlider.value);
    }

    private void OnEnable()
    {
        keyboardSensitivitySlider.value = PlayerPrefs.GetFloat(keyboardSenseParameter, .5f);
    }
}
