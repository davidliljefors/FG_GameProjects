using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ToonShaderLightSettings : MonoBehaviour
{
	private const string k_ToonLightDir = "_ToonLightDirection";
	private const string k_ToonLightColor = "_ToonLightColor";
	private const string k_ToonLightIntensity = "_ToonLightIntensity";

	private int toonLightDirID;
	private int toonLightColorID;
	private int toonLightIntensityID;

	private Light mainLight;

	void OnEnable()
	{
		toonLightDirID = Shader.PropertyToID(k_ToonLightDir);
		toonLightColorID = Shader.PropertyToID(k_ToonLightColor);
		toonLightIntensityID = Shader.PropertyToID(k_ToonLightIntensity);
		mainLight = GetComponent<Light>();
	}

	void Update()
	{
		Shader.SetGlobalVector(toonLightDirID, -transform.forward);
		Shader.SetGlobalColor(toonLightColorID, mainLight.color);
		Shader.SetGlobalFloat(toonLightIntensityID, mainLight.intensity);
	}
}