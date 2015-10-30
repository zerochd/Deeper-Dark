using UnityEngine;
using System.Collections;
using System;


public class Enemy : AllCharacter
{

    protected bool hasSpawn;

	//标志是否第一次出现
	protected bool isBorn = true;

	private AIMode step = AIMode.NONE;
	private AIMode nextStep = AIMode.NONE;
	private bool foundPlayer = false;

	//警戒墙壁2个像素
	private float distanceFromWall = -2f;

	//攻击距离
	private float distanceFromPlayer = -0.4f;

	//冲刺速度
	private float boost = 1f;

	//玩家的变换,敌人一开始不知道
	private Transform playerTransform = null;

//	//玩家的被发现的位置
//	private Vector3 PlayerLastedPosition = Vector3.zero;

	//接触到玩家
	private bool playerTouch = false;

	//面向方向
	private bool facingRight = false;

	//接触墙壁
	private bool wallTouch = false;

	//设置是否能移动
	private bool canMove = true;

	//动画
	private Animator enemyAnim;
	
	//敌人类型
	public ENEMY_TYPE enemyType = ENEMY_TYPE.NORMAL;

	//表情状况
	public GameObject Point;

	//怪物攻击音效
	public AudioClip enemyAttackClip;

	//怪物被攻击音效
	public AudioClip enemyHurtClip;

	//怪物死亡音效
	public AudioClip enemyDieClip;

	//怪物出生下落的音效
	public AudioClip enemyFallClip;

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
				hp = 3;
				damage = 20;
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

//		IsDead();

