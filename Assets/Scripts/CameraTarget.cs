using System.Collections.Generic;
using UnityEngine;
public class CameraTarget : MonoBehaviour
{
    public void CenterCamera()
    {
        List<float> allX = new List<float>();
        List<float> allZ = new List<float>();

        for (int i = 0; i < this.transform.parent.childCount; i++)
        {
            allX.Add(this.transform.parent.GetChild(i).transform.localPosition.x);
            allZ.Add(this.transform.parent.GetChild(i).transform.localPosition.z);
        }

        float sizeX = (Mathf.Max(allX.ToArray()) - Mathf.Min(allX.ToArray()));
        float sizeZ = (Mathf.Max(allZ.ToArray()) - Mathf.Min(allZ.ToArray()));

        Vector2 center = new Vector2(sizeX / 2, sizeZ / 2);

        float distance = (sizeX * sizeZ) / 2;

        this.transform.localPosition = new Vector3(center.x, 0, center.y);
        Camera.main.GetComponent<CameraController>().distance = 60 + (distance / 4);
    }
}