using UnityEngine;

public class Invencibilidad : MonoBehaviour
{
    // Segundos que dura el power-up desde que se recoge
    public float duracion = 5f;

    private float tiempoRestante;

    public bool Activo
    {
        get { return tiempoRestante > 0f; }
    }

    public float TiempoRestante
    {
        get { return tiempoRestante; }
    }

    // Lo llama ObjetoInvencibilidad al recoger el power-up (si ya estaba activo, reinicia el tiempo)
    public void Activar()
    {
        tiempoRestante = duracion;
    }

    private void Update()
    {
        if (tiempoRestante > 0f)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0f)
            {
                tiempoRestante = 0f;
            }
        }
    }
}