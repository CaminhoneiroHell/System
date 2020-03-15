using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {

	}

	void ExhaustShake()
	{
		//StopAllCoroutines();
		StartCoroutine("Shake");
	}


	//[Header("Steering Wheel Settings")]
	public GameObject steeringWheel, wheelFront, wheelBack;

	[Header("Exhaust Settings")]
	[SerializeField]float duration = 0.5f;
	[SerializeField]float magnitude = 0.1f;
	[SerializeField] GameObject exhaust;

	IEnumerator Shake()
	{

		float elapsed = 0.0f;

		Vector3 originalExhaustPos = exhaust.transform.position;

		while (elapsed < duration)
		{

			elapsed += Time.deltaTime;

			float percentComplete = elapsed / duration;
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;

			exhaust.transform.position = new Vector3(x, y, originalExhaustPos.z);

			yield return null;
		}

		exhaust.transform.position = originalExhaustPos;
	}


}
