using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehavior : MonoBehaviour {

	public int Damage;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Boss"){
			other.GetComponent<HealthBehavior>().ReceiveDMG(Damage);
		}
	}
}
