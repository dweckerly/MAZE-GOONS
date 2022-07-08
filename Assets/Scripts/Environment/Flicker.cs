using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    public float MaxReduction;
    public float MaxIncrease;
    public float RateDamping;
    public float Strength;
    public bool StopFlickering;

    private Light _lightSource;
    private float _baseIntensity;

    public void Start()
    {
        _lightSource = GetComponent<Light>();
        if (_lightSource == null)
        {
            Debug.LogError("Flicker script must have a Light Component on the same GameObject.");
            return;
        }
        _baseIntensity = _lightSource.intensity;
        StartCoroutine(DoFlicker());
    }

    private IEnumerator DoFlicker()
    {
        while (!StopFlickering)
        {
            _lightSource.intensity = Mathf.Lerp(_lightSource.intensity, Random.Range(_baseIntensity - MaxReduction, _baseIntensity + MaxIncrease), Strength * Time.deltaTime);
            yield return new WaitForSeconds(RateDamping);
        }
    }
}
