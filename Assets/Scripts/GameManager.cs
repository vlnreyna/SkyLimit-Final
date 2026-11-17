using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager Instance;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button Button;


    //Sirve como separacion Visual
    [Header("Monedas")]
    public int monedas = 0;
    public TextMeshProUGUI textoMonedas;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // solo si lo quieres entre escenas
    }
    public void SumarMoneda(int cantidad){
        monedas += cantidad;
        ActualizarMonedasUI();
        GuardarMonedas();
    }


    void ActualizarMonedasUI(){
        if(textoMonedas != null)
            textoMonedas.text = "x"+ monedas.ToString();
    }

    void GuardarMonedas(){
        PlayerPrefs.SetInt("Monedas", monedas);
        PlayerPrefs.Save();
    }

    void CargarMonedas(){
        monedas= PlayerPrefs.GetInt("Monedas", 0);
    }

    [Header("CheckPoint")]
    public Vector3 checkpointPosicion;
    private CheckPoint checkpointActual;

    void Start(){

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            checkpointPosicion = player.transform.position;
        }
    }

    public void IrAlMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ActivarCheckpoint(Vector3 nuevaPosicion, CheckPoint nuevoCheckpoint){
        checkpointPosicion = nuevaPosicion;
        checkpointActual = nuevoCheckpoint;
    }

    public void RespawnJugador(){
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine(){
        yield return new WaitForSeconds(1f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            player.GetComponent<PlayerRespawn>().Reaparecer();
        }
    }
}