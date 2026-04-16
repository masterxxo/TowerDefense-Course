using System.Collections;
using UnityEngine;

public class CrossbowVisuals : MonoBehaviour
{
    private TowerCrossbow _crossbow;
    private Enemy _currentEnemy;
    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualDuration = .1f;

    [Header("Glowing Visuals")]
    private float _currentIntensity;
    [SerializeField] private float maxIntensity = 150;
    
    [Space]
    [SerializeField] private MeshRenderer meshRenderer;
    private Material _material;
    
    [Space]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [Header("Rotor Visuals")]
    [SerializeField] private Transform rotor;
    [SerializeField] private Transform rotorUnloaded;
    [SerializeField] private Transform rotorLoaded;
    
    
    [Header("Front glow string")]
    [SerializeField] private LineRenderer frontStringL;
    [SerializeField] private LineRenderer frontStringR;

    [Space]
    [SerializeField] private Transform frontStartPointL;
    [SerializeField] private Transform frontStartPointR;
    [SerializeField] private Transform frontEndPointL;
    [SerializeField] private Transform frontEndPointR;
    
    [Header("Back glow string")]
    [SerializeField] private LineRenderer backStringL;
    [SerializeField] private LineRenderer backStringR;

    [Space]
    [SerializeField] private Transform backStartPointL;
    [SerializeField] private Transform backStartPointR;
    [SerializeField] private Transform backEndPointL;
    [SerializeField] private Transform backEndPointR;

    [SerializeField] private LineRenderer[] lineRenderers;
    private void Awake()
    {
        _crossbow = GetComponent<TowerCrossbow>();
        _material = new Material(meshRenderer.material);
        meshRenderer.material = _material;
        
        UpdateMaterialOnLinerenderers();
        StartCoroutine(ChangeEmission(1));
    }

    private void UpdateMaterialOnLinerenderers()
    {
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            lineRenderer.material = _material;
        }
    }

    private void Update()
    {
        UpdateEmissionColor();
        UpdateStrings();

        if (attackVisuals.enabled && _currentEnemy != null)
        {
          attackVisuals.SetPosition(1, _currentEnemy.GetCenterPoint());  
        }
    }

    private void UpdateStrings()
    {
        UpdateStringVisual(frontStringR, frontStartPointR, frontEndPointR);
        UpdateStringVisual(frontStringL, frontStartPointL, frontEndPointL);
        UpdateStringVisual(backStringL, backStartPointL, backEndPointL);
        UpdateStringVisual(backStringR, backStartPointR, backEndPointR);
    }

    public void PlayReloadFX(float duration)
    {
        float halfDuration = duration / 2;
        StartCoroutine(ChangeEmission(halfDuration));
        StartCoroutine(UpdateRotorPosition(halfDuration));
    }
    
    private void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, _currentIntensity / maxIntensity);
        emissionColor = emissionColor * Mathf.LinearToGammaSpace(_currentIntensity);
        _material.SetColor("_EmissionColor", emissionColor);
    }
    
    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint));
    }

    private IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        //_crossbow.EnableRotation(false);
        _currentEnemy = _crossbow.currentEnemy;
        attackVisuals.enabled = true;
        attackVisuals.SetPosition(0, startPoint);
        attackVisuals.SetPosition(1, endPoint);
        
        yield return new WaitForSeconds(attackVisualDuration);
        
        attackVisuals.enabled = false;
        //_crossbow.EnableRotation(true);
    }

    private IEnumerator ChangeEmission(float duration)
    {
        
        float startTime = Time.time;
        float startIntensity = 0;
        
        // Do something repeatedly until the duration has passed
        while (Time.time - startTime < duration)
        {
            // Calculates the proportion of the duration that has elapsed since the start of the coroutine
            float tValue = (Time.time - startTime) / duration;
            _currentIntensity = Mathf.Lerp(startIntensity, maxIntensity, tValue);
            yield return null;
        }
        
        _currentIntensity = maxIntensity;
    }

    private IEnumerator UpdateRotorPosition(float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float tValue = (Time.time - startTime) / duration;
            rotor.position = Vector3.Lerp(rotorUnloaded.position, rotorLoaded.position, tValue);
            yield return null;
        }
        rotor.position = rotorLoaded.position;
    }

    private void UpdateStringVisual(LineRenderer lineRenderer, Transform startPoint, Transform endPoint)
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
