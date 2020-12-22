using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomBlockGenerator : MonoBehaviour
{
    public GameObject blockPrefab;
    public List<GameObject> blockTypes;
    List<Block> blockList = new List<Block>();
    public float generateCount;
    public float randomMinRadius;
    public float randomMaxRadius;
    float next;
    void Start()
    {
        for (int i = 0; i < generateCount; i++)
        {
            Vector2 pos2D = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 randomPos2D = Random.insideUnitCircle.normalized * Random.Range(randomMinRadius, randomMaxRadius) + pos2D;

            Block block = Instantiate(blockPrefab).GetComponent<Block>();
            block.transform.position = new Vector3(randomPos2D.x, this.transform.position.y, randomPos2D.y);
            block.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
            block.type = Random.Range(0, blockTypes.Count);
            if (blockTypes[block.type])
            {
                GameObject blockMod = Instantiate(blockTypes[block.type]);
                blockMod.transform.SetParent(block.transform);
                blockMod.transform.localPosition = new Vector3(0, 1, 0);
                blockMod.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            // foreach (MeshRenderer renderer in blockMod.GetComponentsInChildren<MeshRenderer>())
            // {
            //     if (renderer.GetComponent<MeshFilter>())
            //     {
            //         renderer.material = block.meshRenderer.material;
            //     }
            // }

            blockList.Add(block);
        }
    }
}