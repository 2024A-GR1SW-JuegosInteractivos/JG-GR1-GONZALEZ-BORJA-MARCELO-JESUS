using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderEjemplo : MonoBehaviour
{
    [SerializeField] float delayDestroy = 0.5f;
    [SerializeField] Color32 tienePaqueteColor = new( 255, 0, 0, 255);
    [SerializeField] Color32 notienePaqueteColor = new( 255, 255, 255, 255);
    private SpriteRenderer spriteRenderer; //null
    private bool tienepaquete; //false

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ouch!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        if (collision.gameObject.CompareTag("paquete") && !tienepaquete)
        {
            Debug.Log("Recoger Paquete");
            tienepaquete = true;
            spriteRenderer.color = tienePaqueteColor;
            Destroy(collision.gameObject, delayDestroy);
        }

        if (collision.gameObject.CompareTag("Cliente") && tienepaquete)
        {
            Debug.Log("Entregar Paquete");
            tienepaquete = false;
            spriteRenderer.color = notienePaqueteColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
