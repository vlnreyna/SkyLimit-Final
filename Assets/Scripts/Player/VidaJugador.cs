using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    public int golpesParaMorir = 2;

    private int golpes;
    private PlayerRespawn respawn;
    private Invencibilidad invencibilidad;

    private void Start()
    {
        respawn = GetComponent<PlayerRespawn>();
    }

    public void RecibirGolpe()
    {
        if (invencibilidad == null)
        {
            invencibilidad = GetComponent<Invencibilidad>();
        }

        if (invencibilidad != null && invencibilidad.Activo) return;

        golpes++;

        if (golpes >= golpesParaMorir)
        {
            golpes = 0;

            if (respawn != null)
            {
                respawn.Reaparecer();
            }
        }
    }
}
