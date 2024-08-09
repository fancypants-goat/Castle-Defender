using UnityEngine;

public class KingdomMovement : MonoBehaviour
{
    
    [SerializeField] float maxmoveSpeed;
    private Vector2 movement;
    public ModeManager buildMode;
    // Update is called once per frame
    void Update()
    {
        if (buildMode.walkMode)
        {
            Move();
        }
    }

    void Move()
    {
        //Inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Diagonal movement
        if (movement.sqrMagnitude > 1)
        {
            movement.Normalize();
        }

        //movement
        float currentSpeedX = maxmoveSpeed * movement.x * Time.deltaTime;
        float currentSpeedY = maxmoveSpeed * movement.y * Time.deltaTime;

        // updated tf.position = tf.position + new Vector3(currentSpeedX, currentSpeedY);
        // this does the same thing but shorter
        transform.Translate(new Vector3(currentSpeedX,currentSpeedY));
    }
}
