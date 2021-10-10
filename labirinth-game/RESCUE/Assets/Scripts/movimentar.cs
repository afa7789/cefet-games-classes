using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimentar : MonoBehaviour {
	public GameObject Heli;
	// Use this for initialization
	public int estado;
	public float scale1;
	private float speed,speed2,moveHorizontal,moveVertical;
	void Start () {
		speed2= speed =0.5f;
		estado =0;
		//speed2= speed;
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.S) && ((estado == 1 )|| (estado == 0))){//move pra baixo
			estado=1;
			speed2*=1.01f;
			moveHorizontal=0;
			moveVertical = -1.2f;
		}else if(Input.GetKey(KeyCode.W) && ((estado == 2 )|| (estado == 0))){//move pra cima
			estado=2;
			speed2*=1.01f;
			moveHorizontal=0;
			moveVertical = 1.0f;
		}else if(Input.GetKey(KeyCode.A)&& ((estado == 3 )|| (estado == 0))){//move para esquerda
			Heli.transform.InverseTransformDirection(1, 0, 0);
			moveHorizontal=-1;  
			estado=3;
		}else if(Input.GetKey(KeyCode.D)&&((estado == 4 )|| (estado == 0))){//move para direita
			moveHorizontal=1;
			estado=4;
		}else{//idle padrão
			speed2 = 0.0f;
			speed2= 0.01f;
			moveHorizontal =0;
			moveVertical=- 1.0f;
			speed2*=1.5f;
			estado=0;
		}
		Heli.transform.Translate( (float) (speed*moveHorizontal) ,(float)(speed2*moveVertical) ,0);
	}
}
