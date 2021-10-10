using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerObjetivo : MonoBehaviour {
	
	public GameObject obj;
	public float timeLeft;
	//private bool moverPraTela = false;
	// Use this for initialization
	void Start () {
		obj.SetActive(false);
		timeLeft = 50.0f;
	}
 
	public void Update(){
	    timeLeft -= Time.deltaTime;
	    if (timeLeft <= 0.0f)
	    {
	    	obj.SetActive(true);
	    	//moverPraTela= true;
	    }
	}
}
