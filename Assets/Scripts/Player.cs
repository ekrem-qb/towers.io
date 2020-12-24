using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public FixedJoystick joystick;
    public Text healthMeter;
    public Text speedMeter;
    public Material material;
    float health = 100;
    float maxHealth = 100;
    float speed = 1;
    float minSpeed = 0.25f;
    public float rotateSpeed;
    Rigidbody rb;
    AI ai;
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        ai = this.GetComponent<AI>();
    }
    void FixedUpdate()
    {
        if (joystick)
        {
            float joystickDelta = Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg;
            if (joystickDelta != 0)
            {
                // this.transform.position += (this.transform.forward * speed * ((Mathf.Abs(joystick.Horizontal) + Mathf.Abs(joystick.Vertical)) / 2)) / Mathf.Abs(Mathf.DeltaAngle(this.transform.eulerAngles.y, -45 + joystickDelta));
                this.transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(this.transform.eulerAngles.y, -45 + joystickDelta, Time.deltaTime * rotateSpeed), 0);
            }
            rb.AddForce(this.transform.forward * speed * ((Mathf.Abs(joystick.Horizontal) + Mathf.Abs(joystick.Vertical) / 2)) * Time.deltaTime * 100, ForceMode.Impulse);
        }
    }
    public void AddSpeed(float addValue)
    {
        if (speed > minSpeed || addValue > 0)
        {
            speed += addValue;
            if (speedMeter)
            {
                speedMeter.text = "⇉ " + speed;
            }
        }
    }
    public void AddHealth(float addValue)
    {
        if (health < maxHealth || addValue < 0)
        {
            health += addValue;
            if (healthMeter)
            {
                healthMeter.text = "❤ " + (int)health;
            }
        }
        if (health < 1)
        {
            if (joystick)
            {
                // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                foreach (Block block in this.GetComponentsInChildren<Block>())
                {
                    block.Disconnect();
                }
                Destroy(this.gameObject);
            }
        }
    }
    public void AddMaxHealth(float addValue)
    {
        maxHealth += addValue;
        if (ai)
        {
            ai.ExpandRadius(addValue);
        }
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    // void OnMouseDrag()
    // {
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     RaycastHit hit;
    //     if (Physics.Raycast(ray, out hit))
    //     {
    //         this.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
    //     }
    // }
}