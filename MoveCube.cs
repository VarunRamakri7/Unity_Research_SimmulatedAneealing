using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    private float LOWER_BOUND = -5.0f; // Lower range for random variable
    private float UPPER_BOUND = 5.0f; // Upper range for random variable
    public GameObject staticCube; // Gets the static cube from the game
    private const int MAX_ITERATIONS = 1000 ; // Number of times the script should run
    private const int IDEAL_DIST = 10; // Ideal distance between the two cubes
    private const float IDEAL_ANGLE = 45; // Ideal angle between the two cubes
    private int currentIteration = 0; // The current iteration number
    private const float ROT_WEIGHT = 0.1f; // Value to normalize cost
    private float moveTempCost; // The temporary cost of moving cube
    private float moveActCost; // The actual cost of moving cube
    private float rotTempCost; // The temporary cost of rotating cube
    private float rotActCost; // The actual cost of rotating cube
    private float tempTotCost; // The temporary cost of moving + rotating cube
    private float actTotCost; // The actual cost of moving + rotating cube cube
    private Vector3 tempPosition; // The temporary position after moving the cube
    private Vector3 tempAngle; // The temporary angle after rotating the cube

    // Update is called once per frame
    void Update()
    {
        // Change range for iteration range
        if ((currentIteration < 250) && (currentIteration >= 0))
        {
            LOWER_BOUND = -5.0f;
            UPPER_BOUND = 5.0f;
        }
        if ((currentIteration < 500) && (currentIteration >= 250))
        {
            LOWER_BOUND = -3.0f;
            UPPER_BOUND = 3.0f;
        }
        else if((currentIteration < 750) && (currentIteration >= 500))
        {
            LOWER_BOUND = -2.0f;
            UPPER_BOUND = 2.0f;
        }
        else
        {
            LOWER_BOUND = -1.0f;
            UPPER_BOUND = 1.0f;
        }

        if (currentIteration < MAX_ITERATIONS)
        {
            // TODO: Write text file to check values

            // Randomly get a number to do an operation
            int randCall = Random.Range(1, 4);

                switch (randCall)
                {
                    case 1:
                        moveCube();
                        break;

                    case 2:
                        rotateCube(staticCube);
                        break;

                    case 3:
                        moveCube();
                        rotateCube(staticCube);
                        break;
                }

            // Update the total costs
            tempTotCost = moveTempCost + 0.1f * rotTempCost;
            actTotCost = moveActCost + 0.1f * rotActCost;

            if (tempTotCost <= actTotCost)
            {
                gameObject.transform.position = tempPosition;
                gameObject.transform.eulerAngles = tempAngle;
                Debug.Log(tempTotCost + " vs " + actTotCost);
                System.IO.File.AppendAllText("D:\\UnityGames\\Ramakrishnan_V_ResearchTest\\NewFile.txt", tempTotCost.ToString() + '\n');
            }
        }

        currentIteration++;
    }

    /**
     * This function is called to move the cube such that it reached the ideal distance
     */ 
     public void moveCube()
    {
        // Calculate the current distance between the two cubes
        float curDist = Vector3.Distance(staticCube.transform.position, gameObject.transform.position);

        // Get the random number for the X and Z coordinate
        float randX = Random.Range(LOWER_BOUND, UPPER_BOUND);
        float randZ = Random.Range(LOWER_BOUND, UPPER_BOUND);

        // Calculate the temporary position and distance between the cubes
        tempPosition = new Vector3(gameObject.transform.position.x + randX, 0, gameObject.transform.position.z + randZ);
        float tempDistance = Vector3.Distance(tempPosition, staticCube.transform.position);

        // Calculate the temporary and actual difference between the current
        // distance and the ideal distance
        moveTempCost = Mathf.Abs(tempDistance - IDEAL_DIST);
        moveActCost = Mathf.Abs(curDist - IDEAL_DIST);
    }

    /**
     * This function is called to rotate the moving cube such that it makes and angle
     * of 45 degrees with the static cube
     */

    public void rotateCube(GameObject staticCube)
    {

        // Calculate the current angle between the two cubes
        float curCost = Mathf.DeltaAngle(staticCube.transform.eulerAngles.y, gameObject.transform.eulerAngles.y);

        // Get the random number for the Y coordinate
        float randY = Random.Range(-10, 10);

        // Calculate the temporary angle and angle between the cubes
        tempAngle = new Vector3(0, gameObject.transform.eulerAngles.y + randY, 0);
        float tempDifference = Mathf.DeltaAngle(tempAngle.y, staticCube.transform.eulerAngles.y);

        // Calculate the temporary and actual difference between the current
        // angle and the ideal angle
        rotTempCost = Mathf.DeltaAngle(tempDifference, IDEAL_ANGLE);
        rotActCost = Mathf.DeltaAngle(curCost, IDEAL_ANGLE);
    }
}
