using UnityEngine;
using System.Collections;

public class PlayerAnimatorController : MonoBehaviour {

	// eyes control
	[Range(1.0f,6.0f)]
	public float AnimationSpeed = 3.0f;

	public float blinkFreqMax = 1.0f;
	public float blinkFreqMin = 4.0f;
	private float blinkCurrentCounter = 0.0f;

	// color effects control
	public GameObject PlayerModel;
	public GameObject PlayerModelAmimator;

	private SkinnedMeshRenderer meshRenderer;
	public Color effectsColorStart;
	public Color effectsColorEnd;
	[Range(0.0f,10.0f)]
	public float colorSwapTime = 1.0f;
	private float currentColorT = 0.0f;
	[Range(0.0f,10.0f)]
	public float emissionOscilation = 1.0f;
	public float defaultEmission = 5.0f;

	private float currentIdleStateTimer = 0.0f;
	public float IdleStateChangeTime = 10.0f;

	private Animator anim;
	private Animator modelAnimator;
	private PlayerController playerController;

	void Start () {
		anim             = gameObject.GetComponent<Animator>();
		meshRenderer     = PlayerModel.GetComponent<SkinnedMeshRenderer>();
		modelAnimator    = PlayerModelAmimator.GetComponent<Animator>();
		playerController = GlobalOptions.Player.GetComponent<PlayerController>();

		anim.SetFloat("AnimSpeed", AnimationSpeed);

		modelAnimator.SetFloat("IdleAnimSpeed" , AnimationSpeed*0.166f);
		modelAnimator.SetFloat("AnimSpeed066"  , AnimationSpeed*0.66f);
		modelAnimator.SetFloat("AnimSpeed198"  , AnimationSpeed*1.98f);
	}
	
	// Update is called once per frame
	void Update () {

		if (anim){
			blinkCurrentCounter += Time.deltaTime;

			if (blinkCurrentCounter >= blinkFreqMin){
				float interval = Random.Range(blinkFreqMin, blinkFreqMax);

				if (blinkCurrentCounter >= interval){
					anim.SetTrigger("Blink");
					blinkCurrentCounter = 0.0f;
				}

			};
		};
			

		if(meshRenderer){
			currentColorT = Mathf.PingPong(Time.time, colorSwapTime); 
			Color NewColor = Color.Lerp(effectsColorStart, effectsColorEnd, currentColorT);
			Material _lightMat = meshRenderer.material;
			float emission = 1.0f;
			if (emissionOscilation == 0){
				emission = defaultEmission;
			}else{
				emission = Mathf.PingPong (Time.time * emissionOscilation, 1.0f);			
			};

			Color finalColor = NewColor * Mathf.LinearToGammaSpace (emission);
			
			_lightMat.SetColor ("_EmissionColor", finalColor);


		};

		if(modelAnimator && !playerController.isMoving()){

			currentIdleStateTimer += Time.deltaTime;

			if (currentIdleStateTimer >= IdleStateChangeTime){
				float rnd = Random.Range(0.0f, 100.0f);

				if (rnd > 90.0f){
					//Debug.Log("Idle1");
					modelAnimator.SetTrigger("Idle1");
				}else if(rnd > 0.75){
					//Debug.Log("Idle0");
					modelAnimator.SetTrigger("Idle0");
				}
				currentIdleStateTimer = 0.0f;
			}

		}
	
	}
}
