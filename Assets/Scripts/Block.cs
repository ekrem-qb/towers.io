using UnityEngine;
using System.Collections.Generic;
public class Block : MonoBehaviour
{
    public float connectionSpeed;
    public Vector3 targetPos;
    public Vector3 targetScale;
    public int type;
    void Awake()
    {
        RefreshType();
        targetScale = this.transform.localScale;
    }
    void Update()
    {
        if (this.transform.localPosition != targetPos)
        {
            this.transform.localPosition = Vector3.SlerpUnclamped(this.transform.localPosition, targetPos, Time.deltaTime * connectionSpeed);
        }
        if (this.transform.rotation != Quaternion.identity)
        {
            this.transform.rotation = Quaternion.SlerpUnclamped(this.transform.rotation, Quaternion.identity, Time.deltaTime * connectionSpeed);
        }
        if (this.transform.localScale != targetScale)
        {
            this.transform.localScale = Vector3.SlerpUnclamped(this.transform.localScale, targetScale, Time.deltaTime * connectionSpeed);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (this.enabled)
        {
            if (!other.transform.IsChildOf(this.transform.parent))
            {
                Block block = other.GetComponent<Block>();
                this.GetComponentInParent<GridManager>().AddBlock(block);
                block.enabled = true;
            }
        }
    }
    public void RandomType()
    {
        type = Random.Range(1, 3);
        RefreshType();
    }
    public void RefreshType()
    {
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        Material newMat = meshRenderer.material;
        if (type > 10)
        {
            Vector3 oldScale = this.transform.localScale;
            targetScale = new Vector3(oldScale.x * 2, oldScale.y, oldScale.z * 2);
            targetPos = new Vector3(targetPos.x - (oldScale.x / 2f), targetPos.y, targetPos.z - (oldScale.z / 2f));
        }
        switch (type)
        {
            case 1:
                newMat.color = Color.red;
                break;
            case 2:
                newMat.color = Color.blue;
                break;
            case 11:
                newMat.color = Color.magenta;
                break;
            case 22:
                newMat.color = Color.cyan;
                break;
        }
        meshRenderer.material = newMat;
    }
}