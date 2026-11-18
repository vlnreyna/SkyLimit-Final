using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Camera camara;
    public GameObject enemigoPrefab;

    public int enemigosIniciales = 2;
    public int incrementoPorOleada = 1;
    public int maximoPorOleada = 10;

    public float alturaPrimeraOleada = 15f;
    public float alturaEntreOleadas = 20f;

    public float limiteX = 2.5f;
    public float margenSobreCamara = 3f;
    public float rangoVertical = 6f;
    public float margenBorrado = 20f;

    private float siguienteOleadaY;
    private int oleada;
    private List<GameObject> activos = new List<GameObject>();

    private void Start()
    {
        siguienteOleadaY = camara.transform.position.y + alturaPrimeraOleada;
    }

    private void Update()
    {
        if (enemigoPrefab == null) return;

        if (camara.transform.position.y >= siguienteOleadaY)
        {
            GenerarOleada();
            oleada++;
            siguienteOleadaY += alturaEntreOleadas;
        }

        float suelo = camara.transform.position.y - camara.orthographicSize - margenBorrado;

        for (int i = activos.Count - 1; i >= 0; i--)
        {
            if (activos[i] == null)
            {
                activos.RemoveAt(i);
            }
            else if (activos[i].transform.position.y < suelo)
            {
                Destroy(activos[i]);
                activos.RemoveAt(i);
            }
        }
    }

    private void GenerarOleada()
    {
        int cantidad = Mathf.Min(enemigosIniciales + oleada * incrementoPorOleada, maximoPorOleada);
        float techo = camara.transform.position.y + camara.orthographicSize + margenSobreCamara;

        for (int i = 0; i < cantidad; i++)
        {
            float x = Random.Range(-limiteX, limiteX);
            float y = techo + Random.Range(0f, rangoVertical);

            GameObject nuevo = Instantiate(enemigoPrefab, new Vector3(x, y, 0f), Quaternion.identity);
            activos.Add(nuevo);
        }
    }
}
