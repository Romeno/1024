using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Cell
{
    public int num;
    public GameObject go;
}

public class Game1024 : MonoBehaviour
{
    public float cellSize;
    public Vector2Int fieldSize;

    public float percentToSpawn2;
    public int initialNumberCount;

    public Transform cellParent;
    public GameObject cellPrefab;
    public GameObject gameOverObject;

    public int score;

    private Cell[,] mathModel;
    private Queue<Cell> cellPool;

    private bool moving;
    private bool spawning;
    private bool gameOver;

    private int emptyCellCount;

    #region Game1024 Initialization

    void Start()
    {
        mathModel = new Cell[fieldSize.y, fieldSize.x];

        cellPool = new Queue<Cell>();

        NewGame();
    }

    void NewGame()
    {
        for (int i = 0; i < fieldSize.y; i++)
        {
            for (int j = 0; j < fieldSize.x; j++)
            {
                if (mathModel[i, j] != null)
                {
                    DeleteCell(mathModel[i, j]);
                    mathModel[i, j] = null;
                }
            }
        }

        moving = false;
        spawning = false;
        gameOver = false;

        score = 0;

        emptyCellCount = fieldSize.x * fieldSize.y;

        for (int i = 0; i < initialNumberCount; i++)
        {
            StartSpawnNumber(0.0f);
        }
    }

    #endregion

    #region Game1024 Update

    void Update()
    {
        if (CheckGameOver())
        {
            GameOverState();
        }
        else
        {
            GameState();
        }
    }

    void GameState()
    {
        bool spawnNumber = false;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (!moving && !spawning)
        {
            if (Time.time % 0.16f < 0.016f)
            {
                if (Mathf.Abs(horizontal) > 0.001f)
                {
                    spawnNumber = true;

                    if (horizontal > 0)
                    {
                        MoveNumbers1();
                    }
                    else
                    {
                        MoveNumbers1();
                    }
                }

                if (Mathf.Abs(vertical) > 0.001f)
                {
                    spawnNumber = true;

                    if (vertical > 0)
                    {
                        MoveNumbers1();
                    }
                    else
                    {
                        MoveNumbers1();
                    }
                }

                if (spawnNumber)
                {
                    if (emptyCellCount >= fieldSize.x * fieldSize.y)
                    {

                    }
                    else
                    {
                        StartSpawnNumber(percentToSpawn2);
                    }
                }
            }
        }
    }

    void MoveNumbers1()
    {
        
    }

    bool CheckGameOver()
    {
        return emptyCellCount == 0;
    }

    void GameOverState()
    {
        Animator a = gameOverObject.transform.GetChild(0).GetComponent<Animator>();
        if (!gameOver)
        {
            gameOverObject.SetActive(true);
            gameOver = true;
        }
        else
        {
            if (a.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !a.IsInTransition(0))
            {
                if (Input.anyKeyDown)
                {
                    gameOverObject.SetActive(false);
                    NewGame();
                }
            }
        }
    }

    void StartSpawnNumber(float percentToSpawnTwo)
    {
        int num = 1;
        int k = 0;
        bool spawned = false;
        int spawnOffset = Random.Range(0, emptyCellCount);

        if (Random.Range(0.0f, 1.0f) < percentToSpawnTwo)
        {
            num = 2;
        }

        for (int i = 0; i < fieldSize.y && !spawned; i++)
        {
            for (int j = 0; j < fieldSize.x && !spawned; j++)
            {
                if (mathModel[i, j] == null)
                {
                    if (k >= spawnOffset)
                    {
                        //Debug.Log("x = " + j + " y = " + i);
                        mathModel[i, j] = CreateNewCell();
                        mathModel[i, j].num = num;
                        mathModel[i, j].go.transform.position = MathModelPos2GlobalPos(j, i);
                        mathModel[i, j].go.transform.GetChild(0).GetComponent<Text>().text = num.ToString();
                        spawned = true;
                    }

                    k += 1;
                }
            }
        }
    }

    #endregion

    #region Game1024 Math

    Vector3 MathModelPos2GlobalPos(int x, int y)
    {
        return new Vector3(x * cellSize - fieldSize.x + cellSize / 2, -y * cellSize + fieldSize.y - cellSize / 2, 0);
    }

    #endregion

    #region Game1024 Utility

    Cell CreateNewCell()
    {
        Cell cell;

        if (cellPool.Count == 0)
        {
            cell = new Cell();
            cell.go = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity, cellParent);
        }
        else
        {
            cell = cellPool.Dequeue();
            cell.go.SetActive(true);
        }

        emptyCellCount--;
        return cell;
    }

    void DeleteCell(Cell cell)
    {
        cell.go.SetActive(false);
        cellPool.Enqueue(cell);
    }

    #endregion
}
