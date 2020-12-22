using System.Collections.Generic;
using UnityEngine;
public class CameraTarget : MonoBehaviour
{
    public void CenterCamera()
    {
        List<float> allX = new List<float>();
        List<float> allZ = new List<float>();

        foreach (Block block in this.transform.parent.GetComponentsInChildren<Block>())
        {
            allX.Add(block.GetTargetPosition().x);
            allZ.Add(block.GetTargetPosition().z);
        }

        float sizeX = (Mathf.Max(allX.ToArray()) - Mathf.Min(allX.ToArray()));
        float sizeZ = (Mathf.Max(allZ.ToArray()) - Mathf.Min(allZ.ToArray()));

        Vector2 center = new Vector2(sizeX / 2, sizeZ / 2);

        float distance = (sizeX + sizeZ) * 2;

        this.transform.localPosition = new Vector3(center.x, 0, center.y);
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.distance = 80 + distance;
    }
}