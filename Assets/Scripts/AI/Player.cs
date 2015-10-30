using UnityEngine;
using System.Collections;

public class Player : AllCharacter {


	private static readonly string IdleState = "Base Layer.player-idle";
	private static readonly string HurtState = "Base Layer.player-hurt";
//	private static readonly string AttackState = "Base Layer.player-attack";

	public static Player instance = null;

	//装备
	public Transform equipment;

    int exp;                        //经验值
    int level;                      //等级
	public int armor{get;set;}		//护甲
	//朝向右边
	public bool facingRight 	= true;

    //人物移动至下一个房间的速度
    float inverseTime;

	//能否控制
	bool canControl 	= true;

	//能否移动
    bool canMove 		= true;

	//能否攻击
	bool canAttack		= true;

	//是否碰触墙壁
	bool wallTouch 		= false;

	//玩家动画
	Animator playerAnim;

	//攻击判定区域
	CircleCollider2D attackPoint;

	//无敌时间
	float superTime;

//	//是否碰触墙壁
//	public bool wallTouch 	= false;

	public AudioClip playerAttackClip;

	public AudioClip playerHurtClip;

	public AudioClip playerDieClip;

	//死亡特效
	public GameObject dieEffect;

//	public bool AmSuper{
//		get
//		{
//			return superTime >= 0f;
//		}
//	}

	//方法

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		hp = 100;
		damage = 1;
		speed = 2.5f;
		armor = 1;

        inverseTime = 10f;

		attackPoint = GameObject.Find("attackPoint").GetComponent<CircleCollider2D>();
		playerAnim = GetComponent<Animator>();
//		GetComponent<Rigidbody2D>().AddForce(Vector2.right*500f);
	}
	
	// Update is called once per frame
	void Update () {

		if(Globe.game_state != Globe.GAME_STATE.RUNNING){
			//停止速度
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
			return;
		}

		//逐渐减少superTime，当小于0时停止Super状态
		if(superTime >= 0f){
			superTime -= Time.deltaTime;
		}
		else{
//			SetControl(true);
			SetSuper(false);
		}

		if(canControl){
			canMove = IsAnimState(IdleState,playerAnim)||IsAnimState(HurtState,playerAnim);

	        if (canMove)
	        {
				//当能移动的情况下正是没有攻击的情况
				if(attackPoint.isActiveAndEnabled)
					attackPoint.enabled = false;
	            Move();
				if(canAttack){
					Attack();
				}
	        }
		}
	}

	public int GetDamage(){
		return this.damage;
	}

	public void SetDamage(int e_damage){
		this.damage = e_damage;
	}

	public void SetControl(bool value){
		GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
		canControl = value;
	}

	//设置animTrigger
	public void SetAnimTrigger(string name){
		if(name != null){
			playerAnim.SetTrigger(name);
		}else{
			Debug.LogError("no this anim");
		}
	}


	//检查是否为该动画状态
	bool IsAnimState(string animatorStateInfo,Animator anim){
		if(anim == null){
			Debug.LogWarning ("don't have anim");
			return false;
		}

		AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
		if (stateinfo.fullPathHash == Animator.StringToHash(animatorStateInfo))
			return true;
		return false;
	}


	//玩家移动
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        GetComponent<Rigidbody2D>().velocity = new Vector2(h * speed, v * speed);
		if( h > 0 && !facingRight)
			Flip();
		else if(h < 0 && facingRight)
			Flip();
    }

	//玩家水平翻转方向
	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = this.transform.localScale;
		theScale.x *= -1;
		this.transform.localScale = theScale;
	}

	//攻击
	void Attack(){
		if(Input.GetButtonDown("Attack")){

			playerAnim.SetTrigger("attack");

			//停止控制
			canMove = false;

			//停止速度
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

			//开启攻击区域触发器
			attackPoint.enabled = true;

//			Debug.Log ("SoundManager.Instance:"+SoundManager.Instance.name);

			//播放攻击音效
			SoundManager.Instance.PlaySingle(SOUND_CHANNEL.PLAYER,playerAttackClip);

			//关闭无敌
			if(superTime > float.Epsilon){
				SetSuper(false);
			}
		}
	}


	//移动至下一个房间
    public void MoveNextRoom(Vector3 cameraVector,Vector3 DoorVector)
    {
//        Debug.Log("正在进入房间");
        Vector3 playerMoveEnd;

        //float yMove = (DoorVector.y - transform.position.y) ;
        //float xMove = (DoorVector.x - transform.position.x);
        //初始化y跟x的移动距离
        float yMove = 0f ;
        float xMove = 0f;
        
        //如果是上下移动
        if (Mathf.Abs(DoorVector.x - cameraVector.x) < float.Epsilon)
        {
            inverseTime = 2.75f;
            yMove += Mathf.Sign(DoorVector.y - cameraVector.y)*1.5f;
        }
        //如果是左右移动
        if (Mathf.Abs(DoorVector.y - cameraVector.y) < float.Epsilon)
        {
            inverseTime = 1.25f;
            xMove += Mathf.Sign(DoorVector.x - cameraVector.x)*1.5f;
        }
//        Debug.Log("x: "+xMove + " y: " + yMove);
        //设定移动终点
        playerMoveEnd = transform.position + new Vector3(xMove, yMove, 0);
        //不能让玩家控制
		canControl = false;
        //开启移动协程
        StartCoroutine(PlayerSmoothMove(playerMoveEnd));
    }

    //玩家平滑移动
    IEnumerator PlayerSmoothMove(Vector3 moveEnd)
    {
//        Debug.Log(inverseTime);
        //求出从起始点到终点的距离
        float sqrRemainingDistance = (transform.position - moveEnd).sqrMagnitude;
//        Debug.Log("移动开始"); 
        //如果距离大于0
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Debug.Log("正在移动");
            Vector3 newPosition = Vector3.MoveTowards(transform.position, moveEnd, inverseTime * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - moveEnd).sqrMagnitude;
            yield return null;
        }      
