using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car settings")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 150f;
    public float turnFactor = 190f;
    public float maxSpeed = 250;

    //local
    float accelerationInput = 0.0f;
    float steeringInput = 0.0f;

    float rotationAngle = 0.0f;

    float velocityVsup = 0;

    //Componente
    Rigidbody2D carRigidbody2D;

    // Awake is called when the script is being loaded
    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();
    }

    void ApplyEngineForce()
    {
        // cuan rápido en base a la dirección de la velocidad 
        velocityVsup = Vector2.Dot(transform.up, carRigidbody2D.velocity);

        // limite a la velocidad maxima
        if (velocityVsup > maxSpeed && accelerationInput > 0){
            return;
        }

        // 50% velocidad en reversa
        if (velocityVsup < -maxSpeed * 0.5f && accelerationInput < 0)
        {
            return;
        }

        // limite de velocidad para las direcciones mientras se acelera
        if (carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
        {
            return;
        }

        // aplica resistencia o arrastre si no se acelera para detener el auto
        if (accelerationInput == 0){
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        }else{
            carRigidbody2D.drag = 0;
        }

        // Fuerza del motor
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        // Aplica la fuerza para mover el auto
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        // limite de velocidad al girar para que el auto no gire indefinidamente
        // a > divisor hay < angulo, a < divisor > angulo
        float minSpeedBeforeAllowTurningFactor = (carRigidbody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        // angulo de rotacion
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        // aplica la dirección de giro
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    float GetLateralVelocity()
    {
        // velocidad que el auto tiene hacia los lados
        return Vector2.Dot(transform.right, carRigidbody2D.velocity);
    }

    // Define si las llantas deben dejar o no marcas en el suelo
    public bool IsTireScreeching(out float lateralVelocity, out bool isBreaking)
    {
        lateralVelocity = GetLateralVelocity();
        isBreaking = false;

        // si se mueve hacia adeltan y se frena.
        if (accelerationInput < 0 && velocityVsup > 0)
        {
            isBreaking = true;
            return true;
        }
        
        // Si hay un movimiento brusco o grande hacia lo lados
        if (Mathf.Abs(GetLateralVelocity()) > 2.0f)
        {
            return true;
        }
            
        return false;
    }

    // Forma parte de la física del auto para hacer dirft
    // se buscar remover parte de la fuerza ortogonal (lateral)
    // para mover el auto y que pueda girar de manera correcta.
    // Ya que, si no se la remueve el auto flotaria.
    // Si se remueve todo el auto no giraria
    void KillOrthogonalVelocity()
    {
        // velocidad hacia adelante
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        // velocidad lateral
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);
        // cambio de la velocidad basdo en las velocidades y el factor de drift (para el correcto giro)
        carRigidbody2D.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public float GetVelocityMagnitude()
    {
        return carRigidbody2D.velocity.magnitude;
    }
}
