using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrithGenerator : MonoBehaviour
{
    public int size = 16;
    mazePart[,]maze;
    public Object wall;
    public Object ground;
    struct mazePart
    {
        public bool isActive;
        public int xLastPart;
        public int yLastPart;
        public int[] xNextPart;
        public int[] yNextPart;
        public int nextSize;
        public int next;

    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                maze[i, j].isActive = false;//помечаем что все участки лабиринта не помечены
                maze[i, j].nextSize = 0;
                maze[i, j].next = 0;
                maze[i, j].xNextPart = new int[4];
                maze[i, j].yNextPart = new int[4];
            }
        }
       int x=Random.Range(0, size-1);
       int y = Random.Range(0, size-1);
        maze[x, y].isActive = true;
        maze[x, y].xLastPart = x;
        maze[x, y].yLastPart = y;
        int cheksParts = 0;
        int res = Random.Range(0, 1.0f) < 0.5f ? -1 : 1;//определяем направление движения
        int res2 = Random.Range(0, 1.0f) < 0.5f ? -1 : 1;//определяем куда пойдем
        bool change1 = false;
        bool change2 = false;
        while (cheksParts != size)//прошли ли все части лабиринта
        {
           if (res == -1)//проверяем направление второго уровня
           {//двигаемся влево или вправо
              if(maze[x,y+res2].isActive || y+res2<0 || y + res2 == size)//проверяем была ли проверена эта точка или вышли за пределы лабиринта
              {
                    if (!change1)//проверяем меняли ли направление движения в следующую клетку первого уровня
                    {
                        res2 = -res2; //если да,то двигаемся в противоположном направлении
                        change1 = true;//помечаем что меняли значение на противоположное
                    }
                    else//если да,то меняем направление по х
                    {
                        if (!change2)//проверяем меня ли мы направление второго уровня
                        {
                            res = -res;//если нет то меняем
                            change1 = false;//сбрасываем проверку направлению у
                            res2 = Random.Range(0, 1.0f) < 0.5f ? -1 : 1;//определяем куда пойдем
                            change2 = true;//помечаем что меняли направление
                        }
                        else//если да
                        {
                            change2 = false;
                            change1 = false;
                            int xLast = x;//то возвращаемся назад
                            x = maze[xLast, y].xLastPart;
                            y = maze[xLast, y].yLastPart;
                            
                        }
                    }
                }
                else
                {
                    maze[x, y + res2].isActive = true;
                    maze[x, y + res2].xLastPart = x;//помечаем у следующей детали предыдущую
                    maze[x, y + res2].yLastPart = y;
                    maze[x, y].xNextPart[maze[x, y].next] = x;
                    maze[x, y].yNextPart[maze[x, y].next] = y + res2;
                    maze[x, y].next++;
                    maze[x, y].nextSize++;
                    y = y + res2;
                    change1 = false;
                    change2 = false;
                    cheksParts++;
                    res = Random.Range(0, 1.0f) < 0.5f ? -1 : 1;//определяем направление движения
                    res2 = Random.Range(0, 1.0f) < 0.5f ? -1 : 1;//определяем куда пойдем

                }
            }
            else
            {//двигаемся вверх или вниз
                if (maze[x + res2, y].isActive || x + res2 < 0 || x + res2 == size)//проверяем была ли проверена эта точка или вышли за пределы лабиринта
                {
                    if (!change1)//проверяем меняли ли направление движения в следующую клетку первого уровня
                    {
                        res2 = -res2; //если да,то двигаемся в противоположном направлении
                        change1 = true;//помечаем что меняли значение на противоположное
                    }
                    else//если да,то меняем направление по х
                    {
                        if (!change2)//проверяем меня ли мы направление второго уровня
                        {
                            res = -res;//если нет то меняем
                            change1 = false;//сбрасываем проверку направлению у
                            res2 = Random.Range(0, 1.0f) < 0.5f ? -1 : 1;//определяем куда пойдем 
                            change2 = true;//помечаем что меняли направление
                        }
                        else//если да
                        {
                            change2 = false;
                            change1 = false;
                            int xLast = x;//то возвращаемся назад
                            x = maze[xLast, y].xLastPart;
                            y = maze[xLast, y].yLastPart;
                        }
                    }
                }
                else
                {
                    maze[x + res2, y].isActive = true;
                    maze[x + res2, y].xLastPart = x;//помечаем у следующей детали предыдущую
                    maze[x + res2, y].yLastPart = y;
                    maze[x, y].xNextPart[maze[x, y].next] = x + res2;
                    maze[x, y].yNextPart[maze[x, y].next] = y;
                    maze[x, y].next++;
                    maze[x, y].nextSize++;
                    x = x + res2;
                    change1 = false;
                    change2 = false;
                    cheksParts++;
                    res = Random.Range(0, 1.0f) < 0.5f ? -1 : 1;//определяем направление движения
                    res2 = Random.Range(0, 1.0f) < 0.5f ? -1 : 1;//определяем куда пойдем

                }
            }


        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
