using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour {
    public float speed;
    public float AttackDuration;
    public float AttackCooldownTime;
    public Object ObjAttack;
    private Rigidbody2D rb;
    private SpriteRenderer sprRenderer;
    private Vector2 Direction;
    private Animator animator;
    private bool AttackCooldown = false;
    private bool CantMove;
    //Variaveis para rolagem
    private bool Rolling = false;
    private float RollSpeed;
    private Vector2 RollDirection;
    public float RollMinSpeed; 
    public float RollMaxSpeed;
    public float RollTime;
    public float RollCooldownTime;
    private bool RollCooldown = false;
    //som de ataque
    public AudioClip impact;
    AudioSource audioSource;
    // Use this for initialization
    void Start () {
        this.rb = GetComponent<Rigidbody2D>();
        this.sprRenderer = GetComponent<SpriteRenderer>();
        this.Direction = new Vector2(0, 1);
        this.animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if(this.CantMove == false){
            this.Move();
        } else{
            this.rb.velocity = 0*this.rb.velocity;
        }
        if(Input.GetMouseButtonDown(0) && this.AttackCooldown == false){
            this.AttackCooldown = true;
            StartCoroutine(this.Attack());
            audioSource.Play(0);
        } 
        else if(Input.GetKeyDown("space") && !this.RollCooldown){
            StartCoroutine(StartRolling());
        }
        if(this.Rolling){
            this.Roll();
        }
        

	}
    IEnumerator StartRolling(){
        this.Rolling = true;
        this.RollCooldown = true;
        this.CantMove = true;
        this.RollSpeed = this.RollMaxSpeed;
        var worldMousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition);
        this.RollDirection = (Vector2)worldMousePosition - (Vector2)transform.position;
        this.RollDirection = this.RollDirection.normalized;
        yield return new WaitForSeconds((float)RollTime);
        this.Rolling = false;
        this.CantMove = false;
        yield return new WaitForSeconds((float)RollCooldownTime);
        this.RollCooldown = false;
    }
    void Roll(){
        this.rb.velocity = (this.RollDirection * this.RollSpeed);
        this.RollSpeed -= (this.RollSpeed - this.RollMinSpeed) / 20;
        Debug.Log(this.RollSpeed);
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

        
        var angle = Vector2.Angle(mouseDirection,Vector2.right);
        if(mouseDirection.y < 0 ) angle = 360 - angle;
        if(angle%45 > 22.5) angle = angle + 45 - angle%45;
        else angle = angle - angle%45;
        
        animator.SetFloat("MouseDirectionX",Mathf.Cos(angle*Mathf.PI/180)*2);
        animator.SetFloat("MouseDirectionY",Mathf.Sin(angle*Mathf.PI/180)*2.5f);
        animator.SetBool("attaking",true);
        var AttackPosition = transform.position + new Vector3(Mathf.Cos(angle*Mathf.PI/180)*2,Mathf.Sin(angle*Mathf.PI/180)*2.5f,0);
        var atk = Instantiate(ObjAttack, AttackPosition, Quaternion.identity);
        Destroy(atk,AttackDuration);

        this.CantMove = true;
        yield return new WaitForSeconds(AttackDuration);
        animator.SetBool("attaking",false);
        this.CantMove = false;
        yield return new WaitForSeconds((float)AttackCooldownTime);
        this.AttackCooldown = false;
    }
}
