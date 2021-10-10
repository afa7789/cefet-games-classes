using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float atrasoTurno = .1f;
    public int turnoDeQuem; //0-vitima; 1-jogador; 2-monstro
    public bool processando;

    public static GameManager instancia = null;
    public BoardManager boardScript;

    public GameObject jogador;
    public GameObject vitima;

    private int turno = 0;

	void Awake () {

        //instanciacao do gamemanager
        if (instancia == null)
            instancia = this;
        else if (instancia != this)
            Destroy(gameObject);

        //instanciacao do script boardmanager
        boardScript = GetComponent<BoardManager>();

        ComecarJogo();
	}

    void Start() {

        jogador = GameObject.Find("Jogador");
        vitima = GameObject.Find("Vitima");
    }

    private void ComecarJogo() {

        boardScript.MontarCena();

        turnoDeQuem = 1;
    }
	
    public void GameOver() {

        enabled = false;
    }

	void Update () {

        Debug.Log("turno: " + turnoDeQuem);

        if (boardScript.montandoLabirinto || processando || turnoDeQuem == 1) return;

        if(turnoDeQuem == 0)
            StartCoroutine(TurnoDaVitima());
        else if (turnoDeQuem == 2)
            StartCoroutine(TurnoDoMonstro());
	}

    IEnumerator TurnoDaVitima() {

        Debug.Log("turno da vitima");

        turno++;

        processando = true;
        yield return new WaitForSeconds(atrasoTurno);

        Debug.Log("prestes a chamar ExecutarTurno()");

        vitima.GetComponent<Vitima>().ExecutarTurno();

        turnoDeQuem = 1;

        yield return new WaitForSeconds(atrasoTurno);
        processando = false;
    }

    IEnumerator TurnoDoMonstro() {

        processando = true;
        yield return new WaitForSeconds(atrasoTurno);

        turnoDeQuem = 1;

        processando = false;
    }
}
