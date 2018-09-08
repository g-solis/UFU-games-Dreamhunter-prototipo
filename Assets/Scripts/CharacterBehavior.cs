using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour {
    public float speed;
    public float AttackDuration;
    public float AttackCouldownTime;
    public Object ObjAttack;
    private Rigidbody2D rb;
    private SpriteRenderer sprRenderer;
    private Vector2 Direction;
    private Animator animator;
    private bool AttackCouldown = false;
    private bool IsAttaking;
    // Use this for initialization
    void Start () {
        this.rb = GetComponent<Rigidbody2D>();
        this.sprRenderer = GetComponent<SpriteRenderer>();
        this.Direction = new Vector2(0, 1);
        this.animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if(this.IsAttaking == false){
            this.Move();  
        } else{
            this.rb.velocity = 0*this.rb.velocity;
        }
        if(Input.GetMouseButtonDown(0) && this.AttackCouldown == false){
            this.AttackCouldown = true;
            StartCoroutine(this.Attack());
        }

	}
    
    void Move()
    {
        int DirectionY = (int)Input.GetAxisRaw("Vertical");
        int DirectionX = (int)Input.GetAxisRaw("Horizontal");

        this.Direction = new Vector2(DirectionX, DirectionY);
        this.rb.velocity = (this.Direction * this.speed);
    }
    IEnumerator Attack()
    {
        var worldMousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition);
        Vector2 mouseDirection = (Vector2)worldMousePosition - (Vector2)transform.position;
        mouseDirection = mouseDirection.normalized;

        animator.SetFloat("MouseDirectionX",mouseDirection.x);
        animator.SetFloat("MouseDirectionY",mouseDirection.y);
        animator.SetBool("attaking",true);
        
        var AttackPosition = transform.position + new Vector3(mouseDirection.x*2,mouseDirection.y*2.5f,0);
        var atk = Instantiate(ObjAttack, AttackPosition, Quaternion.identity);
        Destroy(atk,AttackDuration);

        this.IsAttaking = true;
        yield return new WaitForSeconds(AttackDuration);
        animator.SetBool("attaking",false);
        this.IsAttaking = false;
        yield return new WaitForSeconds((float)AttackCouldownTime);
        this.AttackCouldown = false;
    }
}
