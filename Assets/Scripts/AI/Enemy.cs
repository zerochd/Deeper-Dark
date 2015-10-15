using UnityEngine;
using System.Collections;
using System;


public class Enemy : AllCharacter
{
    private bool hasSpawn;
	private AIMode step = AIMode.NONE;
	private AIMode nextStep = AIMode.NONE;
	private bool foundPlayer = false;

	//警戒墙壁2个像素
	private float distanceFromWall = -2f;

	//攻击距离
	private float distanceFromPlayer = -0.2f;

	//冲刺速度
	private float boost = 1f;

	//玩家的变换,敌人一开始不知道
	private Transform playerTransform = null;

	//玩家的被发现的位置
	private Vector3 PlayerLastedPosition = Vector3.zero;

	//接触到玩家
	private bool playerTouch = false;

	//面向方向
	private bool facingRight = false;

	//动画
	private Animator enemyAnim;
	
	//敌人类型
	public ENEMY_TYPE enemyType = ENEMY_TYPE.NORMAL;

	//表情状况
	public GameObject Point;

	//怪物攻击音效
	public AudioClip enemyAttackClip;

	//怪物死亡音效
	public AudioClip enemyDieClip;

	//死亡特效
	public GameObject dieEffect;

	//追人速度
//	public float inverseTime = 0.5f;

	//敌人类型
	public enum ENEMY_TYPE{
		NORMAL = 0,
		UNNORMAL = 1
	}

	//AI状态
	private enum AIMode{
		NONE = -1,
		WAIT = 0,//等待
		SEARCH = 1,//搜索
		ATTACK//攻击
	}

	// Use this for initialization
	void Start () {

		switch(enemyType){

			case ENEMY_TYPE.NORMAL:{
				hp = 1;
				damage = 10;
				speed = -0.8f;
			}
			break;
			case ENEMY_TYPE.UNNORMAL:{
			}
			break;
		}

		//初始化为等待状态
		nextStep = AIMode.WAIT;
        hasSpawn = false;
		enemyAnim = GetComponent<Animator>();
        //关闭一切组件
	}
	
