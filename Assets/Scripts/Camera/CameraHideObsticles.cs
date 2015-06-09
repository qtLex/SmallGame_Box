using UnityEngine;
using System.Collections;

public class CameraHideObsticles : MonoBehaviour
{
	private bool _isOnView = false;
	public bool isOnView{
		set{
			_isOnView = value;
		}
	}

	private bool _initialized = false;
	public bool Initialized{
		set{
			_initialized = value;
		}
	}

	private Renderer   _renderer;
	private Renderer[] _childrenRenderer;

	private float SourceAlpha = 255f;
	private float DestAlpha   = 0f;
	
	public Shader TransparencyShader;
	private Shader standartShader;

	private bool Initialize(){
		_renderer         = GetComponent<Renderer>();
		_childrenRenderer = GetComponentsInChildren<Renderer>();
		standartShader = Shader.Find("Standard");


		return true;
	}

	private void SetRendererAlpha(Renderer alphaItem){
		if(!alphaItem || !alphaItem.enabled)
			return;

		if (_isOnView){
			//alphaItem.material.shader = TransparencyShader;
			DestAlpha = 0f;
		}
		else{
			DestAlpha = 255f;
		}

		//Debug.Log(alphaItem);

		if(!(alphaItem.material.shader == standartShader)){
			return;
		}

		Color CurrentColor = alphaItem.material.GetColor("_Color");


		SourceAlpha = CurrentColor.a;
		float difference = Mathf.Abs(DestAlpha - SourceAlpha);

		if(difference != 0){
			float coef = (difference < 1f) ? 1f : 4 * Time.deltaTime;

			alphaItem.material.SetColor("_Color", new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, Mathf.Lerp(SourceAlpha, DestAlpha, coef)));
			Debug.Log(Mathf.Lerp(SourceAlpha, DestAlpha, coef));
		}
	}

	void Update() {

		if(!_initialized){
			_initialized = Initialize();
		}

		SetRendererAlpha(_renderer);

		foreach(Renderer item in _childrenRenderer){
			SetRendererAlpha(item);
		}
		
	}
	
	void LateUpdate(){
		_isOnView = false;
	}
	
	
}