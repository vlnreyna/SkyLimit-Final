using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public float velocidadMin = 1f;
    public float velocidadMax = 2.5f;
    public float limiteIzquierdo = -2.5f;
    public float limiteDerecho = 2.5f;

    // Si está marcado, sube y baja en vez de ir de lado a lado
    public bool vertical = false;
    public float rangoVertical = 1f;   // distancia que sube y baja desde donde nace

    private float velocidad;
    private int direccion;
    private Transform jugador;
    private float limiteInferior;
    private float limiteSuperior;
    private Collider2D zona;

    private void Start()
    {
        velocidad = Random.Range(velocidadMin, velocidadMax);
        direccion = Random.value < 0.5f ? -1 : 1;

        limiteInferior = transform.position.y - rangoVertical;
        limiteSuperior = transform.position.y + rangoVertical;
        zona = GetComponentInChildren<Collider2D>();
    }

    private void Update()
    {
        DetectarJugador();

        float xActual = transform.position.x;
        float yActual = transform.position.y;
        float xNueva = xActual;
        float yNueva = yActual;

        if (vertical)
        {
            yNueva = yActual + direccion * velocidad * Time.deltaTime;

            if (yNueva > limiteSuperior)
            {
                yNueva = limiteSuperior;
                direccion = -1;
            }
            else if (yNueva < limiteInferior)
            {
                yNueva = limiteInferior;
                direccion = 1;
            }
        }
        else
        {
            xNueva = xActual + direccion * velocidad * Time.deltaTime;

            if (xNueva > limiteDerecho)
            {
                xNueva = limiteDerecho;
                direccion = -1;
            }
            else if (xNueva < limiteIzquierdo)
            {
                xNueva = limiteIzquierdo;
                direccion = 1;
            }
        }

        Vector3 desplazamiento = new Vector3(xNueva - xActual, yNueva - yActual, 0f);
        transform.position = new Vector3(xNueva, yNueva, transform.position.z);

        if (jugador != null)
        {
            jugador.position += desplazamiento;
        }
    }

    // El collider está en el hijo "jumpingZone" (sin Rigidbody), así que OnCollisionEnter2D no llega
    // a este script. En su lugar se mira si hay un jugador justo encima de la plataforma.
    private void DetectarJugador()
    {
        jugador = null;
        if (zona == null) return;

        Bounds b = zona.bounds;
        Vector2 centro = new Vector2(b.center.x, b.max.y + 0.1f);
        Vector2 tamano = new Vector2(b.size.x, 0.2f);

        Collider2D[] golpes = Physics2D.OverlapBoxAll(centro, tamano, 0f);

        foreach (Collider2D golpe in golpes)
        {
            if (golpe.CompareTag("Player") && golpe.bounds.min.y >= b.max.y - 0.15f)
            {
                jugador = golpe.transform;
            }
        }
    }
}