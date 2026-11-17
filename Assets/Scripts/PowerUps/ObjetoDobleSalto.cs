using UnityEngine;

public class ObjetoDobleSalto : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    { 
        if (!other.CompareTag("Player")) return;

        Player jugador = other.GetComponentInParent<Player>();
        if (jugador == null) return;

        // Si el jugador aún no tiene el componente, se le añade
        DobleSalto doble = jugador.GetComponent<DobleSalto>();
        if (doble == null)
        {
            doble = jugador.gameObject.AddComponent<DobleSalto>();
        }

        doble.Activar();
        Destroy(gameObject);
    }
}