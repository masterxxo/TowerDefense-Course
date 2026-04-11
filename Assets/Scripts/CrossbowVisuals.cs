using System.Collections;
using UnityEngine;

public class CrossbowVisuals : MonoBehaviour
{
    private TowerCrossbow _crossbow;
    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualDuration = .1f;

    [Header("Glowing Visuals")]
    [SerializeField] private float currentIntensity;
    [SerializeField] private float maxIntensity = 150;
    
    [Space]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material material;
    
    [Space]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    
    private void Awake()
    {
        _crossbow = GetComponent<TowerCrossbow>();
        
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;
    }
    
    private void Update()
    {
        UpdateEmissionColor();
    }

    public void PlayReloadFX(float duration)
    {
        StartCoroutine(ChangeEmission(duration));
    }
    
    private void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);
        emissionColor = emissionColor * Mathf.LinearToGammaSpace(currentIntensity);
        material.SetColor("_EmissionColor", emissionColor);
    }
    
    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint));
    }

    private IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        _crossbow.EnableRotation(false);
        attackVisuals.enabled = true;
        attackVisuals.SetPosition(0, startPoint);
        attackVisuals.SetPosition(1, endPoint);
        
        yield return new WaitForSeconds(attackVisualDuration);
        
        attackVisuals.enabled = false;
        _crossbow.EnableRotation(true);
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
            currentIntensity = Mathf.Lerp(startIntensity, maxIntensity, tValue);
            yield return null;
        }
        
        currentIntensity = maxIntensity;
    }
}
