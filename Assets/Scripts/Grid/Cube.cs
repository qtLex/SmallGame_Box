using UnityEngine;
using System.Collections;
using System;

// Класс предназначен для сериализации при компиляции проекта.
[Serializable]
public class Cube{

	public string Key;
	public GameObject Object;

	// Use this for initialization
	public Cube () {
		Key = System.Guid.NewGuid().ToString();
	}

}
