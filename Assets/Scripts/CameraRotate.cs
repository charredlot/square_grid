using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour {
	public static readonly float CAMERA_ROTATE_SPEED = 8.0f;
	public static readonly float CAMERA_TILT_SPEED = 25.0f;
	public static readonly float CAMERA_ZOOM_SPEED = 16.0f;
	public static readonly float CAMERA_TRANSLATE_SPEED = 10.0f;
	public Vector3 center = new Vector3(0,0,0);
	public Vector3 look_target;
	public bool use_mouse = true;

	private Vector3 curr_look_target;

	float radius;
	Camera cam;

	// Use this for initialization
	void Start () {
		float delta_x;
		float delta_z;

		/* y is vertical axis, we're on the x-z, assume x starts at 0 */
		this.center = new Vector3(this.center.x, this.transform.position.y, this.center.z);
		delta_x = this.center.x - this.transform.position.x;
		delta_z = this.center.z - this.transform.position.z;
		this.radius = Mathf.Sqrt((delta_x*delta_x) + (delta_z*delta_z));

		this.curr_look_target = this.look_target;	
		this.transform.LookAt(this.look_target);

		this.cam = (Camera)this.GetComponent("Camera");
	}

	void UpdateZoom() {
		float scroll;

		/* negative is closer for us */
		scroll = -Input.GetAxis ("Mouse ScrollWheel") * CameraRotate.CAMERA_ZOOM_SPEED;
		if (scroll == 0) {
			return;
		}

		this.cam.fieldOfView += scroll;
	}

	void UpdateTilt() {
		float motion;

		if (Input.GetMouseButton((int)Hacks.MouseButton.RIGHT)) {
			motion = Input.GetAxis("Mouse Y");
		} else {
			motion = Input.GetAxis("Vertical");
		}
		motion = motion * Time.deltaTime * CameraRotate.CAMERA_TILT_SPEED;

		this.curr_look_target = new Vector3(
			this.curr_look_target.x,
			this.curr_look_target.y + motion,
			this.curr_look_target.z);
	}
	
	void UpdateAngle() {
		float delta_theta;
		float motion;
		float theta;

		if (Input.GetMouseButton((int)Hacks.MouseButton.RIGHT)) {
			motion = -Input.GetAxis("Mouse X") * 2;

		} else {
			motion = Input.GetAxis("Horizontal");
		}
		motion = motion * Time.deltaTime * CameraRotate.CAMERA_ROTATE_SPEED;

		if (motion == 0) {
			return;
		}

		/* we're moving in x-z plane, use arctan2 for handling quadrants */
		delta_theta = motion / this.radius;
		theta = Mathf.Atan2(
			this.transform.position.z - this.center.z,
			this.transform.position.x - this.center.x);

		theta += delta_theta;
		this.transform.position = new Vector3(
			this.center.x + this.radius * Mathf.Cos(theta),
			this.transform.position.y,
			this.center.z + this.radius * Mathf.Sin(theta));
	}

	Vector3 vclamp(Vector3 v)
	{
		float MY_EPSILON = 0.05f;

		return new Vector3(
			Mathf.Abs (v.x) < MY_EPSILON ? 0 : v.x,
			Mathf.Abs(v.y) < MY_EPSILON ? 0 : v.y,
			Mathf.Abs(v.z) < MY_EPSILON ? 0 : v.z
		);
	}

	
	void UpdateTranslation() {
		Vector3 motion;
		Vector3 basis_x;
		Vector3 basis_y;
		Vector3 basis_z;

		if (Input.GetMouseButton((int)Hacks.MouseButton.MIDDLE)) {
			motion = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
		} else {
			motion = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		}

		if ((motion.x == 0) && (motion.y == 0)) {
			return;
		}

		/* up down is still along y-axis */
		basis_y = new Vector3(0, 1, 0);
		/* in out, movement along this basis should be 0 if our math is correct */
		basis_z = (this.transform.position - this.center).normalized;
		/* left-right needs to be converted to the axis perpendicular to the radius */
		basis_x = Vector3.Cross(basis_y, basis_z).normalized;

		/* invert y, also our basis x is pointing the wrong way probably? */
		motion = motion.x * basis_x - motion.y * basis_y;
		//motion *= CameraRotate.CAMERA_TRANSLATE_SPEED * Time.deltaTime;

		this.center += motion;
		this.transform.position += motion;
		this.curr_look_target += motion;
	}


	void Update () {


		this.UpdateTranslation();
		this.UpdateAngle();
		this.UpdateZoom();
		this.UpdateTilt();
		this.transform.LookAt(this.curr_look_target);



		GameObject g = GameObject.Find ("camera_center");
		g.transform.position = this.center;

		g = GameObject.Find ("look_target");
		g.transform.position = this.curr_look_target;
	}

}
