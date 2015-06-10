using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour 
{
	private PlayerController playerController;


	void Start()
	{
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Enemy")
		{
			if (playerController.PlayerAttack)
			{
				//TODO 攻击怪物逻辑
				Debug.Log("玩家打人啦~~~~~~~");
			}
		}
	}
}
