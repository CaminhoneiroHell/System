using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartAnimationController : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] float wheelSpeed = 10f;
    void Update()
    {
		ExhaustShake();
		wheelFront.transform.Rotate(0, 0, -wheelSpeed * Time.deltaTime, Space.Self);
		wheelBack.transform.Rotate(0, 0, -wheelSpeed * Time.deltaTime, Space.Self);
	}

	void ExhaustShake()
	{
		//StopAllCoroutines();
		StartCoroutine(Shake());
	}


	//[Header("Steering Wheel Settings")]
	public GameObject steeringWheel, wheelFront, wheelBack;

	[Header("Exhaust Settings")]
	[SerializeField]float duration = 0.5f;
	[SerializeField]float magnitude = 0.1f;
	[SerializeField] GameObject[] exhaustList;

	float elapsed = 0.0f;
	GameObject exhaust;
	Vector3 originalExhaustPos;
	IEnumerator Shake()
	{

		for (int i = 0; i < exhaustList.Length; i++)
		{
			exhaust = exhaustList[i].gameObject;
			originalExhaustPos = exhaust.transform.position;


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

			}

			exhaust.transform.position = originalExhaustPos;
			yield return null;
		}

	}


}
