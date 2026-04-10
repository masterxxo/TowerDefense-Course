using System.Collections;
using UnityEngine;

public class CrossbowVisuals : MonoBehaviour
{
    private TowerCrossbow _crossbow;
    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualDuration = .1f;

    private void Awake()
    {
        _crossbow = GetComponent<TowerCrossbow>();
    }
    
    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(FXCoroutine(startPoint, endPoint));
    }

    private IEnumerator FXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        _crossbow.EnableRotation(false);
        attackVisuals.enabled = true;
        attackVisuals.SetPosition(0, startPoint);
        attackVisuals.SetPosition(1, endPoint);
        
        yield return new WaitForSeconds(attackVisualDuration);
        
        attackVisuals.enabled = false;
        _crossbow.EnableRotation(true);
    }
}
