using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class EventManager : MonoBehaviour {

	//------------------------------
	//变量
	

	//是否创建事件
	private bool hasCreatedEvents = false;

	//一系列事件
	private Event[] events = new Event[0];

	//接触对象存放数组
	private List<string> contactingObjects = new List<string>();

	//初始化标示符
	private bool isPrologue = false;

	//是否正在执行
	private bool isExecuting = false;

	//
	private bool isStartedByContact = false;

	//活动事件下标
	private int activeEventIndex = -1;

	//下一个事件的下标
	private int nextEvaluatingEventIndex = -1;

	//现在的状态
	private STEP step = STEP.NONE;

	//下一个状态
	private STEP nextStep = STEP.NONE;

	//当前活动中的事件
	private Event activeEvent = null;

	//下一批读取的脚本名称
	private string[] nextScirptFiles = null;



	//事件状态枚举
	private enum STEP
	{
		NONE = -1,
		LOAD_SCRIPT = 0,//载入脚本
		WAIT_TRIGGER,//等待触发事件
		START_EVENT,//事件开始
		EXECUTE_EVENT,//事件执行
		NUM
	}

	//第一次读取的脚本文件
	public string[] firstScriptFiles = new string[0];


	//--------------------------------
	//方法


	//private
	private void Start()
	{
		if(firstScriptFiles.Length == 0)
		{
			firstScriptFiles = GlobalParam.getInstance().getStartScriptFiles();
		}

		//初始化
		this.isPrologue = true;
		//初始化下一步为读取事件自定义脚本
		this.nextStep = STEP.LOAD_SCRIPT;
		this.nextScirptFiles = firstScriptFiles;
		this.nextEvaluatingEventIndex = -1;

	}

	private void Update()
	{
		if(this.nextStep == STEP.NONE)
		{
			switch(this.step)
			{
				case STEP.LOAD_SCRIPT:
				{
					if(hasCreatedEvents)
					{
						this.isPrologue = true;
						this.isExecuting = false;
						this.activeEvent = null;
						this.activeEventIndex = -1;
						this.nextEvaluatingEventIndex = -1;
						this.nextScirptFiles = null;

						this.nextStep = STEP.WAIT_TRIGGER;
					}
				}
				break;

				case STEP.WAIT_TRIGGER:
				{
					if(isPrologue)
						this.nextStep = STEP.START_EVENT;
					else
					{
						if(this.contactingObjects.Count > 0 )
						{
							this.isStartedByContact = true;
							this.nextStep = STEP.START_EVENT;
						}
					}
				}
				break;

				//开始事件
				case STEP.START_EVENT:
				{
					string[] contactingObjectsArray = contactingObjects.ToArray();
					
					int i ;
					
					for(i = this.activeEventIndex+1;i < this.events.Length; i++)
					{
						Event ev = this.events[i];
					Debug.Log ("found ev:"+ev.getEventName());
						if(ev.evaluate(contactingObjectsArray,this.isPrologue))
						{
							break;
						}
					}

					if( i < events.Length)
					{
						this.activeEvent = this.events[i];
						this.activeEventIndex = i;
						this.nextStep = STEP.EXECUTE_EVENT;
					}
					else
					{
						this.activeEvent = null;
						this.activeEventIndex = -1;
						this.isPrologue = false;

						//如果还有脚本，继续加载
						if(this.nextScirptFiles != null)
						{
							this.nextStep = STEP.LOAD_SCRIPT;
						}
						else
						{
							this.nextStep = STEP.WAIT_TRIGGER;
						}
					}
					
				}
				break;

				//执行事件
				case STEP.EXECUTE_EVENT:
				{
					if(this.activeEvent.isDone())
					{
						do
						{
							if(this.nextEvaluatingEventIndex >= 0)
							{
								Event ev = this.events[this.nextEvaluatingEventIndex];
								if(	ev.evaluate(this.contactingObjects.ToArray(),this.isPrologue))
								{
									activeEvent = ev;
									activeEventIndex = nextEvaluatingEventIndex;
									nextStep = STEP.EXECUTE_EVENT;
									break;
								}
							}
							
							if(this.activeEvent.doContinue())
								this.activeEventIndex = this.events.Length;
							
							this.nextStep = STEP.START_EVENT;
						}
						while(false);
						this.nextEvaluatingEventIndex = -1;
					}
				}
				break;

			}//switch(this.step)
		}//if(nextStep == STEP.NONE)

		//------------------------------------------

		while(this.nextStep != STEP.NONE)
		{
			this.step = this.nextStep;
			this.nextStep = STEP.NONE;

			switch(this.step)
			{
				case STEP.LOAD_SCRIPT:
				{
					this.isExecuting = false;
					this.hasCreatedEvents = false;
					StartCoroutine("createEventsFromFile",this.nextScirptFiles);
				}
				break;

				case STEP.WAIT_TRIGGER:
				{
					this.isExecuting = false;
					this.contactingObjects.Clear();
				}
				break;

				case STEP.EXECUTE_EVENT:
				{
					this.isExecuting = true;
				    this.isStartedByContact = false;
					this.activeEvent.start();
				}
				break;

			}
		}//while(this.nextStep != STEP.NONE)

		switch ( this.step )
		{
			case STEP.EXECUTE_EVENT:
			{
				if ( this.activeEvent != null )
				{
					this.activeEvent.execute( this );
				}
			}
			break;
		}
	}


	//读取文件生成一系列事件
	private IEnumerator createEventsFromFile(string[] fileNames)
	{
		if(fileNames.Length > 0)
		{
			List<string> linesOfAllFiles = new List<string>();

			//将所有file的字符串行填入LinesOfAllFile
			foreach(string file in fileNames)
			{
				yield return StartCoroutine(loadFile(file,linesOfAllFiles));
			}

			ScriptParse parser = new ScriptParse();

			events = parser.parseAndCreateEvents(linesOfAllFiles.ToArray());
			Debug.Log("Created "+ this.events.Length.ToString() + " events");

		}
		else
		{
			events = new Event[0];
		}

		this.hasCreatedEvents = true;
	}

	//读取文档
	private IEnumerator loadFile(string fileName,List<string> allLines)
	{
		string[] lines;

		if(Application.isWebPlayer)
		{
			WWW www = new WWW(fileName);
			yield return www;
			lines = www.text.Split('\r','\n');
		}
		else
		{
			if(!File.Exists(fileName))
			{
				Debug.LogError("File Open Error" + fileName);
			}

			lines = File.ReadAllLines(fileName);
		}
		allLines.AddRange(lines);
	}

	//public
	//开始事件
	public void startEvent(int eventIndex)
	{
		this.activeEvent = this.events[eventIndex];
		this.activeEventIndex = eventIndex;
		this.nextStep = STEP.EXECUTE_EVENT;

	}

	public void addContactingObject( BaseObject baseObject )
	{
		string name = baseObject.name;
		if ( !this.contactingObjects.Contains( name ) )
		{
			this.contactingObjects.Add( name );
		}
	}


}
