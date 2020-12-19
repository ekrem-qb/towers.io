using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Control : MonoBehaviour
{
    public FixedJoystick joystick;
    void Start()
    {

    }
    void FixedUpdate()
    {
        this.transform.Translate(new Vector3(1, 0, 1) * joystick.Horizontal);
        this.transform.Translate(new Vector3(-1, 0, 1) * joystick.Vertical);
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