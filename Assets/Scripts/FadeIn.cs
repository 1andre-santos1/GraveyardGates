using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
	void Start ()
	{
		Destroy(gameObject,GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
	}
}
