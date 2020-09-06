using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class arm_tree_grab : MonoBehaviour
{
    public GameObject monsterObj;
    public GameObject handsPrefab;
    public List<GameObject> arrHands;
    //public SphereCollider treeCollider;

    

    List<tree_object> trees;


    // Use this for initialization
    void Start()
    {
        trees = new List<tree_object>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = monsterObj.transform.position;
    }


    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "tree")
        {
            trees.Add(new tree_object(false, collision.gameObject));
            //print("size: " + trees.count);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "tree")
        {
            foreach (tree_object tree in trees)
            {
                if(collision.gameObject == tree.treeObject())
                {
                    trees.Remove(tree);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (trees != null  && monsterObj.activeSelf)
        {
            foreach (tree_object tree in trees)
            {
                if (!tree.isDrawn())
                {
                    print("creating random pos");
                    tree.createRandPos();
                    tree.createMonsterPosY(monsterObj);
                    tree.setDrawn(true);
                }
                float distance = Vector3.Distance(new Vector3(transform.position.x, tree.getMonsterPosY(), transform.position.z), tree.getRandPos());
                int handsNum = (int)(distance / 2f);

                for (int i = 0; i < arrHands.Count; i++)
                {
                    Destroy(arrHands[i]);
                }

                    arrHands.Clear();

               

                for(int i = 0; i < handsNum; i++)
                {
                    arrHands.Add((GameObject)Instantiate(handsPrefab, new Vector3(transform.position.x * i, tree.getMonsterPosY() * i, transform.position.z * i), Quaternion.identity/*Quaternion.RotateTowards(transform.rotation, tree.treeObject().transform.rotation, 1f)*/));
                }

                //print("amount of hands: " + i); 

                //print("drawing");
                Gizmos.color = Color.red;
                //Gizmos.DrawLine(transform.position, tree.transform.position);


                Gizmos.DrawLine(new Vector3(transform.position.x, tree.getMonsterPosY(), transform.position.z),
                                tree.getRandPos());
            }
        }
    }
}