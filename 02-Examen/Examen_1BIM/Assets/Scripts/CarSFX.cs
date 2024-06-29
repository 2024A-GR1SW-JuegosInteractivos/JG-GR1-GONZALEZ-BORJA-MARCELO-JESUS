using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSFX : MonoBehaviour
{
    public AudioSource tireScreeching;
    public AudioSource engineAudioSource;
    public AudioSource carHit;

    float desiredEnginePitch = 0.5f;
    float desiredTirePitch = 0.5f;

    //Componente
    CarController carController;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateTiresScreechingSFX();
    }

    void UpdateEngineSFX()
    {
        float velocityManitude = carController.GetVelocityMagnitude();

        // aumentar el volumen 
        float desireEngineVolumen = Mathf.Abs(velocityManitude * 0.2f);

        // volumen mínimo
        desireEngineVolumen = Mathf.Clamp(desireEngineVolumen, 0.2f, 1.0f);

        engineAudioSource.volume =  Mathf.Lerp(engineAudioSource.volume, desireEngineVolumen, Time.deltaTime * 10);
        // para mayor variacion
        desiredEnginePitch = velocityManitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
    }

    void UpdateTiresScreechingSFX() {
        if (carController.IsTireScreeching(out float lateralVelocity, out bool isBreaking))
        {
            if (isBreaking)
            {
                tireScreeching.volume = Mathf.Lerp(tireScreeching.volume, 0.3f, Time.deltaTime * 5);
                desiredTirePitch = Mathf.Lerp(desiredTirePitch, 0.3f, Time.deltaTime * 5);
            }
            else
            {
                tireScreeching.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                desiredTirePitch = Mathf.Abs(lateralVelocity) * 0.01f;
            }
        }
        else
        {
            tireScreeching.volume = Mathf.Lerp(tireScreeching.volume, 0, Time.deltaTime * 10);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        float relativeVelocity = collision.relativeVelocity.magnitude;
        float volume = relativeVelocity * 0.1f;

        carHit.volume = volume;
        carHit.pitch = Random.Range(0.95f, 1.05f);

        if (!carHit.isPlaying)
        {
            carHit.Play();
        }
    }
}
