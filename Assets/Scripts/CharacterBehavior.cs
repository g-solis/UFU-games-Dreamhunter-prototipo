using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour {
    private Rigidbody2D rb;
    public float speed;
    private Vector2 Direction;
    // Use this for initialization
    void Start () {
        this.rb = GetComponent<Rigidbody2D>();
        this.Direction = new Vector2(0, 1);
    }
	
	// Update is called once per frame
	void Update () {
        this.Move();
        this.Atack();

	}
    
    void Move()
    {
        int DirectionY = (int)Input.GetAxisRaw("Vertical");
        int DirectionX = (int)Input.GetAxisRaw("Horizontal");

        this.Direction = new Vector2(DirectionX, DirectionY);
        this.rb.velocity = (this.Direction * this.speed);
    }
    void Atack()
    {

    }
}
