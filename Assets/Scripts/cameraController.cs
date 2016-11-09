using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

	public float speed = 50f;

	//public float minFOV = 15f;
	//public float maxFOV = 150f;
	public float sensitivity = 50f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		/*
		float FOV = Camera.main.fieldOfView;
		FOV += Input.GetAxis ("Mouse ScrollWheel") * -sensitivity;
		FOV = Mathf.Clamp (FOV, minFOV, maxFOV);
		Camera.main.fieldOfView = FOV;
*/
		Vector3 newPos = this.transform.position;
		newPos.z += Input.GetAxis ("Mouse ScrollWheel") * sensitivity;
		this.transform.position = newPos;


	}

	private void FixedUpdate()
	{

		//if (!Input.GetMouseButton (1)) {
		// Basic Movement Player //
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		if (!Input.GetMouseButton (1)) {
			//Sets x and y basic movement
			transform.Translate (new Vector3 (speed * moveHorizontal, 0, 0));
		}

		//	if (!Input.GetMouseButton (2)) {
		transform.Translate (new Vector3 (0, speed * moveVertical, 0));
		//	}

	}
}
