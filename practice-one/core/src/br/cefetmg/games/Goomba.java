/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package br.cefetmg.games;

import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Animation;
import com.badlogic.gdx.graphics.g2d.Animation.PlayMode;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
//askdlaksldkalsdk
/**
 *
 * @author Dota
 */
public class Goomba {
    
    //private enum GoombaState{;
    //        
    //    IDLE,UP,DOWN,LEFT,RIGHT;
    //};
    public int GombaState;
    public float tempoDaAnimacao;
    private float goombax,goombay;
    private Texture gombaSprite,gombaShadowSprite,gombaSheet;
    TextureRegion[][] quadrosDaAnimacao;
    Animation andarParaFrente,andarParaTras,andarParaEsquerda,andarParaDireita;


    public Goomba(Texture gombaSprite, Texture gombaShadowSprite,Texture gombaSheet) {
        this.GombaState =0;
        this.gombaSprite = gombaSprite;
        this.gombaShadowSprite = gombaShadowSprite;
        this.gombaSheet = gombaSheet;
        quadrosDaAnimacao = TextureRegion.split(gombaSheet, 21, 24);
        andarParaFrente = new Animation(0.1f,
                            quadrosDaAnimacao[0][0], // 1ª linha, 1ª coluna
                            quadrosDaAnimacao[0][1], // idem, 2ª coluna
                            quadrosDaAnimacao[0][2],
                            quadrosDaAnimacao[0][3],
                            quadrosDaAnimacao[0][4]);
        andarParaDireita = new Animation(0.1f,
                            quadrosDaAnimacao[1][0], // 2ª linha, 1ª coluna
                            quadrosDaAnimacao[1][1], // idem, 2ª coluna
                            quadrosDaAnimacao[1][2],
                            quadrosDaAnimacao[1][3],
                            quadrosDaAnimacao[1][4]);
        andarParaTras = new Animation(0.1f,
                            quadrosDaAnimacao[2][0], // 3ª linha, 1ª coluna
                            quadrosDaAnimacao[2][1], // idem, 2ª coluna
                            quadrosDaAnimacao[2][2],
                            quadrosDaAnimacao[2][3],
                            quadrosDaAnimacao[2][4]);
        andarParaEsquerda = new Animation(0.1f,
                            quadrosDaAnimacao[3][0], // 4ª linha, 1ª coluna
                            quadrosDaAnimacao[3][1], // idem, 2ª coluna
                            quadrosDaAnimacao[3][2],
                            quadrosDaAnimacao[3][3],
                            quadrosDaAnimacao[3][4]);
        andarParaFrente.setPlayMode(PlayMode.LOOP_PINGPONG);
        andarParaTras.setPlayMode(PlayMode.LOOP_PINGPONG);
        andarParaDireita.setPlayMode(PlayMode.LOOP_PINGPONG);
        andarParaEsquerda.setPlayMode(PlayMode.LOOP_PINGPONG);
    }
    
    public Object animator(){
        switch(GombaState){
            case 0:
                break;
            case 1:
                return andarParaFrente.getKeyFrame(tempoDaAnimacao);
            case 2:
                return andarParaDireita.getKeyFrame(tempoDaAnimacao);
            case 3:
                return andarParaTras.getKeyFrame(tempoDaAnimacao);
            case 4:
                return andarParaEsquerda.getKeyFrame(tempoDaAnimacao);
            default:
                break;
        }
        return andarParaFrente.getKeyFrame(tempoDaAnimacao); 
    }
    
    public float getGoombax() {
        return goombax;
    }

    public void setGoombax(float goombax) {
        this.goombax += goombax;
    }

    public float getGoombay() {
        return goombay;
    }

    public void setGoombay(float goombay) {
        this.goombay += goombay;
    }

    public Texture getGombaSprite() {
        return gombaSprite;
    }

    public Texture getGombaShadowSprite() {
        return gombaShadowSprite;
    }
    
}
