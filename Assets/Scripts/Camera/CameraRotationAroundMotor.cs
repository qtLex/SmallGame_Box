using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Камеру не нужно подчинять игроку!
// Просто навешиваем скрипт и указываем объект слежения.

public class CameraRotationAroundMotor : MonoBehaviour {

	public GameObject TargetObject;
	
	[Range(150.0f,1000.0f)][Tooltip("Скорость вертикального поворота при управлении мышью")]
	public float VerticalSpeed = 100.0f;

	[Range(300.0f,1000.0f)][Tooltip("Скорость горизонтального поворота при управлении мышью")]
	public float HorisontalSpeed = 50.0f;

	[Range(100.0f,1000.0f)][Tooltip("Скорость зума при управлении мышью")]
	public float ZoomSpeed = 100.0f;

	[Range(10.0f,100.0f)][Tooltip("Скорость восстановления позиции камеры при перемещении объекта(насколько быстро она устаканится)")]
	public float ReturnSpeed = 50.0f;

	[Range(1.0f,10.0f)][Tooltip("Радиус, в котором объекты закрывающие игрока будут прозрачными")]
	public float Radius = 8.0f;

	[Range(1.0f,10.0f)][Tooltip("Толщина объекта, настраивается для того чтобы объкт сам не стал прозрачным")]
	public float TargetThickness = 1.0f;

	[Range(1.0f,100.0f)][Tooltip("Постоянное расстояние до игрока, которое сохраняет камера при автоматическом повороте")]
	public float CameraDistance = 40.0f;

	[Range(1.0f,89.0f)][Tooltip("Угол к плоскости игрока, который сохраняет камера")]
	public float CameraAngle = 45.0f;

	public Shader TransparentShader;

	void Start () {
		transform.LookAt(TargetObject.transform);
		if (!TransparentShader){
			TransparentShader = Shader.Find("Transparent/Parralax Specular");
		}
	}

	void Update () {

		// Расчет точки зависания камеры
		Plane ForwLeft = new Plane(TargetObject.transform.up, TargetObject.transform.position);
		Vector3 CameraPositionOnObjectPlane = Vector3.ProjectOnPlane(transform.position - TargetObject.transform.position, ForwLeft.normal) + TargetObject.transform.position;

		float b = Mathf.Cos(CameraAngle * Mathf.Deg2Rad) * CameraDistance;
		float a = Mathf.Sin(CameraAngle * Mathf.Deg2Rad) * CameraDistance;

		Vector3 delta = ((CameraPositionOnObjectPlane - TargetObject.transform.position).normalized * a) + (ForwLeft.normal * b);
		Vector3 TargetPosition = TargetObject.transform.position + delta;

		// Поворот согласно повороту объекта.
		Quaternion TargetRotation = Quaternion.LookRotation(TargetObject.transform.position - transform.position,TargetObject.transform.up);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, 100 * Time.deltaTime);
		Vector3 HorizontalAxis = Vector3.Cross(transform.forward, TargetObject.transform.up);

		if(Input.GetKey(KeyCode.Mouse1)){
			transform.RotateAround(TargetObject.transform.position, TargetObject.transform.up, HorisontalSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);
			transform.RotateAround(TargetObject.transform.position, HorizontalAxis, VerticalSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime);
			transform.Translate(new Vector3(0,0,1) * ZoomSpeed * Input.GetAxis("Mouse ScrollWheel"));
		}else{
			transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Time.deltaTime * ReturnSpeed);
		}

		Ray toTarget = new Ray(transform.position, TargetObject.transform.position - transform.position);

		RaycastHit[] hits = Physics.SphereCastAll(toTarget, Radius, Vector3.Distance(transform.position, TargetObject.transform.position) - Radius - TargetThickness, LayerMask.GetMask("Box"));

		foreach (RaycastHit _iterator in hits){
			if (!_iterator.transform.gameObject.Equals(TargetObject)){

				GameObject _obj = _iterator.transform.gameObject;
				CameraHideObsticles ObsticleHandler =_obj.GetComponent<CameraHideObsticles>();
				if(ObsticleHandler == null){
					ObsticleHandler = _obj.AddComponent<CameraHideObsticles>();
				}

				//ObsticleHandler.TransparencyShader = TransparentShader;
				ObsticleHandler.isOnView = true;
			}
		}


	}
}
