using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomBlockGenerator : MonoBehaviour
{
    public GameObject blockPrefab;
    public List<GameObject> blockList;
    public float generateCount;
    public float generateInterval;
    public float randomMinRadius;
    public float randomMaxRadius;
    float next;
    void Update()
    {
        if (blockList.Count < generateCount && Time.time > next)
        {
            next = Time.time + generateInterval;

            GameObject block = Instantiate(blockPrefab);
            Vector2 pos2D = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 randomPos2D = Random.insideUnitCircle.normalized * Random.Range(randomMinRadius, randomMaxRadius) + pos2D;
            block.transform.position = new Vector3(randomPos2D.x, this.transform.position.y, randomPos2D.y);
            block.GetComponent<Block>().RandomType();
            block.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
            blockList.Add(block);
        }
    }
}