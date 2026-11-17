using UnityEngine;

public class DobleSalto : MonoBehaviour
{
    // Segundos que dura el power-up desde que se recoge
    public float duracion = 10f;

    private float tiempoRestante;
    private bool saltoExtraDisponible;

    public bool Activo
    {
        get { return tiempoRestante > 0f; }
    }

    public float TiempoRestante
    {
        get { return tiempoRestante; }
    }

    // Lo llama ObjetoDobleSalto al recoger el power-up (si ya estaba activo, reinicia el tiempo)
    public void Activar()
    {
        tiempoRestante = duracion;
        saltoExtraDisponible = true;
    }

    // Lo llama Player al aterrizar en una plataforma: vuelve a tener el salto extra
    public void Recargar()
    {
        saltoExtraDisponible = Activo;
    }

    // Lo llama Player cuando se pulsa salto en el aire. Devuelve true si se puede hacer el salto extra.
    public bool IntentarSaltoExtra()
    {
        if (!Activo || !saltoExtraDisponible) return false;

        saltoExtraDisponible = false;
        return true;
    }

    private void Update()
    {
        if (tiempoRestante > 0f)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0f)
            {
                tiempoRestante = 0f;
                saltoExtraDisponible = false;
            }
        }
    }
}
