using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrithGenerator : MonoBehaviour
{
    public int size = 16;
    public int[,]maze;
    public Object wall;
    public Object ground;
    // Start is called before the first frame update
    void Start()
    {
        maze = new int[size,size];
        string x = "";
        for(int i = 0; i < size;i++)
        {
            for(int j = 0; j < size; j++)
            {
                if (i == 0 || j == 0 || i == size - 1 || j == size - 1)
                {
                    maze[i, j] = 1;

                }
                else
                {
                    maze[i, j] = 0;
                }
                x += maze[i, j];
            }
            x += "\n";
        }
        Debug.Log(x);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                if (maze[i, j] == 0)
                {
                    Instantiate(ground, new Vector3(i * 20, 0, j * 20),Quaternion.identity);
                }
                else
                {
                    float rot = 0;
                    if (i < j)
                    {
                        rot = 90;
                    }
                    Instantiate(wall, new Vector3(i * 10, 10, j * 10),new Quaternion(Quaternion.identity.x,rot,Quaternion.identity.z,Quaternion.identity.w));
                }
            }
        }
    }
}
