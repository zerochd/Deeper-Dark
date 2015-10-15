using UnityEngine;
using System.Collections;

public interface Item {
	
	void start ();
	
	string name{get;set;}

	string description{get;set;}

	bool only{get;set;}
	

}