	// Update is called once per frame
	void Update () {

		IsDead();

        if (hasSpawn == false)
        {
            if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
            {
                Spawn();
            }
        }
        else
        {
			if (!GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
            {
				Debug.Log ("Stop true");
				Stop();
//                Destroy(gameObject);
            }
			StateMachine();
        }





	}
	

	//------------------------------
	//方法
	

	void Spawn()
	{
		hasSpawn = true;
		Debug.Log ("spawn true");
		//开启一切组件
	}
	
	
	void Stop()
	{
		hasSpawn = false;
	}

	//Enemy状态机
	void StateMachine(){
		//判断是否迁移到下一个状态
		if(this.nextStep == AIMode.NONE)
		{
			switch(this.step){
			case AIMode.WAIT:
				if(true == hasSpawn){
//					Debug.Log ("search");
					nextStep = AIMode.SEARCH;
				}
			
				break;
			case AIMode.SEARCH:
				if(true == IsFoundPlayer()){
					nextStep = AIMode.ATTACK;
				}
				if(false == hasSpawn){
					nextStep = AIMode.WAIT;
				}
			
				break;
			case AIMode.ATTACK:
//				if(PlayerLastedPosition == Vector3.zero){
//					nextStep = AIMode.SEARCH;
//				}
				if(false == IsFoundPlayer()){
					nextStep = AIMode.SEARCH;
					StopCoroutine("EnemyAttackRange");
				}
				if(false == hasSpawn){
					nextStep = AIMode.WAIT;
				}
				break;
			}
		}
		
		//状态迁移初始化
		if(this.nextStep != AIMode.NONE)
		{

			switch(this.nextStep){
				case AIMode.WAIT:{

					GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				}
					break;
				case AIMode.SEARCH:{

				}
					break;
				case AIMode.ATTACK:{
					GetComponent<Rigidbody2D>().velocity = Vector2.zero;
					StartCoroutine("EnemyAttackRange");

				}
					break;
			}
			//更新状态
			this.step = this.nextStep;
			this.nextStep = AIMode.NONE;
		}
		
		
		//执行各个状态
		switch(this.step){
			case AIMode.WAIT:{
			}
				break;
			//搜索模式，左右移动，如果距离墙有distanceFromWall的长度，翻转角色方向
			case AIMode.SEARCH:{
				if(WallTouch(distanceFromWall)){
					Flip();
				}		
				GetComponent<Rigidbody2D>().velocity = new Vector2 (speed, GetComponent<Rigidbody2D>().velocity.y);
			}
				break;

			//攻击模式，往玩家的位置移动
			case AIMode.ATTACK:{
//				StartCoroutine(EnemySmoothMove());
				//当找到玩家上一个位置的时候


//				playerTouch = Physics2D.Linecast(this.transform.position,
//			                                 new Vector2(this.transform.position.x + distanceFromPlayer,this.transform.position.y),
//			                                 1 << LayerMask.NameToLayer("Player"));

				if(PlayerLastedPosition != Vector3.zero){

//					playerTouch = Physics2D.Linecast(this.transform.position,
//				                                 new Vector2(this.transform.position.x + distanceFromPlayer,this.transform.position.y),
//				                                 1 << LayerMask.NameToLayer("Player"));
					
					EnemyDirectMove(this.PlayerLastedPosition,playerTouch);

//					if(playerTouch){
//						PlayerLastedPosition = Vector3.zero;
//						enemyAnim.SetTrigger("attack");

				}

//				if(playerTouch){
//
//					PlayerLastedPosition = Vector3.zero;
//					enemyAnim.SetTrigger("attack");
//					playerTouch = false;
//					
//
//				}
				
			}
				break;
				
		}
	}
	

	
	//判断是否找到玩家
	public bool IsFoundPlayer(){
		return foundPlayer;
	}

	//设置是否找到玩家
	public void HaveFoundPlayer(Transform someoneTransform){

		//传递玩家的Transform
		this.playerTransform = someoneTransform;

		if(someoneTransform != null){
			//设置标志变量
			foundPlayer = true;

			//显示惊叹标志
			Point.SetActive(true);

			//传递玩家的位置
			this.PlayerLastedPosition = someoneTransform.position;
		}
		else{
			foundPlayer = false;

			//显示惊叹标志
			Point.SetActive(false);

			this.PlayerLastedPosition = Vector2.zero;
		}

	}

	//检查是否碰触到墙壁一定距离
	public bool WallTouch(float distance){

		bool wallTouched;


		wallTouched = Physics2D.Linecast(this.transform.position,
		                                 new Vector2(this.transform.position.x + distance,this.transform.position.y),
		                                 1 << LayerMask.NameToLayer("Wall"));

//		Debug.DrawLine(this.transform.position, new Vector3(this.transform.position.x + distance,this.transform.position.y,0f), Color.red, 1f);
		return wallTouched;
	}

	//角色转身
	void Flip ()
	{
		
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		distanceFromWall *= -1f;
		distanceFromPlayer *= -1f;
		speed *= -1f;
	}

	//敌人攻击距离判定
	IEnumerator EnemyAttackRange(){
		while(true){

			playerTouch = Physics2D.Linecast(this.transform.position,
			                                 new Vector2(this.transform.position.x + distanceFromPlayer,this.transform.position.y),
			                                 1 << LayerMask.NameToLayer("Player"));

			//如果接触到玩家的话,下一个玩家接触检测延迟0.8s+1帧,设置玩家位置为空，
			//设置动画为attack
			if(playerTouch && IsFoundPlayer() == true){

				Vector3 dir = (playerTransform.position - this.transform.position).normalized;
				
				Vector3 attackMoveEnd = playerTransform.position + dir*0.5f;

				//玩家被击飞移动
				StartCoroutine(playerTransform.GetComponent<Player>().EnemyAttackForce(attackMoveEnd));

				//减少玩家的血量
				playerTransform.GetComponent<Player>().SetHP(damage);

				//重置玩家被找到的位置
				PlayerLastedPosition = Vector3.zero;

				//开始攻击动画
				enemyAnim.SetTrigger("attack");



				//开始攻击音效
				SoundManager.instance.PlaySingle("enemy",enemyAttackClip);

				playerTouch = false;

				yield return new WaitForSeconds(0.8f);

				//重新寻找玩家

				HaveFoundPlayer(null);
			}

			yield return null;
		}
	}

//	//攻击击飞
//	IEnumerator EnemyAttackForce(Vector3 attackMoveEnd){
//
//		if(IsFoundPlayer() == false){
//			yield break;
//		}
//
//		Debug.Log ("foundplayer:"+foundPlayer + "playerTramsform:"+ playerTransform);
//
//		//设置玩家不能动
////		playerTransform.GetComponent<Player>().SetControl(false);
//
//		//获得移动距离
//		float sqrRemainingDistance = (playerTransform.position - attackMoveEnd).sqrMagnitude;
//		float maxDistanceDelta = 4f ;
//
//		//设置玩家的动画状态为hurt
//		playerTransform.GetComponent<Player>().SetAnimTrigger("hurt");
//
//		playerTransform.GetComponent<Player>().SetSuper(true);
//
//		while (sqrRemainingDistance > float.Epsilon && !playerTransform.GetComponent<Player>().wallTouch)
//		{
//			//Debug.Log("正在移动");
//			Vector3 newPosition = Vector3.MoveTowards(playerTransform.position, attackMoveEnd, maxDistanceDelta * Time.deltaTime);
//
////			Vector3 newPosition = Vector3.MoveTowards(playerTransform.position, attackMoveEnd, maxDistanceDelta * Time.deltaTime);
//			playerTransform.position = newPosition;
//			sqrRemainingDistance = (playerTransform.position - attackMoveEnd).sqrMagnitude;
//
//
//			yield return null;
//		}
//		yield return new WaitForSeconds(0.3f);
//		//设置玩家可以操控
////		playerTransform.GetComponent<Player>().SetControl(true);
//			
//	}


//
//	void LookRight(){
//		facingRight = true;
//		Vector3 theScale = transform.localScale;
//		theScale.x = -1;
//		transform.localScale = theScale;
//		distanceFromWall = -1f;
//	}
////
//	void LookLeft(){
//		facingRight = false;
//		Vector3 theScale = transform.localScale;
//		theScale.x = 1;
//		transform.localScale = theScale;
//		distanceFromWall = 1f;
//	}
//	

//	//敌人追踪移动
//	IEnumerator EnemySmoothMove()
//	{
//
//		while(playerTransform != null){
//			Vector3 moveEnd = playerTransform.position;
//			//Debug.Log("正在移动");
//
//			//当玩家在敌人左边保持向左，反之向右
//			if(playerTransform.position.x  - this.transform.position.x <= float.Epsilon){
//				LookLeft();
//			}
//			else{
//				LookRight();
//			}
//
////			this.transform.Translate(dir*0.01f*Time.deltaTime);//不停地移动
//			Vector3 newPosition = Vector3.MoveTowards(this.transform.position, moveEnd, inverseTime * Time.deltaTime);
//
//			transform.position = newPosition;
//	
//			yield return null;
//		}
//		
//		
//	}

	//如果传入的是Transform,就是实时跟踪，不然就是定点跟踪
//	void EnemySmoothMove(Vector3 moveEnd){
//		//慢慢跟踪,normalized将向量变为标准向量
//		Vector3 dir = (moveEnd - this.transform.position).normalized;
//		this.transform.Translate(dir*1.5f*Time.deltaTime);
//	}

	//根据找到上一个玩家的位置进行跟踪
	void EnemyDirectMove(Vector3 moveEnd,bool playerTouch){

		float sqrRemainingDistance = (this.transform.position - moveEnd).sqrMagnitude;

		if(sqrRemainingDistance > float.Epsilon && playerTouch == false){
			Vector3 newPosition = Vector3.MoveTowards(this.transform.position, moveEnd, (Mathf.Abs(speed)+boost) * Time.deltaTime);
			transform.position = newPosition;
		}
		else{

		}
	}

	//受伤
	public void Hurt(int damage){

		//生命值
		hp = hp - damage;

	}



	void IsDead(){
		if(hp <= 0){
//			gameObject.GetComponent<BoxCollider2D>().enabled = false;
//			gameObject.GetComponent<SpriteRenderer>().enabled = false;
//			Point.GetComponent<SpriteRenderer>().enabled = false;
//			Invoke("DeadDestory",0.5f);
			SoundManager.instance.PlaySingle("enemy",enemyDieClip);
			//实例化死亡特效
			if(dieEffect != null){
				Instantiate(dieEffect, this.transform.position, this.transform.rotation);
			}
			Destroy(this.gameObject);
		}
	}

//	void DeadDestory(){
//		Destroy(this.gameObject);
//	}
	
}
