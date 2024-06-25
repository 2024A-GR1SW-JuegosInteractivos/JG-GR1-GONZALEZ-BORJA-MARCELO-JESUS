using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // variables
    [SerializeField] float steerSpeed = 200f;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float addSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float steerAmount = Input.GetAxis("Horizontal") * steerSpeed * -1 * Time.deltaTime;
        float moveAmount = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        // rotar el objeto
        transform.Rotate(0, 0, steerAmount);
        // mover el objeto
        transform.Translate(0,  moveAmount, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("rapido"))
        {
            Debug.Log("Rapido");
            moveSpeed += addSpeed;
        }

        if (collision.gameObject.CompareTag("lento"))
        {
            Debug.Log("Lento");
            moveSpeed -= addSpeed;
        }
    }

}
