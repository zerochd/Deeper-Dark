using UnityEngine;
using System.Collections;

public enum SOUND_CHANNEL{
	NONE = -1,
	PLAYER = 0,
	ENEMY = 1,
	BACKGROUND
}

public class SoundManager : UnitySingleton<SoundManager> {

	public AudioSource enemySource;
	public AudioSource playerSource;
	public AudioSource bkSoundSource;

//	public AudioSource musicSource;
//	public static SoundManager instance = null;
	
	public float lowPitchRange = 0.95f;
	public float highPitchRange = 1.05f;
	
//	// Use this for initialization
//	void Awake () {
//		if (instance == null)
//			instance = this;
//		else if (instance != this)
//			Destroy (gameObject);
//		DontDestroyOnLoad (gameObject);
//		
//	}
	
	public void PlaySingle(SOUND_CHANNEL soundType,AudioClip clip){
		switch(soundType)
		{

			case SOUND_CHANNEL.PLAYER:
				playerSource.clip = clip;
				playerSource.Play();
				break;

			case SOUND_CHANNEL.ENEMY:
				enemySource.clip = clip;
				enemySource.Play();
				break;

			case SOUND_CHANNEL.BACKGROUND:
				bkSoundSource.clip = clip;
				bkSoundSource.Play();
				break;

			default:
			Debug.Log ("no this soundType");break;

		}

	}
	
	public void RandomizeSfx(SOUND_CHANNEL soundType,params AudioClip[] clips){
		int randomIndex = Random.Range (0, clips.Length);

//		float randomPitch = Random.Range (lowPitchRange, highPitchRange);

		PlaySingle(soundType,clips [randomIndex]);
		
	}
}
