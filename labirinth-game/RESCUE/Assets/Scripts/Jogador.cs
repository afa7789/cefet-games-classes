using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jogador : Bichinho {

    private Animator animator;

    protected override void Start () {

        animator = GetComponent<Animator>();
        entidade = 1;
        base.Start();
    }
	

	void Update () {

        //so e vez do jogador quando turnoDeQuem == 1
        if (GameManager.instancia.turnoDeQuem != 1) return;

        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0) vertical = 0;

        if (horizontal != 0 || vertical != 0) {
            if(TentarMover(horizontal, vertical))
                Mover(animator,horizontal, vertical);
            }
    }

    protected override void ProcessarColisao(int colisao) {

        switch(colisao) {
            case 0:
                break;

            case 2:
                break;
        }
    }
}
