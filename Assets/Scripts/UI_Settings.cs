using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    private CameraController _camController;
    
    [Header("Keyboard Sensitivity")]
    [SerializeField] private Slider keyboardSenseSlider;
    [SerializeField] private string keyboardSenseParameter = "KeyboardSense";
    [SerializeField] private TextMeshProUGUI keyboardSenseText;
    
    [SerializeField] private float minKeyboardSense = 60;
    [SerializeField] private float maxKeyboardSense = 240;
    
    [Header("Mouse Sensitivity")]
    [SerializeField] private Slider mouseSenseSlider;
    [SerializeField] private string mouseSenseParameter = "MouseSense";
    [SerializeField] private TextMeshProUGUI mouseSenseText;
    
    [SerializeField] private float minMouseSense = 1;
    [SerializeField] private float maxMouseSense = 10;

    private void Awake()
    {
        _camController = FindAnyObjectByType<CameraController>();
    }
    
    public void KeyboardSensitivity(float value)
    {
        float newSensitivity = Mathf.Lerp(minKeyboardSense, maxKeyboardSense, value);
        _camController.AdjustKeyboardSensitivity(newSensitivity);

        keyboardSenseText.text = Mathf.RoundToInt(value*100) + "%";
    }

    public void MouseSensitivity(float value)
    {
        float newSensitivity = Mathf.Lerp(minMouseSense, maxMouseSense, value);
        _camController.AdjustMouseSensitivity(newSensitivity);
        
        mouseSenseText.text = Mathf.RoundToInt(value*100) + "%";
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(keyboardSenseParameter, keyboardSenseSlider.value);
        PlayerPrefs.SetFloat(mouseSenseParameter, mouseSenseSlider.value);
    }

    private void OnEnable()
    {
        keyboardSenseSlider.value = PlayerPrefs.GetFloat(keyboardSenseParameter, .5f);
        mouseSenseSlider.value = PlayerPrefs.GetFloat(mouseSenseParameter, 5f);
    }
}
