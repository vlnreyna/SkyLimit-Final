using UnityEngine;

public class ObjetoInvencibilidad : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Player jugador = other.GetComponentInParent<Player>();
        if (jugador == null) return;

        // Si el jugador aún no tiene el componente, se le añade
        Invencibilidad invencible = jugador.GetComponent<Invencibilidad>();
        if (invencible == null)
        {
            invencible = jugador.gameObject.AddComponent<Invencibilidad>();
        }

        invencible.Activar();
        Destroy(gameObject);
    }
}