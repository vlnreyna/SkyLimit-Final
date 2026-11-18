using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5.0f;
    public float speed = 2.0f;

    public float tiempoVolando = 4f;
    public float tiempoPersiguiendo = 3f;
    public float radioVuelo = 3f;
    public float velocidadVuelo = 1.5f;

    public int vidas = 2;
    public float rebote = 8f;
    public float tiempoEntreContactos = 1f;

    private Rigidbody2D rb;
    private Collider2D col;
    private Vector2 movement;
    private Vector2 centroVuelo;
    private Vector2 objetivoVuelo;
    private bool persiguiendo;
    private float cambioFase;
    private float proximoContacto;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        if (player == null)
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");

            if (jugador != null)
            {
                player = jugador.transform;
            }
        }

        centroVuelo = rb.position;
        ElegirObjetivoVuelo();
        cambioFase = Time.time + tiempoVolando;
    }

    void Update()
    {
        if (player == null) return;

        if (Time.time >= cambioFase)
        {
            persiguiendo = !persiguiendo;
            cambioFase = Time.time + (persiguiendo ? tiempoPersiguiendo : tiempoVolando);

            if (!persiguiendo)
            {
                centroVuelo = rb.position;
                ElegirObjetivoVuelo();
            }
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (persiguiendo && distanceToPlayer < detectionRadius)
        {
            Vector2 direction = ((Vector2)player.position - rb.position).normalized;
            movement = direction * speed;
        }
        else
        {
            Vector2 haciaObjetivo = objetivoVuelo - rb.position;

            if (haciaObjetivo.magnitude < 0.2f)
            {
                ElegirObjetivoVuelo();
                haciaObjetivo = objetivoVuelo - rb.position;
            }

            movement = haciaObjetivo.normalized * velocidadVuelo;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }

    void ElegirObjetivoVuelo()
    {
        objetivoVuelo = centroVuelo + Random.insideUnitCircle * radioVuelo;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Contacto(collision, true);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Contacto(collision, false);
    }

    void Contacto(Collision2D collision, bool inicio)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (Time.time < proximoContacto) return;

        bool desdeArriba = collision.collider.bounds.min.y >= col.bounds.center.y;

        if (desdeArriba)
        {
            if (!inicio) return;

            proximoContacto = Time.time + tiempoEntreContactos;

            Rigidbody2D rbJugador = collision.rigidbody;

            if (rbJugador != null)
            {
                rbJugador.linearVelocityY = rebote;
            }

            vidas--;

            if (vidas <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            proximoContacto = Time.time + tiempoEntreContactos;

            VidaJugador vida = collision.gameObject.GetComponent<VidaJugador>();

            if (vida != null)
            {
                vida.RecibirGolpe();
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
