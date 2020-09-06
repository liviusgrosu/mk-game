using UnityEngine;
using System.Collections;

public class tree_object {

    public bool drawn;
    public GameObject treeObj;
    public Vector3 randomPos;
    public float monsterPosY;

    /*public tree_object()
    {
        drawn = false;
        treeObj = null;
    }*/

    public tree_object(bool drawn, GameObject treeObj)
    {
        this.drawn = drawn;
        this.treeObj = treeObj;
    }

    public void setDrawn(bool state)
    {
        drawn = state;
    }

    public bool isDrawn()
    {
        return drawn;
    }
	
    public void setTreeObject(GameObject treeObj)
    {
        this.treeObj = treeObj;
    }

    public GameObject treeObject()
    {
        return treeObj;
    }

    public void createRandPos()
    {
        randomPos = new Vector3(treeObj.transform.position.x, Random.Range(treeObj.transform.position.y, treeObj.transform.position.y + treeObj.GetComponent<MeshCollider>().bounds.size.y), treeObj.transform.position.z);
    }

    public Vector3 getRandPos()
    {
        return randomPos;
    }


    public void createMonsterPosY(GameObject monsterObj)
    {
        monsterPosY = Random.Range(monsterObj.transform.position.y + 0.5f, monsterObj.transform.position.y + /*monsterObj.GetComponent<CapsuleCollider>().bounds.size.y*/ 1f);
    }

    public float getMonsterPosY()
    {
        return monsterPosY;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
