using UnityEngine;
using System.Collections;

public class PlayerAnimatorController : MonoBehaviour {

	public float blinkFreqMax = 1.0f;
	public float blinkFreqMin = 4.0f;
	private float blinkCurrentCounter = 0.0f;

	private Animator anim;
	void Start () {
		anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!anim){return;};
		blinkCurrentCounter += Time.deltaTime;

		if (blinkCurrentCounter >= blinkFreqMin){
			float interval = Random.Range(blinkFreqMin, blinkFreqMax);

			if (blinkCurrentCounter >= interval){
				anim.SetTrigger("Blink");
				blinkCurrentCounter = 0.0f;
			}

		}
	
	}
}
