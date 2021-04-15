using UnityEngine;
public class Block : MonoBehaviour
{
    public float connectionSpeed;
    Vector3 targetPos;
    public Vector3 targetScale;
    public int scale = 1;
    public MeshRenderer meshRenderer;
    Material defaultMaterial;
    public Player player;
    public int type, x, y, badNeighbours, goodNeighbours;
    public float distanceToBestBlock;
    public Block topLeft, top, topRight;
    public Block left, right;
    public Block bottomLeft, bottom, bottomRight;
    void Awake()
    {
        defaultMaterial = meshRenderer.material;
    }
    void Start()
    {
        player = this.GetComponentInParent<Player>();

        if (player)
        {
            meshRenderer.material = player.material;
        }

        targetScale = this.transform.localScale;
    }
    void Update()
    {
        if (player)
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
        if (player)
        {
            Block enemyBlock = other.GetComponent<Block>();
            if (enemyBlock)
            {
                if (enemyBlock.player != this.player)
                {
                    if (enemyBlock.player)
                    {
                        Player enemy = enemyBlock.GetComponentInParent<Player>();
                        if (enemy.GetMaxHealth() < player.GetMaxHealth())
                        {
                            enemy.AddHealth(-enemy.GetMaxHealth());
                        }
                    }
                    else
                    {
                        this.GetComponentInParent<GridManager>().AddBlock(enemyBlock);
                        if (enemyBlock.transform.childCount > 0)
                        {
                            enemyBlock.transform.GetChild(0).GetComponents<Behaviour>()[0].enabled = true;
                        }
                        enemyBlock.player = this.player;
                        enemyBlock.meshRenderer.material = enemyBlock.player.material;
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
    public void SetTargetPosition(Vector2Int newPos)
    {
        targetPos = new Vector3(newPos.y * this.transform.localScale.x, 0, newPos.x * this.transform.localScale.z);
        x = newPos.x;
        y = newPos.y;
        name = "(" + x + "," + y + ")";
    }
    public Vector2Int GetTargetPosition()
    {
        return new Vector2Int(x, y);
    }
    public void Disconnect()
    {
        player = null;
        meshRenderer.material = defaultMaterial;
        if (this.transform.childCount > 0)
        {
            this.transform.GetChild(0).GetComponents<Behaviour>()[0].enabled = false;
        }
        this.transform.SetParent(this.transform.root.parent);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (top)
        {
            Gizmos.DrawCube(top.transform.position, top.transform.localScale);
        }
        if (topLeft)
        {
            Gizmos.DrawCube(topLeft.transform.position, topLeft.transform.localScale);
        }
        if (left)
        {
            Gizmos.DrawCube(left.transform.position, left.transform.localScale);
        }
        if (bottomLeft)
        {
            Gizmos.DrawCube(bottomLeft.transform.position, bottomLeft.transform.localScale);
        }
        if (topRight)
        {
            Gizmos.DrawCube(topRight.transform.position, topRight.transform.localScale);
        }
        if (right)
        {
            Gizmos.DrawCube(right.transform.position, right.transform.localScale);
        }
        if (bottomRight)
        {
            Gizmos.DrawCube(bottomRight.transform.position, bottomRight.transform.localScale);
        }
        if (bottom)
        {
            Gizmos.DrawCube(bottom.transform.position, bottom.transform.localScale);
        }
    }
}