//            Debug.Log("已经移动完成");
        //将玩家的触发器改为碰撞器
           //GetComponent<BoxCollider2D>().isTrigger = false;
           GetComponent<CircleCollider2D>().isTrigger = false;
           canControl = true;
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

	//设置无敌状态(通过改变alpha跟当前gameobject跟子object的layer层)
	public void SetSuper(bool value){
		if(value){
//			this.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f,1f,1f,0.3f);
			this.gameObject.layer = LayerMask.NameToLayer("Super");
			this.gameObject.GetComponentInChildren<BoxCollider2D>().gameObject.layer = LayerMask.NameToLayer("Super");
			//无敌时间设置为2秒
			superTime = 2f;
		}
		else{
//			this.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f,1f,1f,1f);
			this.gameObject.layer = LayerMask.NameToLayer("Player");
			this.gameObject.GetComponentInChildren<BoxCollider2D>().gameObject.layer = LayerMask.NameToLayer("Player");
		}
	}

	public void HurtForce(Vector3 attackMoveEnd){
		StartCoroutine(EnemyAttackForce(attackMoveEnd));
	}

	public IEnumerator EnemyAttackForce(Vector3 attackMoveEnd){

		//设置玩家不能动
		SetControl(false);

		//获得移动距离
		float sqrRemainingDistance = (this.transform.position - attackMoveEnd).sqrMagnitude;
		float maxDistanceDelta = 4f ;
		
		//设置玩家的动画状态为hurt
		SetAnimTrigger("hurt");
		
		SetSuper(true);
		
		while (sqrRemainingDistance > float.Epsilon && !wallTouch)
		{
			//Debug.Log("正在移动");
			Vector3 newPosition = Vector3.MoveTowards(this.transform.position, attackMoveEnd, maxDistanceDelta * Time.deltaTime);
			this.transform.position = newPosition;
			sqrRemainingDistance = (this.transform.position - attackMoveEnd).sqrMagnitude;
//			Debug.Log ("sqrRemainingDistance"+sqrRemainingDistance);
			
			yield return null;
		}
		yield return new WaitForSeconds(0.3f);
		//设置玩家可以操控
		SetControl(true);
		wallTouch = false;
	}

	public void AttackEnable(){
		canAttack = true;
	}

	public void AttackDisable(){
		canAttack = false;
	}

	public int GetHP(){
		return hp;
	}

	public void SetHP(int damage){
		//如果是扣血播放受伤音效
		if(damage>0){
//			if(playerHurtClip!=null)
//				AudioSource.PlayClipAtPoint(playerHurtClip,Vector2.zero);
			hp = hp - (int)(damage - damage * armor/10);
		}
		else{
			hp -= damage;
		}
		//防止生命值溢出,并死亡
		if(hp < 0){
			hp = 0;
			PlayerDie();
		}
		if(hp > 100)
			hp = 100;
	}

	void PlayerDie(){
		//打出你死了
		TextManager.Instance.showTint("你已经死了",SHOW_TYPE.FADEIN);
		TextManager.Instance.MoveTint(new Vector3(0,-20f));
//		Debug.Log ("Globe.game_state"+Globe.game_state);
		//游戏结束
		Globe.game_state = Globe.GAME_STATE.OVER;
		if(dieEffect != null){
//			Debug.Log("dieEffect",dieEffect);
			Instantiate(dieEffect, this.transform.position, this.transform.rotation);
		}
		if(playerDieClip != null)
			SoundManager.Instance.PlaySingle(SOUND_CHANNEL.ENEMY,playerDieClip);
		Destroy(this.gameObject);
	}

	public void ResetPosition(){
		
	}
}
