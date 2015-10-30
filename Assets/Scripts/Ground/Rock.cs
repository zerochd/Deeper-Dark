using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	//危险提示
	public GameObject DangerPoint;

	//石头坠落音效
	public AudioClip rockFallClip;

	//石头坠落特效
	public GameObject fallEffect;

	int damage = 50;

	Vector3 bornVec;
	Vector3 endVec;
	//加速度
	float acc;

	bool havePlaySound = false;
	// Use this for initialization
	void Start () {
		Instantiate (DangerPoint,this.transform.position,this.transform.rotation);
		endVec = this.transform.position;
		this.transform.position += Vector3.up * 10f;
		bornVec = this.transform.position;
		acc = 1f;
	}
	
//	// Update is called once per frame
//	void Update () {
//		if(bornVec.y - endVec.y > float.Epsilon){
//			bornVec.y -= acc * Time.deltaTime;
//			this.transform.position = bornVec;
//			acc += 0.01f;
//		}
//	}

	void FixedUpdate(){
		if(bornVec.y - endVec.y > float.Epsilon){
			this.transform.position = bornVec;
			bornVec.y -= acc * Time.fixedDeltaTime;
			acc += Random.Range(0,1) == 0 ? 0.85f : 0.9f;
			if(bornVec.y - endVec.y < float.Epsilon + 0.5f)
			{
				this.GetComponent<CircleCollider2D>().isTrigger = false;
			}
		}
		else{
			if(rockFallClip != null && havePlaySound == false){
				AudioSource.PlayClipAtPoint(rockFallClip,Vector3.zero,10f);
				havePlaySound = true;

				if(fallEffect != null){
					Instantiate(fallEffect, this.transform.position - Vector3.up, this.transform.rotation);
				}
				this.GetComponent<CircleCollider2D>().isTrigger = true;
				iTween.ColorTo(this.gameObject,new Color(1f,1f,1f,0),1.5f);
//				Invoke("DestroyRock",1.5f);
				Destroy(this.gameObject,1.5f);
			}
		}
		//开启碰撞
//		this.GetComponent<CircleCollider2D>().isTrigger = false;

	}

	void OnCollisionEnter2D(Collision2D other){
		if(other.transform.GetComponent<Rigidbody2D>()){
			Vector2 forceVec = (Vector2)(other.transform.position - this.transform.position);
			other.gameObject.GetComponent<Rigidbody2D>().AddForce(forceVec*2500f);
			if(other.gameObject.CompareTag("Enemy")){
				Debug.Log ("hit enemy");
				other.gameObject.GetComponent<Enemy>().SetHP(damage);

			}else if(other.gameObject.CompareTag("Player")){
				Player.instance.SetHP(damage);
			}

		}
	}

	void OnCollisionStay2D(Collision2D other){
		if(other.transform.GetComponent<Rigidbody2D>()){
			Vector2 forceVec = (Vector2)(other.transform.position - this.transform.position);
			other.gameObject.GetComponent<Rigidbody2D>().AddForce(forceVec*2500f);
			if(other.gameObject.CompareTag("Enemy")){
				Debug.Log ("stay enemy");
				other.gameObject.GetComponent<Enemy>().SetHP(damage);
				
			}else if(other.gameObject.CompareTag("Player")){
				Player.instance.SetHP(damage);
			}
			
		}
	}

//	//删除石头
//	void DestroyRock(){
//		Destroy(this.gameObject);
//	}
	
}
