using UnityEngine;

public class OldBoard : MonoBehaviour {
    public Rigidbody rigidBody;
    public ConstantForce constForce;
    public float maxAcceleration;
    public float idealHeight;
    public float maxHeight;

    // Use this for initialization
    void Start () {
        rigidBody = transform.GetComponent<Rigidbody>();
        constForce = transform.GetComponent<ConstantForce>();
	}
	
	void FixedUpdate () {
        //rigidBody.AddForce(new Vector3(0, VerticalForce(), 0));
        SlerpRotation();
    }

    float IdealY(Vector3 fromPos)
    {
        Vector3 fromPosition = new Vector3(fromPos.x, fromPos.y - transform.localScale.y / 2f, fromPos.z);

        RaycastHit hit;
        float idealY = 0.0f;

        if (Physics.Raycast(fromPos, -Vector3.up, out hit))
        {
            idealY = hit.point.y + this.idealHeight;
            Debug.DrawLine(hit.point, hit.point + new Vector3(0, idealHeight, 0));
        }
        return idealY;
    }

    float VerticalForce()
    {
        float height = Height();
        float acceleration = Mathf.Max(0, VerticalAcceleration(height, Physics.gravity.y, maxAcceleration, IdealY(transform.position)));

        Debug.Log(acceleration);

        return acceleration * rigidBody.mass;
    }

    float Height()
    {
        RaycastHit hit;
        float height = maxHeight;
        Vector3 fromPosition = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2f, transform.position.z);
        if (Physics.Raycast(fromPosition, -Vector3.up, out hit, maxHeight))
        {
            height = hit.distance;
        }
        return height;
    }

    float VerticalAcceleration(float height, float gravity, float maxAcceleration, float idealHeight)
    {        
        float acceleration = (height * ((-gravity - maxAcceleration) / idealHeight) + maxAcceleration);
        return acceleration;
    }

    void SlerpRotation()
    {
        float speed = Time.deltaTime * 20.0f;
        Debug.Log(speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, 0, 0, 0), speed);
    }
}
