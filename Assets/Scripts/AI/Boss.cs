using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	int damage = 30;
	
	public const int MAX_HP = 20;

	int hp = MAX_HP;

	bool hasSpawn = false;
	bool isBorn = true;
	bool die = false;

	//面向方向
	private bool facingRight = false;

	//移动速度
	private float speed = 0.8f;

	//找到玩家
	private bool foundPlayer = false;

	//玩家接触
	private bool playerTouch = false;

	//AIMODE
	private AIMode step = AIMode.NONE;
	private AIMode nextStep = AIMode.NONE;

	//STATE_TYPE
	private STATE_TYPE stateType = STATE_TYPE.NONE;

	//玩家的变换,敌人一开始不知道
	private Transform playerTransform = null;
	
	//boss动画
	private Animator bossAnim;

	//攻击间隔
	private float attackCD;

	//落石头攻击
	public Rock attckRock;

	//boss攻击音效
	public AudioClip bossAttackClip;

	//boss出现音效
	public AudioClip bossFallClip;

	//boss受伤音效
	public AudioClip bossHurtClip;

	//boss移动音效
	public AudioClip bossFootClip;

	//
	public Color AngryColor;

	//AI状态
	private enum AIMode{
		NONE = -1,
		WAIT = 0,//等待
		SEARCH = 1,//搜索
		ATTACK//攻击
	}

	private enum STATE_TYPE{
		NONE = -1,
		BEGIN = 0,	//初始状态
		Angry = 1	//愤怒状态
	}

	void Start(){
		this.transform.position += new Vector3(0.8f,-1f);
		nextStep = AIMode.WAIT;
		stateType = STATE_TYPE.BEGIN;
		hasSpawn = false;
		bossAnim = GetComponent<Animator>();
	}

	void Update(){

		if(attackCD > 0f){
			attackCD -= Time.deltaTime;
		}

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
				Debug.Log ("Stop true");
				Stop();
				//                Destroy(gameObject);
			}
			//开启状态机
			if(attackCD <= 0f && die == false){
				playerTouch = false;
				StateMachine();
			}
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

		}
		else{
			foundPlayer = false;
		}
		
	}

	//开始出现
	void Spawn()
	{

		if(Player.instance.transform.position.x > this.transform.position.x){
			if(facingRight == false){
				Flip();
			}
		}

		hasSpawn = true;
		//开启一切组件
		if(isBorn == true){
			//boss出现
			StartCoroutine(BossBorn());
		}
	}

	void StateMachine(){
		//判断是否迁移到下一个状态
		if(this.nextStep == AIMode.NONE)
		{
			switch(this.step){
			case AIMode.WAIT:{
				if(false == isBorn){
					nextStep = AIMode.SEARCH;
				}
			}
				break;
			case AIMode.SEARCH:
				if(true == IsFoundPlayer()){
					nextStep = AIMode.ATTACK;
				}
				break;
			case AIMode.ATTACK:
				if(false == IsFoundPlayer()){
					nextStep = AIMode.SEARCH;
				}
				break;
			}
		}
		
		//状态迁移初始化
		if(this.nextStep != AIMode.NONE)
		{
			switch(this.nextStep){
			case AIMode.WAIT:{

			}
				break;
			case AIMode.SEARCH:{
//				GetComponentInChildren<SearchBOSSAI>().enabled = true;
			}
				break;
			case AIMode.ATTACK:{
//				Debug.Log ("attack");
//				GetComponentInChildren<SearchBOSSAI>().enabled = false;
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
			//搜索模式
		case AIMode.SEARCH:{
		}
			break;
			
			//攻击模式，往玩家的位置移动
		case AIMode.ATTACK:{
			if(playerTransform!=null){
				BossDirectMove(playerTransform.position);
			}
		}
			break;
			
		}
	}

	//停止活动
	void Stop()
	{
		hasSpawn = false;
	}

	//Boss出生效果
	IEnumerator BossBorn(){

//		Debug.Log ("bossborn");
		Vector3 bornVec = this.transform.position + Vector3.up * 10f;
		Vector3 endVec = this.transform.position;
		
		if(bossFallClip!=null){
			AudioSource.PlayClipAtPoint(bossFallClip,Vector3.zero,0.05f);
		}
		
		while(bornVec.y - endVec.y > float.Epsilon){
			bornVec.y -= 10f * Time.deltaTime;

			this.transform.position = bornVec;
			yield return null;
		}
		//		Debug.Log ("end born");
		bossAnim.SetTrigger("gesture");
		GetComponent<BoxCollider2D>().isTrigger = false;
		Globe.MeetBoss = true;
		Vector3 textVec = Camera.main.WorldToScreenPoint(this.transform.FindChild("head").position);
		TextManager.Instance.ShowBossDialog("你不能通过这里",textVec);
		yield return new WaitForSeconds(1.3f);
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
		if(!facingRight)
			transform.position += Vector3.right * 1.27f * (theScale.y/1.5f);
		else
			transform.position -= Vector3.right * 1.27f * (theScale.y/1.5f);
//		this.transform.localScale = theScale;
		this.transform.localScale = theScale;
		speed *= -1f;
	}



	
	//根据找到上一个玩家的位置进行跟踪
	void BossDirectMove(Vector3 moveEnd){
		
		if(!IsFoundPlayer())
			return;
		float sqrRemainingDistance = (this.transform.position - moveEnd).sqrMagnitude;
//		Debug.Log ("sqrRemainingDistance"+sqrRemainingDistance);
		if(sqrRemainingDistance > float.Epsilon && false == playerTouch){
			//boss移动
			bossAnim.SetBool("move",true);
		
			Vector3 newPosition = Vector3.MoveTowards(this.transform.position, moveEnd, (Mathf.Abs(speed)) * Time.deltaTime);
//			Debug.Log ("newPosition"+newPosition);
			this.transform.position = newPosition;
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
			if(sqrRemainingDistance < 4f){

				//BOSS攻击
				Debug.Log("attack");
				playerTouch = true;
				bossAnim.SetTrigger("attack");
				bossAnim.SetBool("move",false);
				HaveFoundPlayer(null);
				//开始CD冷却
				attackCD = 3f;
			}	

		}
		else{
			
		}

	}

	//获得随机落石Vec
	public Vector3 GetRandomRock(){
		float nowCenterX = Camera.main.transform.position.x;
		float nowCenterY = Camera.main.transform.position.y;
		float x = (float)Random.Range(nowCenterX-4f,nowCenterX+4f);
		float y = (float)Random.Range(nowCenterY-1f,nowCenterY+1f);
		return new Vector3(x,y,0);
	}
	

	public int GetDamage(){
		return damage;
	}

	public void SetHP(int damage){

		if(damage>0){
			if(bossHurtClip!=null){
//				AudioSource.PlayClipAtPoint(bossHurtClip,Vector2.zero);
				SoundManager.Instance.PlaySingle(SOUND_CHANNEL.BACKGROUND,bossHurtClip);
			}
		}
		//生命值
		hp = hp - damage;

		// 
		if(hp <= MAX_HP/2){
			//		stateType = STATE_TYPE.Angry;
			bossAnim.SetTrigger("gesture");
		}

		if(hp <= 0){
//			Vector3 textVec = Camera.main.WorldToScreenPoint(this.transform.FindChild("head").position);
//			TextManager.Instance.ShowBossDialog("我还会回来的！",textVec);
			bossAnim.SetTrigger("die");
			die = true;
		}
		
	}

	//-------------------------------------------------------
	//animation Events

	//boss-attack
	//攻击晃动
	public void AttackEffect(){
		iTween.ShakeRotation(Camera.main.gameObject,Vector3.forward*8f,0.5f);
		
		if(bossAttackClip!=null){
			SoundManager.Instance.PlaySingle(SOUND_CHANNEL.BACKGROUND,bossAttackClip);
		}
		
		switch(stateType){
		case STATE_TYPE.BEGIN:{
			if(attckRock!=null){
				Instantiate(attckRock,GetRandomRock(),this.transform.rotation);
			}
		}break;
		case STATE_TYPE.Angry:{
			if(attckRock!=null){
				Instantiate(attckRock,GetRandomRock(),this.transform.rotation);
				Instantiate(attckRock,GetRandomRock(),this.transform.rotation);
			}
		}break;
		default:Debug.LogError("NO THIS TYPE!");break;
		}
		
	}

	//boss-ges
	//愤怒
	void Angry(){
		if(hp <= MAX_HP/2){
			speed = 1.2f;
			stateType = STATE_TYPE.Angry;
			GetComponent<SpriteRenderer>().material.color = AngryColor;
			transform.localScale += Vector3.one * 0.1f;
			attackCD += 1f;
		}

	}

	//boss-move
	//移动音效
	void MoveSound(){
		if(bossFootClip!=null)
			AudioSource.PlayClipAtPoint(bossFootClip,Vector3.zero);
	}


	//boss-die 
	//死亡
	void Dead(){
		attackCD += 10f;
		//开门
		Globe.MeetBoss = false;
		TextManager.Instance.showTint("你击杀了怪物",SHOW_TYPE.FADEIN);
		TextManager.Instance.MoveTint(new Vector3(0,-20f));
		Globe.game_state = Globe.GAME_STATE.OVER;
		Destroy(this.gameObject);
	}

}
