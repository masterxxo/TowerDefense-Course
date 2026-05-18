using UnityEngine;

public class UI_BuildButtons : MonoBehaviour
{
    [SerializeField] private float yPositionOffset;
    private bool _isActive;
    private UI_Animator _animatorUI;

    private void Awake()
    {
        _animatorUI = GetComponentInParent<UI_Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ShowBuildButton();
        }
    }

    public void ShowBuildButton()
    {
        _isActive = !_isActive;
        
        float yOffset = _isActive ? yPositionOffset : -yPositionOffset;
        Vector3 offset = new Vector3(0, yOffset);
        
        _animatorUI.ChangePosition(transform, offset);
    }
}
