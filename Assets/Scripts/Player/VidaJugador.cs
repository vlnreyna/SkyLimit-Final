using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    public int golpesParaMorir = 2;

    private int golpes;
    private PlayerRespawn respawn;

    private void Start()
    {
        respawn = GetComponent<PlayerRespawn>();
    }

    public void RecibirGolpe()
    {
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
