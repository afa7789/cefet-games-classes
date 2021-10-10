package br.cefetmg.games.collision;

import com.badlogic.gdx.math.Circle;
import com.badlogic.gdx.math.Rectangle;
import com.badlogic.gdx.math.Vector2;

/**
 * Utilitário para verificação de colisão.
 *
 * @author fegemo <coutinho@decom.cefetmg.br>
 */
public class Collision {

    /**
     * Verifica se dois círculos em 2D estão colidindo.
     * @param c1 círculo 1
     * @param c2 círculo 2
     * @return true se há colisão ou false, do contrário.
     */
    public static final boolean circlesOverlap(Circle c1, Circle c2) {
        if( (c1.radius + c2.radius) * (c1.radius + c2.radius) > (((c1.x - c2.x)*(c1.x - c2.x)) + ((c1.y - c2.y)*(c1.y - c2.y))))
            return true;
        return false;
    }

    /**
     * Verifica se dois retângulos em 2D estão colidindo.
     * Esta função pode verificar se o eixo X dos dois objetos está colidindo
     * e então se o mesmo ocorre com o eixo Y.
     * @param r1 retângulo 1
     * @param r2 retângulo 2
     * @return true se há colisão ou false, do contrário.
     */
    
    public static final boolean colliderDesafio1(Rectangle r1,Collidable o1){
        if(rectsOverlap(r1,o1.getMinimumBoundingRectangle()))
            return circleOverLapRects(o1.getMinimumEnclosingBall(),r1);
        return false;
    }
    
    public static final boolean rectsOverlap(Rectangle r1, Rectangle r2) {
        if( (r1.x <= r2.x + r2.width) && (r1.x+ r1.width >= r2.x) )
            if( (r1.y <= r2.y + r2.height) && (r1.y+ r1.height >=r2.y) )
                return true;//if(r1.z == r2.z)
                    
        return false;
    }
//  public static final boolean circleOverLapRects(Circle c1, Rectangle r2){
//        if( (c1.x - ( Math.min(r2.width/2 , c1.x - r2.x ) ))*(c1.x - (Math.min(r2.width/2 , c1.x - r2.x ))) + (c1.y - (Math.min(r2.height/2 , c1.y - r2.y ))* (c1.y - (Math.min(r2.height/2 , c1.y - r2.y ))) )<= (c1.radius * c1.radius) )
//            return true;
//        return false;
//  }
    public static final boolean circleOverLapRects(Circle c1, Rectangle r2){
        
        Vector2 distancia = new Vector2(c1.x,c1.y);
        Vector2 centro_circulo = new Vector2(c1.x,c1.y);//circulo centro
        Vector2 centro_retangulo = new Vector2(r2.x+ r2.width/2 , r2.y+r2.height/2 );
        distancia.sub(r2.x,r2.y);
        
        Vector2 distancia_X = new Vector2( distancia.x,0 );
        Vector2 distancia_Y = new Vector2( distancia.y,0 );
        
        distancia_X.nor();
        distancia_Y.nor();
       
        Vector2 P = distancia_Y.scl(r2.width/2).add(distancia_X.scl(r2.width/2));
        P.add(centro_retangulo);
        
        if( centro_circulo.dst(P) <= c1.radius  )
            return true;
        
        return false;
    }
    
}
