using UnityEngine;
using System.Collections;

public class CameraHideObsticles : MonoBehaviour {

	private bool _isOnView = false;
	public bool isOnView{
		set{
			_isOnView = value;
		}
	}

	//private float SourceAlpha = 0.0f;
	//private float DestAlpha = 0.9f;

	public Shader TransparencyShader;

	void Update() {

		if(!GetComponent<Renderer>()){return;};

		if (_isOnView){

			//GetComponent<Renderer>().material.shader = TransparencyShader;
			Color SourceAlpha = GetComponent<Renderer>().material.GetColor("_Color");
			GetComponent<Renderer>().material.SetColor("_Color", new Color(SourceAlpha.r, SourceAlpha.g, SourceAlpha.b, Mathf.Lerp(SourceAlpha.a, 0f, 4 * Time.deltaTime)));

			//SourceAlpha = GetComponent<Renderer>().material.GetFloat("_AlphaTransparency");
			//DestAlpha = 0.9f;
			//GetComponent<Renderer>().material.SetFloat("_AlphaTransparency", Mathf.Lerp(SourceAlpha, DestAlpha, 4 * Time.deltaTime));

		}
		else{

			Color SourceAlpha = GetComponent<Renderer>().material.GetColor("_Color");
			//Color DestAlpha = new Color(SourceAlpha.r, SourceAlpha.g, SourceAlpha.b, 255f);
			GetComponent<Renderer>().material.SetColor("_Color", new Color(SourceAlpha.r, SourceAlpha.g, SourceAlpha.b, Mathf.Lerp(SourceAlpha.a, 255, 4 * Time.deltaTime)));
			//float SourceAlpha = GetComponent<Renderer>().material.GetFloat("_AlphaTransparency");
			//float DestAlpha = 0.0f;
			//GetComponent<Renderer>().material.SetFloat("_AlphaTransparency", Mathf.Lerp(SourceAlpha, DestAlpha, 4 * Time.deltaTime));
		}
	
	}

	void LateUpdate(){
		_isOnView = false;
	}

	
}
