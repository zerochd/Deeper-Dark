using UnityEngine;
using System.Collections;
using System;


public class AttackPoint : MonoBehaviour {

	public Player player;



	void OnTriggerEnter2D(Collider2D other){

		if(other.tag.Equals("Enemy")){
			if(other.GetComponent<Enemy>()){
				other.GetComponent<Enemy>().Hurt(player.GetDamage());

				gameObject.GetComponent<CircleCollider2D>().enabled = false;
			}
			else{
				Debug.LogError("don't have Enemy Script");
			}
		}
	}



}
