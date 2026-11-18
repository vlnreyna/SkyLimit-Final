
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public Camera camara;

    // Prefabs de plataformas que se generarán aleatoriamente
    public GameObject[] plataformasPrefab;

    public float limiteX = 2.5f;
    public float separacionMinX = 2f;
    public float margenGeneracion = 6f;
    public float margenBorrado = 15f;

    // Dificultad progresiva
    public float distanciaMinY = 2f;
    public float distanciaMaxY = 3.5f;
    public float distanciaMinYFinal = 3f;
    public float distanciaMaxYFinal = 4.5f;
    public float alturaMaxDificultad = 600f;

    // Checkpoints
    public GameObject checkpointPrefab;
    public int plataformasPorCheckpoint = 10;
    public float offsetCheckpointY = 1f;

    // Monedas
    public GameObject monedaPrefab;
    [Range(0f, 1f)] public float probabilidadMoneda = 0.5f;
    public float offsetMonedaY = 1f;

    // Plataformas móviles
    [Range(0f, 1f)] public float probabilidadMovil = 0.3f;
    [Range(0f, 1f)] public float probabilidadVertical = 0.35f;
    public int plataformasEstaticasIniciales = 4;

    // Power-up de doble salto
    public GameObject powerUpPrefab;
    [Range(0f, 1f)] public float probabilidadPowerUp = 0.12f;
    public float offsetPowerUpY = 1.1f;

    private float siguienteY;
    private float ultimaX;
    private int contador;

    private List<GameObject> activas = new List<GameObject>();

    // Power-up de invencibilidad
    public GameObject InmunidadPrefab;
    [Range(0f, 1f)] public float probabilidadInmunidad = 0.12f;
    public float offsetInmunidadY = 1.1f;

    // Protección del último checkpoint activado
    private bool hayProtegida;
    private float protegidaY;

    private void Start()
    {
        siguienteY = camara.transform.position.y;
        ultimaX = 0f;
    }

    // Lo llama el checkpoint al activarse
    public void ProtegerDesde(float y)
    {
        if (!hayProtegida || y > protegidaY)
        {
            hayProtegida = true;
            protegidaY = y;
        }

        Debug.Log($"Checkpoint y={y:F0} a los {Time.timeSinceLevelLoad:F0} s");
    }

    private void Update()
    {
        if (plataformasPrefab.Length == 0) return;

        // Altura hasta la que se generan plataformas
        float techo = camara.transform.position.y
                      + camara.orthographicSize
                      + margenGeneracion;

        while (siguienteY < techo)
        {
            // Calcula la dificultad según la altura
            float t = Mathf.Clamp01(siguienteY / alturaMaxDificultad);

            float minY = Mathf.Max(
                Mathf.Lerp(distanciaMinY, distanciaMinYFinal, t),
                1.5f
            );

            float maxY = Mathf.Max(
                Mathf.Lerp(distanciaMaxY, distanciaMaxYFinal, t),
                minY
            );

            // Calcula la siguiente altura
            siguienteY += Random.Range(minY, maxY);

            // Busca una posición X suficientemente alejada
            float x = ultimaX;
            bool valida = false;

            for (int i = 0; i < 10; i++)
            {
                x = Random.Range(-limiteX, limiteX);

                if (Mathf.Abs(x - ultimaX) >= separacionMinX)
                {
                    valida = true;
                    break;
                }
            }

            // Si no encuentra una posición válida, fuerza el desplazamiento
            if (!valida)
            {
                float desplazamiento = ultimaX > 0f
                    ? -separacionMinX
                    : separacionMinX;

                x = Mathf.Clamp(
                    ultimaX + desplazamiento,
                    -limiteX,
                    limiteX
                );
            }

            ultimaX = x;

            // Cuenta las plataformas generadas
            contador++;

            // Comprueba si corresponde generar un checkpoint
            bool llevaCheckpoint = checkpointPrefab != null
                && plataformasPorCheckpoint > 0
                && contador % plataformasPorCheckpoint == 0;

            // Selecciona un prefab aleatorio del array
            GameObject prefab = plataformasPrefab[
                Random.Range(0, plataformasPrefab.Length)
            ];

            // Genera la plataforma
            GameObject nueva = Instantiate(
                prefab,
                new Vector3(x, siguienteY, 0f),
                Quaternion.identity
            );

            activas.Add(nueva);

            // Genera plataformas móviles aleatoriamente
            if (!llevaCheckpoint
                && contador > plataformasEstaticasIniciales
                && Random.value < probabilidadMovil)
            {
                PlataformaMovil movil = nueva.AddComponent<PlataformaMovil>();
                movil.vertical = Random.value < probabilidadVertical;
            }

            // Genera el checkpoint como hijo de la plataforma
            if (llevaCheckpoint)
            {
                GameObject cp = Instantiate(
                    checkpointPrefab,
                    nueva.transform
                );

                cp.transform.position = nueva.transform.position
                    + new Vector3(0f, offsetCheckpointY, 0f);
            }
            // Genera un power-up aleatoriamente
            else if (powerUpPrefab != null
                     && Random.value < probabilidadPowerUp)
            {
                Vector3 posicionPowerUp = new Vector3(
                    x,
                    siguienteY + offsetPowerUpY,
                    0f
                );

                GameObject powerUp = Instantiate(
                    powerUpPrefab,
                    posicionPowerUp,
                    Quaternion.identity
                );

                activas.Add(powerUp);
            }
            // Genera un power-up2 aleatoriamente
            else if (InmunidadPrefab != null
                     && Random.value < probabilidadInmunidad)
            {
                Vector3 posicionInmunidad = new Vector3(
                    x,
                    siguienteY + offsetInmunidadY,
                    0f
                );

                GameObject powerUp2 = Instantiate(
                    InmunidadPrefab,
                    posicionInmunidad,
                    Quaternion.identity
                );

                activas.Add(powerUp2);
            }
            // Genera una moneda aleatoriamente
            else if (monedaPrefab != null
                     && Random.value < probabilidadMoneda)
            {
                Vector3 posicionMoneda = new Vector3(
                    x,
                    siguienteY + offsetMonedaY,
                    0f
                );

                GameObject moneda = Instantiate(
                    monedaPrefab,
                    posicionMoneda,
                    Quaternion.identity
                );

                activas.Add(moneda);
            }
        }

        // Altura por debajo de la cual se eliminan los objetos
        float suelo = camara.transform.position.y
                      - camara.orthographicSize
                      - margenBorrado;

        // Protege los objetos situados por encima del último checkpoint
        if (hayProtegida)
        {
            suelo = Mathf.Min(suelo, protegidaY - 3f);
        }

        // Elimina las plataformas y objetos que quedan demasiado abajo
        for (int i = activas.Count - 1; i >= 0; i--)
        {
            if (activas[i] == null)
            {
                activas.RemoveAt(i);
            }
            else if (activas[i].transform.position.y < suelo)
            {
                Destroy(activas[i]);
                activas.RemoveAt(i);
            }
        }
    }
}
 