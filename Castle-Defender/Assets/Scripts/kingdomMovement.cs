using UnityEngine;

public class kingdomMovement : MonoBehaviour
{
    
    [SerializeField] float maxmoveSpeed;
    private Transform tf;
    private Vector2 movement;
    void Start()
    {
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        //Inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //movement
        float currentSpeedX = maxmoveSpeed * movement.x * Time.deltaTime;
        float currentSpeedY = maxmoveSpeed * movement.y * Time.deltaTime;

        // updated tf.position = tf.position + new Vector3(currentSpeedX, currentSpeedY);
        // this does the same thing but shorter
        tf.Translate(new Vector3(currentSpeedX,currentSpeedY));
    }
}
