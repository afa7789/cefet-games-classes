/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package br.cefetmg.games.movement.behavior;

import br.cefetmg.games.movement.AlgoritmoMovimentacao;
import br.cefetmg.games.movement.Direcionamento;
import br.cefetmg.games.movement.Pose;
import com.badlogic.gdx.Input;
import com.badlogic.gdx.math.Vector3;

/**
 *
 * @author Aluno
 */
public class Chegar  extends AlgoritmoMovimentacao {
    
    private static final char NOME = 'c';
    private static final float radius = 5;
    private static final float timeToTarget = 0.25f;
    public Chegar(float maxVelocidade) {
        this(NOME, maxVelocidade);
    }

    protected Chegar(char nome, float maxVelocidade) {
        super(nome);
        this.maxVelocidade = maxVelocidade;
    }

    @Override
    public Direcionamento guiar(Pose agente) {
        Direcionamento output = new Direcionamento();
        Vector3 aux = new Vector3(alvo.getObjetivo());
        output.velocidade = aux.sub(agente.posicao);
        if( output.velocidade.len()< radius){
            return new Direcionamento(0,0);
        }
        output.velocidade.scl(timeToTarget);
        if(output.velocidade.len() > maxVelocidade){
            output.velocidade.nor();
            output.velocidade.scl(maxVelocidade);
        }
        //output.velocidade.scl(maxVelocidade);
        agente.olharNaDirecaoDaVelocidade(output.velocidade);
        output.rotacao = 0;
        // calcula que direção tomar (configura um objeto Direcionamento 
        // e o retorna)
        // ...
        // super.alvo já contém a posição do alvo
        // agente (parâmetro) é a pose do agente que estamos guiando
        // ...
        return output;
    }

    @Override
    public int getTeclaParaAtivacao() {
        return Input.Keys.C;
    }
}
