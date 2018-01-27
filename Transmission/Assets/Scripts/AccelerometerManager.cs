using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerManager : MonoBehaviour
{
    [SerializeField] private float _accelerometerMultiplier = 500;
    public float Acceleration { get; set; }

    public void Update ()
    {
        Acceleration = Input.acceleration.x * Time.deltaTime * _accelerometerMultiplier;

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetKey(KeyCode.A))
                Acceleration = -0.2f * Time.deltaTime * _accelerometerMultiplier;
            else if (Input.GetKey(KeyCode.D))
                Acceleration = 0.2f * Time.deltaTime * _accelerometerMultiplier;
            else
                Acceleration = 0;
        }
    }
}
