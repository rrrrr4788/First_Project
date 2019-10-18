using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float periods = 2f;
    [Range(0, 1)] [SerializeField] float movementFactor;

    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / periods;
        const float tau = 2f * Mathf.PI;
        float rawSine = Mathf.Sin(cycles * tau);
        movementFactor = rawSine / 2f + 0.5f; 

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
