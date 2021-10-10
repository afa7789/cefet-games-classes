using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bichinho : MonoBehaviour {

    public int entidade;
    public float duracaoMovimento = .1f;
    protected float inversoDuracaoMovimento;
	protected virtual void Start () {

        inversoDuracaoMovimento = 1f / duracaoMovimento;
	}

    public IEnumerator MoverCima(Animator animator) {
        Mover(animator,0, 1);
        yield return new WaitForSeconds(duracaoMovimento);
    }

    public IEnumerator MoverBaixo(Animator animator) {
        Mover(animator,0, -1);
        yield return new WaitForSeconds(duracaoMovimento);
    }

    public IEnumerator MoverEsquerda(Animator animator) {
        Mover(animator,-1, 0);
        yield return new WaitForSeconds(duracaoMovimento);
    }

    public IEnumerator MoverDireita(Animator animator) {
        Mover(animator,1, 0);
        yield return new WaitForSeconds(duracaoMovimento);
    }

    protected bool TentarMover (int xDir, int yDir) {

        int xDestino = (int)this.transform.position.x + xDir;
        int yDestino = (int)this.transform.position.y + yDir;

        return GameManager.instancia.boardScript.PodeMover(xDestino, yDestino);
    }

    protected void Mover(Animator animator,int xDir, int yDir) {
        anim(animator,xDir,yDir);
        int colisao = GameManager.instancia.boardScript.Mover(entidade, xDir, yDir);

        Vector2 origem = transform.position;
        Vector2 destino = origem + new Vector2(xDir, yDir);
        StartCoroutine(MovimentoSuave(destino));

        if(colisao != -1) ProcessarColisao(colisao);

        GameManager.instancia.turnoDeQuem++;
        GameManager.instancia.turnoDeQuem = GameManager.instancia.turnoDeQuem % 2;
    }

    protected void anim(Animator animator,int horizontal,int vertical){ 
       //direita Mover(1, 0);
       //esquerda Mover(-1, 0);
       //cima Mover(0, 1);
       //baixo Mover(0, -1);
        if(horizontal ==1 && vertical==0){
            animator.SetTrigger("1");
        }else if(horizontal ==-1 && vertical==0){
            animator.SetTrigger("2");
        }else if(horizontal ==0 && vertical==1){
            animator.SetTrigger("3");
        }else if(horizontal ==0 && vertical==-1){
            animator.SetTrigger("4");
        }else{
            animator.SetTrigger("0");
        }
    }

    protected abstract void ProcessarColisao(int colisao);

    protected IEnumerator MovimentoSuave (Vector3 destino) {

        float quadradoDistanciaRestante = 0f;

        do {
            quadradoDistanciaRestante = (transform.position - destino).sqrMagnitude;
            Vector3 novaPosicao = Vector3.MoveTowards(gameObject.transform.position, destino, inversoDuracaoMovimento * Time.deltaTime);
            gameObject.transform.position = novaPosicao;

            yield return null;

        } while (quadradoDistanciaRestante >= float.Epsilon);
    }

    protected List<int> ChecarCampoDeVisao() {

        return GameManager.instancia.boardScript.EntidadesNoCampoDeVisao((int)transform.position.x, (int)transform.position.y);
    }
}
