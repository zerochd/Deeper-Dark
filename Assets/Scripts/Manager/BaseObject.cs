using UnityEngine;
using System.Collections.Generic;

public class BaseObject : MonoBehaviour {

	//保存参数的字典
	private Dictionary<string,string> variables = new Dictionary<string,string>();

	public string getVariable(string name){

		string value;
		return this.variables.TryGetValue(name,out value)? value : null;
	}



}
