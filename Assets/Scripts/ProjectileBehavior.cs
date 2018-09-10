using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour {

	public int damage;
	public Vector2 direction;
	public float velocity;
	public List<string> targets;
	public float lifetime;

	// Use this for initialization
	void Start () {
		this.transform.position += new Vector3(0,0,-5);
	}

	// Update is called once per frame
	void FixedUpdate () {
		direction = direction.normalized * velocity;
		this.transform.position += (Vector3)direction;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (targets.Contains (other.tag)) {
			other.GetComponent<HealthBehavior> ().ReceiveDMG (damage);
			Destroy (gameObject);
		}
	}

	public IEnumerator Lifetime(){
		yield return new WaitForSeconds (lifetime);
		Destroy (gameObject);
	}
}
