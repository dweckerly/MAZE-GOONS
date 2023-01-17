using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    public float maxIntensityReduction;
    public float maxIntensityIncrease;
    public float intensityStrength;

    public float maxRangeReduction;
    public float maxRangeIncrease;
    public float rangeStrength;
    
    public float RateDamping;
    
    public bool StopFlickering;

    private Light _lightSource;
    private float _baseIntensity;
    private float _baseRange;

    public void Start()
    {
        _lightSource = GetComponent<Light>();
        if (_lightSource == null)
        {
            Debug.LogError("Flicker script must have a Light Component on the same GameObject.");
            return;
        }
        _baseIntensity = _lightSource.intensity;
        _baseRange = _lightSource.range;
        StartCoroutine(DoFlicker());
    }

    private IEnumerator DoFlicker()
    {
        while (!StopFlickering)
        {
            _lightSource.intensity = Mathf.Lerp(_lightSource.intensity, Random.Range(_baseIntensity - maxIntensityReduction, _baseIntensity + maxIntensityIncrease), intensityStrength * Time.deltaTime);
            _lightSource.range = Mathf.Lerp(_lightSource.range, Random.Range(_baseRange - maxRangeReduction, _baseRange + maxRangeIncrease), rangeStrength * Time.deltaTime);
            yield return new WaitForSeconds(RateDamping);
        }
    }
}
