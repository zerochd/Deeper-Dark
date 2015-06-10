using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public Transform playerGraphics;
	[Range(3, 10)]
	public float moveSpeed;
	[Range(0.25f, 0.5f)]
	public float attackCoolTime = 0.3f;

	private Rigidbody2D rigid2D;
	private Animator animator;
	private AnimatorStateInfo currentAnimatorInfo;
	private float x;
	private float y;
	private float timer;
	private bool isFaceRight = true;
	private bool _playerAttack = false;
	public bool PlayerAttack
	{
		get
		{
			return _playerAttack;
		}
	}

	void Awake()
	{
		rigid2D = GetComponent<Rigidbody2D>();

		if (playerGraphics != null)
		{
			animator = playerGraphics.GetComponent<Animator>();
			currentAnimatorInfo = animator.GetCurrentAnimatorStateInfo(0);
		}
	}


	void Update()
	{
		_playerAttack = false;
		timer += Time.deltaTime;

		// 角色移动
		x = Input.GetAxis("Horizontal") * moveSpeed;
		y = Input.GetAxis("Vertical") * moveSpeed;

		if (x > 0)
		{
			//TODO Animation walk
			if (!isFaceRight) 
			{
				Filp();
			}
		}
		else if (x < 0)
		{
			//TODO Animation walk
			if (isFaceRight)
			{
				Filp();
			}

		}
		else 
		{
			//TODO Animation idle
		}

		// 角色攻击
		if (Input.GetMouseButtonDown(0))
		{
			if (timer >= attackCoolTime)
			{
				animator.SetTrigger("attack");
				_playerAttack = true;
				timer = 0f;
			}
		}
	}
	
	void FixedUpdate()
	{
		rigid2D.velocity = new Vector2(x, y);
	}

	void Filp()
	{
		Vector3 scale = playerGraphics.localScale;
		scale.x = -scale.x;
		playerGraphics.localScale = scale;
		isFaceRight = !isFaceRight;
	}
}
