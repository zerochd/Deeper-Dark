using UnityEngine;
using System;

class EventActorDialog : EventActor {

	//------------------
	//变量

	//说话对象
	private BaseObject baseObject;

	private string text;




	//---------------------
	// 方法

	public EventActorDialog(BaseObject baseObject,string text)
	{
		this.baseObject = baseObject;
		this.text = text;
	}


	public override void start(EventManager evman)
	{
		Debug.Log("Object:"+ this.baseObject +"\n text:"+ this.text);
	}


	//在私有类里定义静态公共方法，将原本在创建实例的判断统一整合到一个方法中
	public static EventActorDialog CreateInstance(string[] parameters,GameObject manager,Event ev)
	{
		ev.setCurrentActorName("EventActorDialog");

		if(parameters.Length >= 2)
		{
			BaseObject bo = manager.GetComponent< ObjectManager >().find( parameters[ 0 ] );

			if(bo != null)
			{
				//分离出text
				string[] text = new string[parameters.Length-1];
				Array.Copy(parameters,1,text,0,text.Length);

				//将text合并成一条字符串，中间用\n分割
				EventActorDialog actor = new EventActorDialog(bo,String.Join("\n",text));
				return actor;                    
			}
			else
			{
				//当找不到baseObject时报错
				ev.debugLogError("can't find baseObject(" + parameters[0] + ")");
				return null;
			}
		}
		ev.debugLogError( "Out of Parameter" );
		return null;
	}
}
