using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    //Components
    CarController CarController;

    // Awake is called when the script is being loaded
    void Awake()
    {
        CarController = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal") * Time.deltaTime;
        inputVector.y = Input.GetAxis("Vertical") * Time.deltaTime;

        CarController.SetInputVector(inputVector);
    }
}
