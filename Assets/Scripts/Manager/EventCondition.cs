using UnityEngine;
using System.Collections;

/// <summary>
/// Event condition.事件发生条件
/// </summary>
public class EventCondition : MonoBehaviour {

	//----------------
	//变量

	//角色
	private BaseObject baseObject;
	//标记名称
	private new string name;
	//触发事件所需的值
	private string compareValue;


	//---------------------
	// 方法

	 public EventCondition( BaseObject baseObject, string name, string compareValue )
	{
		this.baseObject       = baseObject;
		this.name         	  = name;
		this.compareValue     = compareValue;
	}


	public bool evaluate(){
		string value = this.baseObject.getVariable(name);
		if(value == null){
			value = "0";
		}

		return compareValue == value;
	}

}
