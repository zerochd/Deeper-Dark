using UnityEngine;
using System;

public class Event {

	//-------------------------------
	// 变量

	//事件发生对象
	private string[] targets;

	//事件发生条件
	private EventCondition[] conditions;

	//事件的指令
	private string[][] actions;

	//是否是开场白
	private bool isPrologue;

	//是否继续评价
	private bool m_continue;

	//事件的名字
	private new string name;

	//存放事件开始的行号
	private int lineNumber;

	//存放一系列action的行号
	private int[] actionLineNumbers = null;
	
	//当前步骤
	private STEP step = STEP.NONE;

	//下一个步骤
	private STEP nextStep = STEP.EXEC_ACTOR;
	
	//现在实行的步骤
	private EventActor currentActor = null;

	//下一次实行的步骤
	private int nextActorIndex = 0;

	//保存最后的步骤名称
	private string actorName ="";

	//保存最后步骤的下标
	private int actorIndex = 0;

	private enum STEP
	{
		NONE = -1,
		EXEC_ACTOR = 0,//执行步骤
		WAIT_INPUT,
		DONE,
		NUM
	}


	// ------------------------------------------------------------------------
	// 方法
	
	public Event(){
	}

	//构造事件
	public Event(string[] targets,EventCondition[] conditions,
	             string[][] actions,bool isPrologue,bool doContinue,string name)
	{
		Array.Sort(targets);
		this.targets = targets;
		this.conditions = conditions;
		this.actions    = actions;
		this.isPrologue = isPrologue;
		this.m_continue = doContinue;
		this.name       = name;

//		Debug.Log ("name:"+name);
	}

	public static Event getEvent(){
		return new Event();
	}

	//public
	//判断是否发生对象跟接触物体相同
	public bool evaluate(string[] contactingObjects, bool isPrologue)
	{
//		Debug.Log ("name:"+this.name);

		if(isPrologue)
		{
			if(!this.isPrologue) return false;
		}
		else
		{
			Array.Sort(contactingObjects);

			if(this.targets.Length == contactingObjects.Length)
			{
				for(int i = 0;i <this.targets.Length;i++)
				{
					if(this.targets[i] == "*")continue;

					if(this.targets[i] != contactingObjects[i])
					{
						return false;
					}
				}
			}
			else
			{
				return false;
			}
		}

		foreach(EventCondition ec in this.conditions)
		{
			if(!ec.evaluate()) return false;
		}

		return true;

	}

	//自定义开始初始化方法
	public void start()
	{
		this.step = STEP.NONE;
		this.nextStep = STEP.EXEC_ACTOR;
		this.currentActor = null;
		this.nextActorIndex = 0;
	}

	public void execute(EventManager evman)
	{
		switch(this.step)
		{
			case STEP.WAIT_INPUT:
			{
				if(Input.GetMouseButtonDown(0))
			   	{
					this.currentActor = null;
					this.nextStep = STEP.EXEC_ACTOR;
				}
		
			}
			break;

			case STEP.EXEC_ACTOR:
			{
				if(this.currentActor.isDone())
				{
					if(this.currentActor.isWaitClick(evman))
					{
						
					}
				}
			}
			break;

		}//switch(this.step)

		//-------------------------------

		while(this.nextStep != STEP.NONE)
		{
			this.step = this.nextStep;
			this.nextStep = STEP.NONE;

			switch(this.step)
			{
				case STEP.EXEC_ACTOR:
				{
					//清空当前步骤
					this.currentActor = null;

					//遍历actions直到找到currentActor
					while(this.nextActorIndex < this.actions.Length)
					{
						this.currentActor = createActor(evman,this.nextActorIndex);
						this.nextActorIndex++;

						if(this.currentActor != null)
						{
							break;
						}
					}

					if(this.currentActor != null)
					{
						this.currentActor.start(evman);
					}
					else{
						this.nextStep = STEP.NONE;
					}
				}
				break;
			}
		}

		//-----------------------------------------------

		switch(this.step)
		{
			case STEP.EXEC_ACTOR:
			{
				this.currentActor.execute(evman);
			}
			break;
		}

	}


	//设置事件的活动行号
	public void setActionLineNumbers(int[] numbers)
	{
		this.actionLineNumbers = numbers;
	}

	//设置行数
	public void setLineNumber(int n)
	{
		lineNumber = n;		
	}

	//判断事件是否结束
	public bool isDone()
	{
		return this.step == STEP.DONE;
	}

	//判断是否继续
	public bool doContinue()
	{
		return this.m_continue;
	}

	//获得事件名称
	public string getEventName()
	{
		return this.name;
	}

	//设置当前步骤名称
	public void setCurrentActorName(string name)
	{
		this.actorName = name;
	}

	//报错
	public void debugLogError(string message)
	{
		Debug.LogError(this.actorName + ":" + message + "at" + this.actionLineNumbers[this.actorIndex] + ".");
	}


	//创建步骤
	private EventActor createActor(EventManager evman,int index)
	{
		//每行指令第一个字符串为该步骤的种类,剩下的为参数
		string[] action = this.actions[index];
		string kind = action[0];
		string[] parameters = new string[action.Length -1];
		Array.Copy (action,1,parameters,0,parameters.Length);

		this.actorName ="";
		this.actorIndex = index;
		EventActor actor = null;


		switch(kind.ToLower())
		{
			//显示文本
			case "text":
				actor = EventActorText.CreateInstance(parameters,evman.gameObject);
				break;
			//对话
			case "dialog":
				actor = EventActorDialog.CreateInstance( parameters, evman.gameObject, this );
				break;
			default:
				Debug.LogError("Invalid command \"" + kind +"\"");
				break;
		}
		return actor;

	}

}