        if (hasSpawn == false)
        {
            if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
            {
                Spawn();
            }
        }
        else
        {
			if (!GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main) && isBorn == false)
            {
//				Debug.Log ("Stop true");
				Stop();
//                Destroy(gameObject);
            }
			if(isBorn == false && canMove == true)
				StateMachine();
        }
	}
	

	//------------------------------
	//方法
	

	void Spawn()
	{
		hasSpawn = true;
//		Debug.Log ("spawn true");
		//开启一切组件
		if(isBorn == true){
//			Debug.Log ("am born");
			StartCoroutine(EnemyBorn());
		}
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
				if(true == hasSpawn && false == isBorn){
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
//				if(PlayerLastedPosition != Vector3.zero){
				if(this.playerTransform != null)
					EnemyDirectMove(this.playerTransform.position,playerTouch);
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

		//放置出生途中获取玩家
		if(isBorn == true)
			return;

		//传递玩家的Transform
		this.playerTransform = someoneTransform;

		if(someoneTransform != null){
			//设置标志变量
			foundPlayer = true;

			//显示惊叹标志
			Point.SetActive(true);

//			//传递玩家的位置
//			this.PlayerLastedPosition = someoneTransform.position;
		}
		else{
			foundPlayer = false;

			//显示惊叹标志
			Point.SetActive(false);
//
//			this.PlayerLastedPosition = Vector2.zero;
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

	//敌人出生效果
	IEnumerator EnemyBorn(){
		Vector3 bornVec = this.transform.position + Vector3.up * 10f;
		Vector3 endVec = this.transform.position;

		if(enemyFallClip!=null){
			AudioSource.PlayClipAtPoint(enemyFallClip,Vector3.zero,0.05f);
		}

		while(bornVec.y - endVec.y > float.Epsilon){
			bornVec.y -= 15f*Time.deltaTime;
			this.transform.position = bornVec;
			yield return null;
		}
		isBorn = false;
	}

	//角色转身
	public void Flip ()
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

	void OnCollisionEnter2D(Collision2D other){
		if(!other.gameObject.CompareTag("Player")){
			Flip();
		}
	}

	//敌人攻击距离判定
	IEnumerator EnemyAttackRange(){
		while(true){
			playerTouch = Physics2D.Linecast(this.transform.position,
			                                 new Vector2(this.transform.position.x + distanceFromPlayer,this.transform.position.y),
			                                 1 << LayerMask.NameToLayer("Player"));

			//如果接触到玩家的话,下一个玩家接触检测延迟0.8s+1帧,设置玩家位置为空，
			//设置动画为attack
			if(playerTouch && IsFoundPlayer() == true && playerTransform != null){

				Vector3 dir = (playerTransform.position - this.transform.position);
				
				Vector3 attackMoveEnd = playerTransform.position + dir*0.5f;

				//玩家被击飞移动
				playerTransform.GetComponent<Player>().HurtForce(attackMoveEnd);

				//减少玩家的血量
				playerTransform.GetComponent<Player>().SetHP(damage);

//				//重置玩家被找到的位置
//				PlayerLastedPosition = Vector3.zero;

				//开始攻击动画
				enemyAnim.SetTrigger("attack");

				//开始攻击音效
				SoundManager.Instance.PlaySingle(SOUND_CHANNEL.ENEMY,enemyAttackClip);

//				playerTouch = false;

				yield return new WaitForSeconds(0.8f);

				//重新寻找玩家
				HaveFoundPlayer(null);
			}

			yield return null;
		}
	}

	//敌人被击飞
	public IEnumerator EnemyHurtForce(Vector3 attackMoveEnd){
		
		//设置敌人不能动
		canMove = false;
		
		//获得移动距离
		float sqrRemainingDistance = (this.transform.position - attackMoveEnd).sqrMagnitude;
		float maxDistanceDelta = 4f ;
		
		while (sqrRemainingDistance > float.Epsilon && !wallTouch)
		{
			//Debug.Log("正在移动");
			Vector3 newPosition = Vector3.MoveTowards(this.transform.position, attackMoveEnd, maxDistanceDelta * Time.deltaTime);
			this.transform.position = newPosition;
			sqrRemainingDistance = (this.transform.position - attackMoveEnd).sqrMagnitude;
			//			Debug.Log ("sqrRemainingDistance"+sqrRemainingDistance);
			GetComponent<SpriteRenderer>().material.color = new Color(1f,1f,1f,0.3f);
			yield return null;
		}
		yield return new WaitForSeconds(0.3f);
		GetComponent<SpriteRenderer>().material.color = new Color(1f,1f,1f,1f);
		canMove = true;
	}

	void OnCollisionStay2D(Collision2D other){
		
		if(other.gameObject.tag.Equals("Wall")){
			wallTouch = true;
		}
	}
	
	void OnCollisionExit2D(Collision2D other){
		if(other.gameObject.tag.Equals("Wall")){
			wallTouch = false;
		}
	}

	//如果传入的是Transform,就是实时跟踪，不然就是定点跟踪
//	void EnemySmoothMove(Vector3 moveEnd){
//		//慢慢跟踪,normalized将向量变为标准向量
//		Vector3 dir = (moveEnd - this.transform.position).normalized;
//		this.transform.Translate(dir*1.5f*Time.deltaTime);
//	}

	//根据找到上一个玩家的位置进行跟踪
	void EnemyDirectMove(Vector3 moveEnd,bool playerTouch){

		if(!IsFoundPlayer())
			return;

		float sqrRemainingDistance = (this.transform.position - moveEnd).sqrMagnitude;

		if(sqrRemainingDistance > float.Epsilon && playerTouch == false){
			Vector3 newPosition = Vector3.MoveTowards(this.transform.position, moveEnd, (Mathf.Abs(speed)+boost) * Time.deltaTime);
			if(this.transform.position.x < moveEnd.x){
				if(facingRight == false){
					Flip();
				}
			}
			else{
				if(facingRight == true){
					Flip();
				}
			}

			transform.position = newPosition;
		}
		else{

		}
	}

	//受伤
	public void SetHP(int damage){

		//生命值
		hp = hp - damage;

		if(damage>0){
			if(enemyHurtClip!=null){
				//				AudioSource.PlayClipAtPoint(bossHurtClip,Vector2.zero);
				if(Player.instance!=null){
					Vector3 dir = (this.transform.position - Player.instance.transform.position).normalized;
					Vector3 attackMoveEnd = this.transform.position + dir*0.5f;
					StartCoroutine(EnemyHurtForce(attackMoveEnd));
				}
				SoundManager.Instance.PlaySingle(SOUND_CHANNEL.BACKGROUND,enemyHurtClip);
			}
		}

		if(hp <= 0){
			SoundManager.Instance.PlaySingle(SOUND_CHANNEL.ENEMY,enemyDieClip);
			//实例化死亡特效
			if(dieEffect != null){
//				Instantiate(dieEffect, this.transform.position, this.transform.rotation);
				GameObject go = Instantiate(dieEffect) as GameObject;
				go.transform.position = this.transform.position;
			}
			Destroy(this.gameObject);
		}

	}

//	private void IsDead(){
//		if(hp <= 0){
//			SoundManager.Instance.PlaySingle(SOUND_CHANNEL.ENEMY,enemyDieClip);
//			//实例化死亡特效
//			if(dieEffect != null){
//				Instantiate(dieEffect, this.transform.position, this.transform.rotation);
//			}
//			Destroy(this.gameObject);
//		}
//	}

//	void DeadDestory(){
//		Destroy(this.gameObject);
//	}
	
}
