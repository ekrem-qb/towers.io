using UnityEngine;
using System.Collections.Generic;
public class Block : MonoBehaviour
{
    public float connectionSpeed;
    Vector3 targetPos;
    Vector3 targetScale;
    public int type;
    public int scale = 1;
    public bool isConnected;
    public MeshRenderer meshRenderer;
    Material defaultMaterial;
    Player player;
    void Awake()
    {
        defaultMaterial = meshRenderer.material;
    }
    void Start()
    {
        player = this.GetComponentInParent<Player>();

        if (player)
        {
            isConnected = true;
            meshRenderer.material = this.transform.GetComponentInParent<Player>().material;
        }

        targetScale = this.transform.localScale;
    }
    void Update()
    {
        if (isConnected)
        {
            if (this.transform.localPosition != targetPos)
            {
                this.transform.localPosition = Vector3.SlerpUnclamped(this.transform.localPosition, targetPos, Time.deltaTime * connectionSpeed);
            }
            if (this.transform.localRotation != Quaternion.identity)
            {
                this.transform.localRotation = Quaternion.SlerpUnclamped(this.transform.localRotation, Quaternion.identity, Time.deltaTime * connectionSpeed);
            }
            if (this.transform.localScale != targetScale)
            {
                this.transform.localScale = Vector3.SlerpUnclamped(this.transform.localScale, targetScale, Time.deltaTime * connectionSpeed);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (isConnected)
        {
            if (!other.transform.IsChildOf(this.transform.parent))
            {
                Block block = other.GetComponent<Block>();
                if (block)
                {
                    if (!block.isConnected)
                    {
                        this.GetComponentInParent<GridManager>().AddBlock(block);
                        if (block.transform.childCount > 0)
                        {
                            block.transform.GetChild(0).GetComponents<Behaviour>()[0].enabled = true;
                        }
                        block.isConnected = true;
                        block.player = this.player;
                        block.meshRenderer.material = this.meshRenderer.material;
                        player.AddSpeed(-0.25f);
                        player.AddMaxHealth(5);
                    }
                }
            }
        }
    }
    public void Merge()
    {
        targetPos = new Vector3(targetPos.x - (targetScale.x / 2f), targetPos.y, targetPos.z - (targetScale.z / 2f));
        targetScale = new Vector3(targetScale.x * 2, targetScale.y, targetScale.z * 2);

        scale *= 2;
    }
    public void SetTargetPosition(Vector3 newPos)
    {
        targetPos = newPos;
    }
    public Vector3 GetTargetPosition()
    {
        return targetPos;
    }
    public void Disconnect()
    {
        player = null;
        isConnected = false;
        meshRenderer.material = defaultMaterial;
        if (this.transform.childCount > 0)
        {
            this.transform.GetChild(0).GetComponents<Behaviour>()[0].enabled = false;
        }
        this.transform.SetParent(this.transform.parent.parent);
    }
}