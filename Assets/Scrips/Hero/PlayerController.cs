using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float speed = 5f;
    [SerializeField]
    private float lookSpeed = 3f;

    private PlayerMotor motor;
    public float MinimumX = -90F;
    public float MaximumX = 90F;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMove;
        Vector3 moveVertical = transform.forward * zMove;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(velocity);

        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRotation, 0f) * lookSpeed;

        motor.Rotate(rotation);

        float xRotation = Input.GetAxisRaw("Mouse Y");
        Vector3 camRotation = new Vector3(xRotation, 0f, 0f) * lookSpeed;

        motor.RotateCam(camRotation);
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
