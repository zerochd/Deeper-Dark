using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class AttackPoint : MonoBehaviour {

	public CHARACTER_TYPE characterType = CHARACTER_TYPE.NONE;

	//伤害文本
	public Text damageInfo;

	public enum CHARACTER_TYPE{
		NONE = -1,
		PLAYER = 0,	//玩家
		BOSS		//BOSS
	}

	void OnTriggerEnter2D(Collider2D other){

		switch(characterType){
		case CHARACTER_TYPE.PLAYER:{
			if(other.tag.Equals("Enemy")){
				//攻击怪物
				if(other.GetComponent<Enemy>()){
					other.GetComponent<Enemy>().SetHP(Player.instance.GetDamage());
					gameObject.GetComponent<CircleCollider2D>().enabled = false;
				}
				//攻击boss
				else if(other.GetComponent<Boss>()){
					//显示伤害数字
					if(damageInfo!=null){
//						GameObject go =	Instantiate(damageInfo) as GameObject;
//						go.transform.SetParent(GameObject.Find("InfoPanel").transform);
						Text damageText = Instantiate(damageInfo);
						damageText.text = "" + Player.instance.GetDamage();
						damageText.transform.SetParent(GameObject.Find("InfoPanel").transform);
						Vector3 textVec = Camera.main.WorldToScreenPoint(other.transform.FindChild("head").position);
						damageText.rectTransform.position = textVec;
					}
					other.GetComponent<Boss>().SetHP(Player.instance.GetDamage());
					gameObject.GetComponent<CircleCollider2D>().enabled = false;
				}
				else{
					Debug.LogError("don't have Enemy Script");
				}
			}
		}break;
		case CHARACTER_TYPE.BOSS:{
			if(other.CompareTag("Player")){
				//攻击玩家的受伤区域
				if(other.gameObject.name.Equals("hurtBox")&&
				   other.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))){
					if(other.GetComponentInParent<Player>()){
	//					Debug.Log ("hit");
						other.GetComponentInParent<Player>().SetHP(GetComponentInParent<Boss>().GetDamage());
						Vector3 dir = (other.transform.position - this.transform.parent.position).normalized;
						
						Vector3 attackMoveEnd = other.transform.position + dir*0.5f;
						
						//玩家被击飞移动
						other.GetComponentInParent<Player>().HurtForce(attackMoveEnd);


						gameObject.GetComponent<CircleCollider2D>().enabled = false;
					}else{
						Debug.LogError("don't have Enemy Script");
					}
				}
			}
		}break;
		default:Debug.LogError("YOU SELECT NONE,BAGA");break;
		}
//
//		if(other.tag.Equals("Enemy")){
//			if(other.GetComponent<Enemy>()){
//				other.GetComponent<Enemy>().SetHP(Player.instance.GetDamage());
//
//				gameObject.GetComponent<CircleCollider2D>().enabled = false;
//			}
//			else{
//				Debug.LogError("don't have Enemy Script");
//			}
//		}
	}



}
