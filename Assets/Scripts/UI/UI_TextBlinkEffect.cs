using System;
using TMPro;
using UnityEngine;

public class UI_TextBlinkEffect : MonoBehaviour
{
    [SerializeField] private float changeValueSpeed;
    private float _targetAlpha;
    private bool _canBlink;

    private TextMeshProUGUI _textMesh;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!_canBlink)
            return;
        
        if (Mathf.Abs(_textMesh.color.a - _targetAlpha) > .01f)
        {
            float newAlpha = Mathf.Lerp(_textMesh.color.a, _targetAlpha, changeValueSpeed * Time.deltaTime);
            ChangeColorAlpha(newAlpha);
        }
        else
        {
            ChangeTargetAlpha();
        }
    }

    public void EnableBlink(bool enable)
    {
        _canBlink = enable;

        if (!_canBlink)
        {
            ChangeColorAlpha(1);
        }
    }
    
    private void ChangeTargetAlpha() => _targetAlpha = (_targetAlpha == 1) ? 0 : 1;
    

    private void ChangeColorAlpha(float newAlpha)
    {
        Color myColor = _textMesh.color;
        _textMesh.color = new Color(myColor.r, myColor.g, myColor.b, newAlpha);
    }
}
