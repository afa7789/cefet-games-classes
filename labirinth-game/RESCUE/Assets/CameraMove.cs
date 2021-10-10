using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    public Transform target;
    public float speed;
    Vector3 teste;

    void Start(){
    	teste= target.position;
    	teste.z = -10;
    }
    
    void Update() {
        teste = target.position;
        teste.z = -10;
        Debug.Log("OLA20");
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, teste, step);

    }
}
