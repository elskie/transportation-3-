using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cubePrefab;
    Vector3 cubePosition; 
    public static GameObject ActiveCube;
    // Start is called before the first frame update
    int AirplaneXPos, AirplaneYPos;
    GameObject[,] cubeGrid; //2d array, a collection of game objects
    int gridXmax, gridYmax;
    bool airplaneActive;
    float TurnDuration,NextTurn;
    public static int cargo;
    int CargoMax;
    //public static int Score;
    int StartXPos, StartYPos;
    int depotX, depotY;

    void Start()
    {
        TurnDuration = 1.5f;//how many seconds pass before the new turn
        NextTurn = TurnDuration; //a variable to act as a way to continuously mine at the miningSpeed
        gridXmax = 16; //make variables if you are repeating numbers that make it a pain to go in a individually retype
        gridYmax = 9;
        cubeGrid = new GameObject[gridXmax, gridYmax]; //keep track of each cube you made by putting them all in an array
        CargoMax = 90;
        cargo = 0;
        //Score = 0;

        for (int y = 0; y < gridYmax; y++)
        {
            for (int x = 0; x < gridXmax; x++)
            {
                cubePosition = new Vector3(x * 2, y * 2, 0);
                cubeGrid[x, y] = Instantiate(cubePrefab, cubePosition, Quaternion.identity);//make cube, tell grid to store it in array
                cubeGrid[x, y].GetComponent<CubeController>().myX =  x;//the cubegrid x,y specifies where the cube is in the array, referencing the one that already exists
                cubeGrid[x, y].GetComponent<CubeController>().myY = y;
            }
        }
        StartXPos = 0;
        StartYPos = 8;
        depotX = 15; 
        depotY = 0;
        cubeGrid[depotX, depotY].GetComponent<Renderer>().material.color = Color.black;
        AirplaneXPos = StartXPos;
        AirplaneYPos = StartYPos;
        cubeGrid[AirplaneXPos, AirplaneYPos].GetComponent<Renderer>().material.color = Color.red;
        airplaneActive = false;
    }

    public void ClickProcess(GameObject clickedCube, int x, int y)
    {
        if (x == AirplaneXPos && y == AirplaneYPos)
        {
            if (airplaneActive)
            {
                //deactivate the active plane, put this first to make code clearer to read without no
                airplaneActive = false;
                clickedCube.transform.localScale /= (1.5f);
            }
            else
            {
                airplaneActive = true;
                clickedCube.transform.localScale *= (1.5f);
            }
        }
        //clicking on the white cubes and not the plane
        else
        {
            if (airplaneActive)
            {
                if ( AirplaneXPos == depotX && AirplaneYPos == depotY)
                {
                    cubeGrid[depotX, depotY].GetComponent<Renderer>().material.color = Color.black;
                }
                else
                {
                    cubeGrid[AirplaneXPos, AirplaneYPos].GetComponent<Renderer>().material.color = Color.white;
                }
                cubeGrid[AirplaneXPos, AirplaneYPos].transform.localScale /= (1.5f);

                AirplaneXPos = x;
                AirplaneYPos = y;
                cubeGrid[x, y].GetComponent<Renderer>().material.color = Color.red;
                cubeGrid[AirplaneXPos, AirplaneYPos].transform.localScale *= (1.5f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > NextTurn)
        {
            NextTurn += TurnDuration;
            // is the plane in the upper left? give cargo
            if (AirplaneXPos == StartXPos && AirplaneYPos == StartYPos && cargo < CargoMax)
            {
                cargo += 10;
                print(cargo + " tons loaded");
            }
            // is the plane is the lower right? give points
            if (AirplaneXPos == depotX && AirplaneYPos == depotY)
            {
                ScoreScript.ScoreValue += (cargo / 10);
                cargo = 0;
            }
            print("Carrying " + cargo + "   Score: " + ScoreScript.ScoreValue);

        }

    }
}
