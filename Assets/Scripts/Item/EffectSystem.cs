using UnityEngine;
using System.Collections;

public class EffectSystem : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		//当检测到粒子停止播放的时候，销毁该实例
		if (GetComponent<ParticleSystem>().isStopped == true)
		{
			Destroy(this.gameObject);
		}
	}
}
