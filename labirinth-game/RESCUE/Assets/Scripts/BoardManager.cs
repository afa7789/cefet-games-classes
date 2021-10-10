using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	public int linhas = 10;
    public int colunas = 10;
    public int quantidadeEntidades = 3;

    public bool montandoLabirinto = false;

    public GameObject protagonista;
    public GameObject monstro;
    public GameObject vitima;

    public int[,] labirinto;
    public int[,] posicoes;

    private void MontagemLabirinto() {

        montandoLabirinto = true;

        //inicializa a posicao das tres entidades do jogo
        posicoes = new int[quantidadeEntidades,2];

        posicoes[0,0] = 9;
        posicoes[0,1] = 9;

        posicoes[1,0] = 1;
        posicoes[1,1] = 1;

        posicoes[2,0] = 5;
        posicoes[2,1] = 5;

        //inicializa o labirinto
        int[,] montagem = new int[,]
        	{{1,1,1,1,1,1,1,1,1,1},
        	 {1,0,1,1,0,0,0,0,0,1},
        	 {1,0,0,0,0,1,1,1,0,1},
        	 {1,0,1,1,0,0,0,1,0,1},
        	 {1,0,0,1,0,1,0,0,0,1},
        	 {1,1,0,1,1,1,1,0,1,1},
        	 {1,0,0,0,0,1,0,0,0,1},
        	 {1,1,1,1,0,1,0,1,1,1},
        	 {1,0,0,0,0,0,0,0,0,1},
        	 {1,1,1,1,1,1,1,1,1,1}};

        labirinto = montagem;

        montandoLabirinto = false;
    }

    public IEnumerator VitimaEntrando() {
    	yield return new WaitForSeconds(5);
    }

    public IEnumerator ProtagonistaEntrando() {
    	yield return new WaitForSeconds(5);
    }

    public IEnumerator MonstroEntrando() {

        montandoLabirinto = true;

        yield return new WaitForSeconds(5);

        montandoLabirinto = false;
    }

    public void MontarCena() {

        MontagemLabirinto();

    }

    public bool PodeMover(int x, int y) {

    	return labirinto[x,y] == 1 ? false : true;
    }

    public int Mover(int entidade, int xDir, int yDir) {

    	posicoes[entidade , 0] += xDir;
    	posicoes[entidade , 1] += yDir;

    	for(int i=0; i<quantidadeEntidades; i++) {

    		if(i==entidade) continue;

    		if(posicoes[i, 0] == posicoes[entidade, 0] && posicoes[i, 1] == posicoes[entidade, 1])
    			return i;
    	}

    	return -1;
    }

    public List<int> EntidadesNoCampoDeVisao(int xOrigem, int yOrigem) {

    	List<int> entidadesVistas = new List<int>();

    	for(int direcao = 0; direcao<4; direcao++) {
    		/*
    		0 - cima
    		1 - baixo
    		2 - esquerda
    		3 - direita
    		*/

    		int posicaoVista = 0;

    		int x = xOrigem;
    		int y = yOrigem;

    		while(posicaoVista != 1 && x>=0 && x<colunas && y>=0 && y<linhas) {

    			if(x != xOrigem || y != yOrigem) {

    				for(int i=0; i<quantidadeEntidades; i++) {
    					if(x == posicoes[i, 0] && y == posicoes[i, 1]) {
    						entidadesVistas.Add(i);
    					}
    				}
    			}

    			switch(direcao) {
    				case 0:
    					y++;
    					break;
    				case 1:
    					y--;
    					break;
    				case 2:
    					x--;
    					break;
    				case 3:
    					x++;
    					break;
    			}

    			posicaoVista = labirinto[x,y];
    		}
    	}

    	return entidadesVistas;
    }
}
