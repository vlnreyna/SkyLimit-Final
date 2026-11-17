/*using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public Camera camara;
    public GameObject[] plataformasPrefab;
    public float limiteX = 2.5f;
    public float distanciaMinY = 2f;
    public float distanciaMaxY = 3f;
    public float margenGeneracion = 10f;
    public float margenBorrado = 30f;

    private float siguienteY;
    private List<GameObject> activas = new List<GameObject>();

    private void Start()
    {
        siguienteY = camara.transform.position.y + camara.orthographicSize;
    }

    private void Update()
    {
        if (plataformasPrefab.Length == 0) return;

        float techo = camara.transform.position.y + camara.orthographicSize + margenGeneracion;
        float minY = Mathf.Max(distanciaMinY, 0.5f);
        float maxY = Mathf.Max(distanciaMaxY, minY);

        while (siguienteY < techo)
        {
            siguienteY += Random.Range(minY, maxY);
            float x = Random.Range(-limiteX, limiteX);
            GameObject prefab = plataformasPrefab[Random.Range(0, plataformasPrefab.Length)];
            GameObject nueva = Instantiate(prefab, new Vector3(x, siguienteY, 0f), Quaternion.identity);
            activas.Add(nueva);
        }

        float suelo = camara.transform.position.y - camara.orthographicSize - margenBorrado;

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
}*/
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public Camera camara;
    public GameObject[] plataformasPrefab;
    public float limiteX = 2.5f;
    public float distanciaMinY = 2f;
    public float distanciaMaxY = 3.5f;
    public float separacionMinX = 2f;
    public float margenGeneracion = 6f;
    public float margenBorrado = 15f;

    // Prefab del checkpoint y cada cuántas plataformas aparece
    public GameObject checkpointPrefab;
    public int plataformasPorCheckpoint = 10;
    // Altura sobre la plataforma a la que se coloca el checkpoint
    public float offsetCheckpointY = 1f;

    private float siguienteY;
    private float ultimaX;
    private int contador;
    private List<GameObject> activas = new List<GameObject>();

    // Altura desde la que NO se borra nada (la del último checkpoint activado)
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
        hayProtegida = true;
        protegidaY = y;
    }

    private void Update()
    {
        if (plataformasPrefab.Length == 0) return;

        // Altura hasta la que hay que tener plataformas generadas
        float techo = camara.transform.position.y + camara.orthographicSize + margenGeneracion;
        float minY = Mathf.Max(distanciaMinY, 1.5f);
        float maxY = Mathf.Max(distanciaMaxY, minY);

        while (siguienteY < techo)
        {
            // Sube una distancia aleatoria respecto a la plataforma anterior
            siguienteY += Random.Range(minY, maxY);

            // Busca una X que esté lo bastante lejos de la anterior (10 intentos)
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

            // Si no encontró una, la fuerza hacia el lado contrario
            if (!valida)
            {
                float desplazamiento = ultimaX > 0f ? -separacionMinX : separacionMinX;
                x = Mathf.Clamp(ultimaX + desplazamiento, -limiteX, limiteX);
            }

            ultimaX = x;

            // Crea la plataforma con un prefab aleatorio
            GameObject prefab = plataformasPrefab[Random.Range(0, plataformasPrefab.Length)];
            GameObject nueva = Instantiate(prefab, new Vector3(x, siguienteY, 0f), Quaternion.identity);
            activas.Add(nueva);

            contador++;

            // Cada N plataformas, añade un checkpoint como hijo de la plataforma
            if (checkpointPrefab != null && plataformasPorCheckpoint > 0 && contador % plataformasPorCheckpoint == 0)
            {
                GameObject cp = Instantiate(checkpointPrefab, nueva.transform);
                cp.transform.position = nueva.transform.position + new Vector3(0f, offsetCheckpointY, 0f);
            }
        }

        // Altura por debajo de la cual se borran plataformas
        float suelo = camara.transform.position.y - camara.orthographicSize - margenBorrado;

        // Si hay un checkpoint activado, no se borra nada por encima de él
        if (hayProtegida)
        {
            suelo = Mathf.Min(suelo, protegidaY - 1f);
        }

        for (int i = activas.Count - 1; i >= 0; i--)
        {
            if (activas[i] == null)
            {
                activas.RemoveAt(i);
            }
            else if (activas[i].transform.position.y < suelo)
            {
                // Destruir la plataforma destruye también su checkpoint hijo
                Destroy(activas[i]);
                activas.RemoveAt(i);
            }
        }
    }
}
