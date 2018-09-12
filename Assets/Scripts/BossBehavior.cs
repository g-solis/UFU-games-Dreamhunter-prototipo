using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour {

	private Vector2 Direction_to_player;
	private GameObject Player;
	private float ActualAttackCooldown;
	public float BurstCooldown;
	public float ShotgunCooldown;
	public int Damage;
	public float ProjectileVelocity;


	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
		Physics2D.IgnoreCollision(Player.GetComponent<Collider2D>(),this.GetComponent<Collider2D>(),true);
		ActualAttackCooldown = 3;
	}

	// Update is called once per frame
	void Update () {
		if(Player != null){
			Direction_to_player.x = Player.transform.position.x - this.transform.position.x;
			Direction_to_player.y = Player.transform.position.y - this.transform.position.y;
			if (ActualAttackCooldown <= 0 && Vector2.Distance(Player.transform.position,this.transform.position) < 20) {
				if(Vector2.Distance(Player.transform.position,this.transform.position) > 6){
					StartCoroutine(BurstAttack());//Se a distancia entre o jogador e o boss for menor que 6,
				} else {//                        ele faz o ataque em shotgun, se for maior ou igual, faz o ataque em burst.
					ShotgunAttack();
				}
			} else {
				ActualAttackCooldown -= Time.deltaTime;
			}
		}
	}

	private IEnumerator BurstAttack(){
		ActualAttackCooldown = BurstCooldown;
		GameObject p;
		var StableDirection = Direction_to_player;
		for (int i = 0; i < 3; i++) {
			p =  (GameObject)Instantiate(Resources.Load("projetil"));
			p.GetComponent<ProjectileBehavior>().direction = StableDirection;
			p.GetComponent<ProjectileBehavior>().damage = Damage;
			p.GetComponent<ProjectileBehavior>().targets.Add("Player");
			p.GetComponent<ProjectileBehavior>().velocity = ProjectileVelocity;
			p.GetComponent<ProjectileBehavior>().lifetime = 5;
			StartCoroutine(p.GetComponent<ProjectileBehavior>().Lifetime());
			p.transform.position = this.transform.position;
			if(i != 2){
				yield return new WaitForSeconds (0.15f);
			}
		}
	}

	private void ShotgunAttack(){
		ActualAttackCooldown = ShotgunCooldown;
		GameObject p;
		for (int i = 0; i < 3; i++) {
			p =  (GameObject)Instantiate(Resources.Load("projetil"));
			p.GetComponent<ProjectileBehavior>().direction = Direction_to_player;
			p.GetComponent<ProjectileBehavior>().damage = Damage;
			p.GetComponent<ProjectileBehavior>().targets.Add("Player");
			p.GetComponent<ProjectileBehavior>().velocity = ProjectileVelocity;
			p.GetComponent<ProjectileBehavior>().lifetime = 1.5f;
			p.transform.position = this.transform.position;
			if(i == 1) { // aumenta o angulo do ataque em 10°
				var angulo = Mathf.Atan2(p.GetComponent<ProjectileBehavior>().direction.x,p.GetComponent<ProjectileBehavior>().direction.y)*180/Mathf.PI;
				angulo = (angulo+10)*Mathf.PI/180;
				p.GetComponent<ProjectileBehavior>().direction = new Vector2(Mathf.Sin(angulo),Mathf.Cos(angulo));
			}
			if(i == 2) {// diminui o angulo do ataque em 10°
				var angulo = Mathf.Atan2(p.GetComponent<ProjectileBehavior>().direction.x,p.GetComponent<ProjectileBehavior>().direction.y)*180/Mathf.PI;
				angulo = (angulo-10)*Mathf.PI/180;
				p.GetComponent<ProjectileBehavior>().direction = new Vector2(Mathf.Sin(angulo),Mathf.Cos(angulo));
			}
			StartCoroutine(p.GetComponent<ProjectileBehavior>().Lifetime());
		}
	}
}
