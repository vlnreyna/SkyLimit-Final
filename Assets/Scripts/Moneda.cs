using UnityEngine;

public class Moneda : MonoBehaviour{


    public int valor = 1;

    //Verifica que haya una colision
    private void OnTriggerEnter2D(Collider2D collision){

        if(collision.CompareTag("Player")){
            GameManager.Instance.SumarMoneda(valor);
            Destroy(gameObject);
        }
    }
}