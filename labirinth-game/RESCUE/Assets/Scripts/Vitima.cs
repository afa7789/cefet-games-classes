using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Vitima : Bichinho {

    private Animator animator;

    public int estado;
    /*
    representa a maquina de estados da AI
    00 - andando aleatoriamente
    01 - fugindo do monstro
    02 - indo ao encontro do jogador
    03 - seguindo o jogador
    04 - indo em direcao a um ponto especifico

    andar aleatoriamente e definir, no ato, um unico passo para uma das quatro (ou menos) direcoes possiveis

    fugir do monstro e correr determinado numero de posicoes tentando sair do campo de visao do monstro,
    procurando a primeira saida perpendicular e nao parando ate chegar ao fim da rota ou ate ter a
    fuga sobrescrita por outra fuga ou por ser resgatado pelo jogador

    ir ao encontro do jogador e tracar o menor caminho possivel para a posicao em que avistou o jogador, nao parando
    ate chegar ao gim da rota ou ate ter o comando sobrescrito por uma fuga ou por ser resgatado pelo jogador

    seguir o jogador e ir para a posicao que o jogador assumia no turno anterior, e so acontece uma vez que a vitima
    e resgatada por ele, ate atingir uma das saidas do labirinto ou ser capturada pelo monstro

    ir em direcao a um ponto especifico e percorrer o menor caminho ate determinado ponto, so sendo interrompido no
    caso de uma fuga, contato visual com o jogador ou resgate pelo jogador
    */

    public Vector2 ultimaPosicaoJogador = new Vector2();
    public List<int> pilhaPercurso = new List<int>();
    /*
    0 - cima
    1 - baixo
    2 - esquerda
    3 - direita
    */

    protected override void Start () {

        animator = GetComponent<Animator>();
        entidade = 0;

        estado = 0;
        base.Start();
    }

    public void ExecutarTurno() {

        Debug.Log("executando turno. estado: " + estado);

        AtualizarEstado(); //atualiza a maquina de estados, mudando o estado atual se for o caso

        Debug.Log("estado atualizado. estado: " + estado);

        ExecutarEstadoAtual(); //executa uma acao de acordo com o estado atual

        Debug.Log("estado executado. estado: " + estado);

        AtualizarUltimaPosicaoJogador(); //atualiza a posicao que o jogador ocupa no momento

        Debug.Log("posicao do jogador atualizada. estado: " + estado);
    }

    private void AtualizarEstado() {

        if(estado != 3) {

            List<int> entidadesVistas = ChecarCampoDeVisao();

            if(estado != 1) {

                if(entidadesVistas.Contains(2)) {
                    estado = 1;
                    BolarRotaDeFuga(GameManager.instancia.boardScript.posicoes[2,0], GameManager.instancia.boardScript.posicoes[2,1]);
                }
                else if(entidadesVistas.Contains(1)) {
                    estado = 2;
                    BolarRotaDeEncontro(GameManager.instancia.boardScript.posicoes[1,0], GameManager.instancia.boardScript.posicoes[1,1]);
                }

                else if(estado != 4) {
                    
                    if(Random.value < 0.1) {
                        estado = 4;
                        EscolherDestinoAleatorio();
                    }
                    else
                        estado = 0;
                }
            }   
        }
    }

    private void ExecutarEstadoAtual() {

        switch(estado) {
            case 0:
                AndarAleatoriamente();
            break;
            case 1:
                SeguirPercurso();
            break;
            case 2:
                SeguirPercurso();
            break;
            case 3:
                SeguirJogador();
            break;
            case 4:
                SeguirPercurso();
            break;
        }
    }

    private int DirecaoDoPonto(int x, int y) {

        int deltaY = y - (int)transform.position.y;
        int deltaX = x - (int)transform.position.x;

        if(deltaY > 0)
            return 0; //o ponto esta acima
        else if(deltaY < 0)
            return 1; //o ponto esta abaixo
        else if(deltaX < 0)
            return 2; //o ponto esta a esquerda
        else
            return 3; //o ponto esta a direita
    }

    private int DistanciaDoPonto(int x, int y) {

        int deltaY = y - (int)transform.position.y;
        int deltaX = x - (int)transform.position.x;

        return deltaX + deltaY;
    }

    private void EmpilharPercurso(int direcao) {

        pilhaPercurso.Add(direcao);
    }

    private void DesempilharPercurso() {

        if (pilhaPercurso.Count > 0)
            pilhaPercurso.RemoveAt(pilhaPercurso.Count - 1);

        if(pilhaPercurso.Count == 0)
            estado = 0;
    }

    private void ExecutarPassoPercurso() {

        if (pilhaPercurso.Count > 0)
            pilhaPercurso.RemoveAt(0);

        if(pilhaPercurso.Count == 0)
            estado = 0;
    }

    private bool SeguirDesviando(int direcao, int xOrigem, int yOrigem, int passos, int maximoPassos) {
        /*
        esta funcao tenta bolar um caminho com o maior numero de curvas possivel, dando um total de passos
        entre metade e a totalidade do valor de maximoPassos
        caso nao haja rota com o minimo de passos necessarios, e dado um passo aleatorio
        */
        if (maximoPassos == 0)
            return true;

        switch(direcao) {

            case 0:
            /*
            para cima, tentando primeiro desviar para a esquerda, depois para a direita
            e em ultimo caso continuar para cima
            */
                if(TentarMover(-1,0)) {

                    EmpilharPercurso(2);
                    if(SeguirDesviando(2, xOrigem-1, yOrigem, passos+1, maximoPassos-1))
                        return true;
                    DesempilharPercurso();
                }
                if(TentarMover(1,0)) {

                    EmpilharPercurso(3);
                    if(SeguirDesviando(3, xOrigem+1, yOrigem, passos+1, maximoPassos-1))
                        return true;
                    DesempilharPercurso();
                }
                if(TentarMover(0,1)) {

                    EmpilharPercurso(0);
                    if(SeguirDesviando(0, xOrigem, yOrigem+1, passos+1, maximoPassos-1))
                        return true;
                }
                if(passos >= maximoPassos) {
                    return true;
                }
                DesempilharPercurso();
                return false;
            break;
            case 1:
            /*
            para baixo, tentando primeiro desviar para a esquerda, depois para a direita
            e em ultimo caso continuar para baixo
            */

                if(TentarMover(-1,0)) {

                    EmpilharPercurso(2);
                    if(SeguirDesviando(2, xOrigem-1, yOrigem, passos+1, maximoPassos-1))
                        return true;
                    DesempilharPercurso();
                }
                if(TentarMover(1,0)) {

                    EmpilharPercurso(3);
                    if(SeguirDesviando(3, xOrigem+1, yOrigem, passos+1, maximoPassos-1))
                        return true;
                    DesempilharPercurso();
                }
                if(TentarMover(0,-1)) {

                    EmpilharPercurso(1);
                    if(SeguirDesviando(1, xOrigem, yOrigem-1, passos+1, maximoPassos-1))
                        return true;
                }
                if(passos >= maximoPassos) {
                    return true;
                }
                DesempilharPercurso();
                return false;

            break;

            case 2:
            /*
            para a esquerda, tentando primeiro desviar para cima, depois para baixo
            e em ultimo caso continuar para a esquerda
            */

                if(TentarMover(0,1)) {

                    EmpilharPercurso(0);
                    if(SeguirDesviando(0, xOrigem, yOrigem+1, passos+1, maximoPassos-1))
                        return true;
                    DesempilharPercurso();
                }
                if(TentarMover(0,-1)) {

                    EmpilharPercurso(1);
                    if(SeguirDesviando(1, xOrigem, yOrigem-1, passos+1, maximoPassos-1))
                        return true;
                    DesempilharPercurso();
                }
                if(TentarMover(-1,0)) {

                    EmpilharPercurso(2);
                    if(SeguirDesviando(2, xOrigem-1, yOrigem, passos+1, maximoPassos-1))
                        return true;
                }
                if(passos >= maximoPassos) {
                    return true;
                }
                DesempilharPercurso();
                return false;

            break;

            case 3:
            /*
            para a direita, tentando primeiro desviar para cima, depois para baixo
            e em ultimo caso continuar para a direita
            */

                if(TentarMover(0,1)) {

                    EmpilharPercurso(0);
                    if(SeguirDesviando(0, xOrigem, yOrigem+1, passos+1, maximoPassos-1))
                        return true;
                    DesempilharPercurso();
                }
                if(TentarMover(0,-1)) {

                    EmpilharPercurso(1);
                    if(SeguirDesviando(1, xOrigem, yOrigem-1, passos+1, maximoPassos-1))
                        return true;
                    DesempilharPercurso();
                }
                if(TentarMover(1,0)) {

                    EmpilharPercurso(3);
                    if(SeguirDesviando(3, xOrigem+1, yOrigem, passos+1, maximoPassos-1))
                        return true;
                }
                if(passos >= maximoPassos) {
                    return true;
                }
                DesempilharPercurso();
                return false;

            break;
        }

        return false;
    }

    private void BolarRotaDeFuga(int x, int y) {

        /*
        esta funcao tenta bolar um caminho com o maior numero de curvas possivel, dando um total de passos
        baseado na distancia entre a vitima e o monstro
        caso nao haja rota com o minimo de passos necessarios, e dado um passo aleatorio
        */

        int direcaoDoMonstro = DirecaoDoPonto(x, y);
        int distanciaDoMonstro = DistanciaDoPonto(x,y);

        bool fugaSatisfatoria = false;

        switch(direcaoDoMonstro) {
            case 0:
                if(SeguirDesviando(1, (int)transform.position.x, (int)transform.position.y, 0, distanciaDoMonstro))
                    fugaSatisfatoria = true;
            break;
            case 1:
                if(SeguirDesviando(0, (int)transform.position.x, (int)transform.position.y, 0, distanciaDoMonstro))
                    fugaSatisfatoria = true;
            break;
            case 2:
                if(SeguirDesviando(3, (int)transform.position.x, (int)transform.position.y, 0, distanciaDoMonstro))
                    fugaSatisfatoria = true;
            break;
            case 3:
                if(SeguirDesviando(2, (int)transform.position.x, (int)transform.position.y, 0, distanciaDoMonstro))
                    fugaSatisfatoria = true;
            break;
        }

        if(!fugaSatisfatoria) {
            estado = 0;
        }
    }

    private void BolarRotaDeEncontro(int x, int y) {

        /*
        esta funcao elabora o caminho em linha reta entre este elemento e o elemento visto
        */

        int direcaoDoJogador = DirecaoDoPonto(x, y);
        int distanciaDoJogador = DistanciaDoPonto(x,y);

        for(int i=0; i<distanciaDoJogador; i++) {

            EmpilharPercurso(direcaoDoJogador);
        }
    }

    private void EscolherDestinoAleatorio() {

        /*
        esta funcao elabora um caminho aleatorio, mas sem passos para tras, com tamanho minimo e maximo
        reciclando a implementacao da fuga
        */

        int direcaoInicial = (int)Random.Range(0,4);
        int distanciaDoPercurso = (int)Random.Range(5,10);

        int tentativa = 0;

        bool rotaSatisfatoria = false;

        do {
            if(SeguirDesviando(direcaoInicial, (int)transform.position.x, (int)transform.position.y, 0, distanciaDoPercurso)){
                rotaSatisfatoria = true;
            }
            else {
                direcaoInicial++;
                direcaoInicial = direcaoInicial % 4;
                tentativa++;
            }
        } while(!rotaSatisfatoria && tentativa < 4);
        
        if(!rotaSatisfatoria) {
            estado = 0;
        }


        Debug.Log("tamanho do percurso: " + pilhaPercurso.Count);
        foreach(int passo in pilhaPercurso) {
            Debug.Log("passo: " + passo);
        }
    }

    private void AndarAleatoriamente() {

        int direcao = Random.Range(0, 4);

        do {

            switch(direcao) {

            case 0:
                if(TentarMover(0,1)) {
                    StartCoroutine(MoverCima(animator));
                    return;
                }
            break;
            case 1:
                if(TentarMover(0,-1)) {
                    StartCoroutine(MoverBaixo(animator));
                    return;
                }
            break;
            case 2:
                if(TentarMover(-1,0)) {
                    StartCoroutine(MoverEsquerda(animator));
                    return;
                }
            break;
            case 3:
                if(TentarMover(1,0)) {
                    StartCoroutine(MoverDireita(animator));
                    return;
                }
            break;
            }

            direcao++;
            direcao = direcao % 4;

        } while(true);
        
    }

    private void SeguirJogador() {

        Vector2 movimento = ultimaPosicaoJogador - (Vector2)transform.position;

        Mover(animator,(int)movimento.x, (int)movimento.y);
    }

    private void SeguirPercurso() {

        int direcao = pilhaPercurso[pilhaPercurso.Count - 1];

        switch(direcao) {

            case 0:
                StartCoroutine(MoverCima(animator));
            break;
            case 1:
                StartCoroutine(MoverBaixo(animator));
            break;
            case 2:
                StartCoroutine(MoverEsquerda(animator));
            break;
            case 3:
                StartCoroutine(MoverDireita(animator));
            break;
        }

        DesempilharPercurso();
    }

    private void AtualizarUltimaPosicaoJogador() {

        ultimaPosicaoJogador = new Vector2(GameManager.instancia.boardScript.posicoes[1,0] , GameManager.instancia.boardScript.posicoes[1,1]);
    }

    protected override void ProcessarColisao(int colisao) {

        switch (colisao) {
            case 1:
                break;

            case 2:
                break;
        }
    }
}
