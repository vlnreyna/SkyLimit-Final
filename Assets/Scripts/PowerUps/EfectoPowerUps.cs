using UnityEngine;

public class EfectoPowerUps : MonoBehaviour
{
    public Color colorInvencible = new Color(0.4f, 0.7f, 1f, 1f);   // azul
    public Color colorDobleSalto = new Color(1f, 0.35f, 0.35f, 1f); // rojo
    public Color colorAmbos = new Color(0.7f, 0.4f, 1f, 1f);        // morado

    private SpriteRenderer sr;
    private DobleSalto dobleSalto;
    private Invencibilidad invencibilidad;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (sr == null) return;

        // Los componentes se añaden en runtime al recoger el power-up, así que se buscan si aún no existen
        if (dobleSalto == null) dobleSalto = GetComponent<DobleSalto>();
        if (invencibilidad == null) invencibilidad = GetComponent<Invencibilidad>();

        bool inmune = invencibilidad != null && invencibilidad.Activo;
        bool doble = dobleSalto != null && dobleSalto.Activo;

        if (inmune && doble) sr.color = colorAmbos;
        else if (inmune) sr.color = colorInvencible;
        else if (doble) sr.color = colorDobleSalto;
        else sr.color = Color.white;
    }
}