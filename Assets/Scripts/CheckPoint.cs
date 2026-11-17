/*using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool checkpointActivo = false;
    public sprite spriteActivo;
    private spriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<spriteRenderer>();
    }

    // Update is called once per frame
    private void OnTgerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")&& !checkpointActivo){
            ActivarCheckpoint();
        }
    }

    void ActivarCheckpoint(){
        if(GameManager.Instance != null){
            GameManager.Instance.ActualizarCheckpoint(transform.position, this);
        }
        checkpointActivo = true;

        if(spriteRenderer != null && spriteActivo != null){
            spriteRenderer.sprite = spriteActivo;
        }
    }
}*/

using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Sprite que se muestra cuando el checkpoint ya está activado (opcional)
    public Sprite spriteActivado;

    // Referencia al SpriteRenderer de este objeto, para poder cambiar su imagen
    private SpriteRenderer spriteRenderer;

    // Evita que el checkpoint se active varias veces
    private bool activado;

    // Se ejecuta una vez al crearse el objeto: guarda la referencia al SpriteRenderer
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Unity lo llama cuando otro collider entra en este trigger (Is Trigger marcado)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si ya estaba activado, no hace nada
        if (activado) return;

        // Busca el PlayerRespawn en el objeto que ha entrado o en su padre
        PlayerRespawn jugador = other.GetComponentInParent<PlayerRespawn>();

        // Si lo que entró no es el jugador, sale
        if (jugador == null) return;

        // Guarda la posición de este checkpoint como punto de reaparición
        jugador.EstablecerCheckpoint(transform.position);
        activado = true;

        // Avisa al generador para que no borre la plataforma ni lo que hay encima
        PlatformGenerator generador = FindFirstObjectByType<PlatformGenerator>();
        if (generador != null)
        {
            generador.ProtegerDesde(transform.position.y);
        }

        // Cambia el sprite al de "activado" si hay uno asignado
        if (spriteRenderer != null && spriteActivado != null)
        {
            spriteRenderer.sprite = spriteActivado;
        }
    }
}
