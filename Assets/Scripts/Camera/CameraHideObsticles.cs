using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraHideObsticles : MonoBehaviour
{
	private bool _isOnView = false;
	public bool isOnView{
		set{
			_isOnView = value;
		}
	}

	public bool _initialized = false;

	private Renderer   _renderer;
	private Renderer[] _childrenRenderer;

	private float SourceAlpha = 1f;
	private float DestAlpha   = 0.3f;
	
	public static Shader TransparencyShader = Shader.Find("Transparent/VertexLit with Z");
	private static Shader _standardShader = Shader.Find("Standard");

	private static Dictionary<int, Shader> ShaderBuffer = new Dictionary<int, Shader>();
	private bool _shaderCached = false;

	private bool Initialize(){
	
		if (_initialized){return true;};
		
		if(!_renderer){
			_renderer      = GetComponent<Renderer>();
		};

		_childrenRenderer        = GetComponentsInChildren<Renderer>();

		

		if(!_standardShader){
			_standardShader = Shader.Find("Standard");
		}

		if(!_shaderCached && _renderer){
			ShaderBuffer.Add(_renderer.GetInstanceID(), _renderer.material.shader);
			_shaderCached = true;

		}

		if(_childrenRenderer.Length > 0){
			foreach(Renderer iteratorRenderer in _childrenRenderer){
				int ID = iteratorRenderer.GetInstanceID();
				if(!ShaderBuffer.ContainsKey(ID)){								
					ShaderBuffer.Add(ID, iteratorRenderer.material.shader);
				}
			}
		}

		return true;
	}

	private void SetRendererAlpha(Renderer alphaItem){
		if(!alphaItem || !alphaItem.enabled)
			return;

		if (_isOnView){
			alphaItem.material.shader = TransparencyShader;
			DestAlpha = 0.3f;
		}
		else{
			DestAlpha = 1.0f;
		}

		if(!(alphaItem.material.shader == TransparencyShader)){
			return;
		}

		Color CurrentColor = alphaItem.material.GetColor("_Color");
		SourceAlpha = CurrentColor.a;
		float difference = Mathf.Abs(DestAlpha - SourceAlpha);

		if(difference != 0){
			float coef = (difference < 0.01f) ? 1f : Time.deltaTime;

			alphaItem.material.SetColor("_Color", new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, Mathf.Lerp(SourceAlpha, DestAlpha, coef)));
		}else{

			if (!_isOnView){
				Shader OldShader = _standardShader;
				if(ShaderBuffer.TryGetValue(alphaItem.GetInstanceID(), out OldShader)){
					alphaItem.material.shader = OldShader;
				}
			}
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