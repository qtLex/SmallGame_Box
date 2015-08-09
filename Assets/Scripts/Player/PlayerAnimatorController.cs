using UnityEngine;
using System.Collections;

public class PlayerAnimatorController : MonoBehaviour {

	// eyes control
	public float blinkFreqMax = 1.0f;
	public float blinkFreqMin = 4.0f;
	private float blinkCurrentCounter = 0.0f;

	// color effects control
	public GameObject PlayerModel;
	private SkinnedMeshRenderer meshRenderer;
	public Color effectsColorStart;
	public Color effectsColorEnd;
	[Range(0.0f,10.0f)]
	public float colorSwapTime = 1.0f;
	private float currentColorT = 0.0f;
	[Range(0.0f,10.0f)]
	public float emissionOscilation = 1.0f;
	public float defaultEmission = 5.0f;


	private Animator anim;

	void Start () {
		anim         = gameObject.GetComponent<Animator>();
		meshRenderer = PlayerModel.GetComponent<SkinnedMeshRenderer>();
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
	
	}
}
