using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Componentes
    CarController CarController;
    TrailRenderer trailRenderer;

    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        //Get the top down car controller
        CarController = GetComponentInParent<CarController>();
        //Get the trail renderer component..
        trailRenderer = GetComponent<TrailRenderer>();
        //Set the trail renderer to not emit in the start.
        trailRenderer.emitting = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(CarController.IsTireScreeching(out float lateralVelocity, out bool isBreaking))
        {
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }
}
