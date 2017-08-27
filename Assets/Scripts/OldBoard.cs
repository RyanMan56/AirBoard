using UnityEngine;

public class OldBoard : MonoBehaviour {
    public Rigidbody rigidBody;
    public ConstantForce constForce;
    public float maxAcceleration;
    public float idealHeight;

    // Use this for initialization
    void Start () {
        rigidBody = transform.GetComponent<Rigidbody>();
        constForce = transform.GetComponent<ConstantForce>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        constForce.force = (new Vector3(0, VerticalForce(), 0));
        //DampVel();
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

        return acceleration * rigidBody.mass;
    }

    float Height()
    {
        RaycastHit hit;
        float height = 0.0f;
        Vector3 fromPosition = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2f, transform.position.z);
        //Vector3 fromPosition = transform.position;
        if (Physics.Raycast(fromPosition, -Vector3.up, out hit))
        {
            height = hit.distance;
        }
        return height;
    }

    float VerticalAcceleration(float height, float gravity, float maxAcceleration, float idealHeight)
    {        
        float acceleration = (height * ((-gravity - maxAcceleration) / idealHeight) + maxAcceleration);
        Debug.Log("Height: " + height + ", acceleration: " + acceleration);
        return acceleration;
    }

    void DampVel()
    {
        Vector3 target = new Vector3(transform.position.x, IdealY(transform.position), transform.position.z);
        Vector3 currentVel = rigidBody.velocity;
        Vector3.SmoothDamp(transform.position, target, ref currentVel, 5.0f, 20);
        rigidBody.velocity = currentVel;
    }

    
}
