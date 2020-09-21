using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DavidTest
{
	public enum AnimDataType
	{
		Trigger,
		Boolean
	}

	[System.Serializable]
	public struct AnimationTestData
	{
		public string parameterName;
		public AnimDataType parameterType;
		public KeyCode inputKey;
	}

	public class CharacterAnimationTester : MonoBehaviour
	{
		[SerializeField] private AnimationTestData[] testData;
		private Animator m_Anim = null;

		private void Awake()
		{
			m_Anim = GetComponent<Animator>();
		}


		private void Update()
		{
			foreach (var data in testData)
			{
				if (data.parameterType == AnimDataType.Trigger && Input.GetKeyDown(data.inputKey))
				{
					m_Anim.SetTrigger(data.parameterName);
				}

				if (data.parameterType == AnimDataType.Boolean)
				{
					m_Anim.SetBool(data.parameterName, Input.GetKey(data.inputKey));
				}
			}

		}
	}
}

