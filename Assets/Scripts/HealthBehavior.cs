using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehavior : MonoBehaviour {

	public int StarterHp;
	private int hp;
	private bool Alive;

	void Start () {
		hp = StarterHp;
		Alive = true;
	}

	void Update () {
		if (!Alive) {
			Destroy (gameObject);
		}
	}

	public int getHP(){
		return hp;
	}

	public bool ReceiveDMG(int dmg){ // Esse valor retornado indica se o boss foi morto ou não
		if ((hp - dmg) < 0) {
			hp = 0;
			Alive = false;
		} else {
			hp -= dmg;
		}
		return Alive;
	}
}
