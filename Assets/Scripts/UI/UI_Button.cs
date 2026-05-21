using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI_Animator _animatorUI;
    private RectTransform _rectTransform;

    [SerializeField] private float showcaseScale = 1.1f;
    [SerializeField] private float scaleUpDuration = .25f;

    private Coroutine _scaleCoroutine;
    [Space]
    [SerializeField] private UI_TextBlinkEffect uiTextBlinkEffect;

    private void Awake()
    {
        _animatorUI = GetComponentInParent<UI_Animator>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_scaleCoroutine != null)
            StopCoroutine(_scaleCoroutine);
        
        _scaleCoroutine = StartCoroutine(_animatorUI.ChangeScaleCo(_rectTransform, showcaseScale, scaleUpDuration));
        if (uiTextBlinkEffect != null)
        {
            uiTextBlinkEffect.EnableBlink(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_scaleCoroutine != null)
            StopCoroutine(_scaleCoroutine);
        
        _scaleCoroutine = StartCoroutine(_animatorUI.ChangeScaleCo(_rectTransform, 1, scaleUpDuration));
        if (uiTextBlinkEffect != null)
        {
            uiTextBlinkEffect.EnableBlink(true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _rectTransform.localScale = Vector3.one;
    }
}
