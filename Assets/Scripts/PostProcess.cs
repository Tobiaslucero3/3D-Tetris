using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcess : MonoBehaviour
{
    public bool on;
    public int xMultiplier;

    public LensDistortion lensD;

    // Start is called before the first frame update
    void Start()
    {
        lensD = FindObjectOfType<LensDistortion>();
    }

    // Update is called once per frame
    void Update()
    {
        lensD.intensity.value = 0.5f;
        lensD.intensityX.value = 0.99f;
    }
}
