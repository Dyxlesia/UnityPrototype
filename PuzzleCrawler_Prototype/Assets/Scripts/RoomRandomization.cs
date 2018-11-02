using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomRandomization : MonoBehaviour
{

    [SerializeField] GameObject Smallroom;
    [SerializeField] GameObject BigRoom;
    [SerializeField] int numberOfSmallRooms;

    Vector3 temp;
    Vector3[] vector3s;

    int phase;
    public static int bigroomCounter;
    public static int smallroomCounter;
    float timer;

    // Use this for initialization
    void Start()
    {
        vector3s = new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(2, 0, 0), new Vector3(3, 0, 0), new Vector3(4, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(2, 0, 1), new Vector3(3, 0, 1), new Vector3(4, 0, 1), new Vector3(0, 0, 2), new Vector3(1, 0, 2), new Vector3(2, 0, 2), new Vector3(3, 0, 2), new Vector3(4, 0, 2), new Vector3(0, 0, 3), new Vector3(1, 0, 3), new Vector3(2, 0, 3), new Vector3(3, 0, 3), new Vector3(4, 0, 3), new Vector3(0, 0, 4), new Vector3(1, 0, 4), new Vector3(2, 0, 4), new Vector3(3, 0, 4), new Vector3(4, 0, 4) };

        phase = 1;
        temp = new Vector3();
        bigroomCounter = 0;
        smallroomCounter = 0;
        timer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Place Big Rooms
        if (phase == 1)
        {
            temp.x = Random.Range(-0.501f, 3.5f);
            temp.x = Mathf.RoundToInt(temp.x);

            temp.z = Random.Range(-0.501f, 3.5f);
            temp.z = Mathf.RoundToInt(temp.z);

            if (bigroomCounter < 2)
            {
                Instantiate(BigRoom, (temp * 30) + new Vector3(15, 0, 15), new Quaternion(0, 0, 0, 0));
                bigroomCounter++;
            }
            else if (bigroomCounter == 2)
            {
                timer += Time.deltaTime;

                if (timer > 0.1f)
                {
                    phase = 2;
                    timer = 0;
                }
            }
        }
        else if (phase == 2)
        {
            int rand = Mathf.RoundToInt(Random.Range(-0.51f, vector3s.Length - 0.5f));

            if (vector3s[rand] != null)
            {
                Instantiate(Smallroom, vector3s[rand] * 30, new Quaternion(0, 0, 0, 0));

                Vector3 posToRemove = vector3s[rand];
                vector3s = vector3s.Where(val => val != posToRemove).ToArray();
                smallroomCounter++;             
            }

            if (smallroomCounter == numberOfSmallRooms)
            {
                phase++;
                timer = 0;
            }
        }
        else if (phase == 3)
        {
            timer += Time.deltaTime;

            if (timer < 0.1f)
            {
                return;
            }

            GameObject[] allRooms = GameObject.FindGameObjectsWithTag("floor");

            bool redo = false;

            foreach (GameObject room in allRooms)
            {
                if (room.transform.parent == null && room.name == "Room(Clone)")
                {
                    redo = true;
                }
            }

            if (redo == true)
            {
                //GameObject[] allInstancedRooms = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Name");
                GameObject[] allSmallRooms = GameObject.FindGameObjectsWithTag("floor");

                foreach (GameObject room in allSmallRooms)
                {
                    if (room.name == "Room(Clone)")
                    {
                        Destroy(room);
                    }
                }

                var allBigInstancedRooms = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "BigRoom(Clone)");

                foreach (GameObject room in allBigInstancedRooms)
                {
                    Destroy(room);
                }

                phase = 1;
                bigroomCounter = 0;
                smallroomCounter = 0;
                timer = 0;
                vector3s = new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(2, 0, 0), new Vector3(3, 0, 0), new Vector3(4, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(2, 0, 1), new Vector3(3, 0, 1), new Vector3(4, 0, 1), new Vector3(0, 0, 2), new Vector3(1, 0, 2), new Vector3(2, 0, 2), new Vector3(3, 0, 2), new Vector3(4, 0, 2), new Vector3(0, 0, 3), new Vector3(1, 0, 3), new Vector3(2, 0, 3), new Vector3(3, 0, 3), new Vector3(4, 0, 3), new Vector3(0, 0, 4), new Vector3(1, 0, 4), new Vector3(2, 0, 4), new Vector3(3, 0, 4), new Vector3(4, 0, 4) };
            }
            else if (redo == false)
            {
                phase++;
            }
            
        }
        else if (phase == 5)
        {
            GameObject[] outerRoomList = GameObject.FindGameObjectsWithTag("floor");

            foreach (GameObject room in outerRoomList)
            {
                //if (room)

                //Vector3 posToRemove = vector3s[rand];
                //vector3s = vector3s.Where(val => val != posToRemove).ToArray();
            }

        }
    }
}
