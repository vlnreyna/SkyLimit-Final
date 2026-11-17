using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform objetivo;
    public float speedCamera = 0.025f;
    public Vector3 displacement;

    private float alturaMaxima;
    private float xFija;

    private void Start()
    {
        alturaMaxima = transform.position.y;
        xFija = transform.position.x;
    }

    private void LateUpdate()
    {
        float yDeseada = objetivo.position.y + displacement.y;

        if (yDeseada > alturaMaxima)
        {
            alturaMaxima = yDeseada;
        }

        Vector3 desiredPosition = new Vector3(xFija, alturaMaxima, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, speedCamera);
    }

    public void ReiniciarEn(Vector3 posicion)
    {
        alturaMaxima = posicion.y + displacement.y;
        transform.position = new Vector3(xFija, alturaMaxima, transform.position.z);
    }
}