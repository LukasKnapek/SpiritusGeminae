using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
    public GameObject[] rooms4; // rooms with 4 doors
    public GameObject[] rooms3; // rooms with 3 doors
    public GameObject[] rooms2s; // rooms with 2 doors opposite each other: "straight"
    public GameObject[] rooms2t; // rooms with 2 doors adjacent to each other: "turning"
    public GameObject[] rooms1; // rooms with 1 door
    public GameObject final_room;
    public GameObject first_room;
    public (float x, float y) spawn;
    static int width = 20; // number of rooms wide
    static int height = 20; // number of rooms high
    public int n_paths = 10; // number of connections between set of rooms connected to start and set of rooms connected to end
    public float scale_factor = 0.45f;
    private bool[,,] wall_grid = new bool[width,height,4]; // z dimension is walls | top, right, bottom, left | true if open
    private char[,] room_grid = new char[width,height]; // '\0' is undiscovered, 's' is start, 'e' is end
    private List<(int x, int y)> undisc = new List<(int x, int y)>(); // undiscovered rooms
    private List<(int x, int y)> start = new List<(int x, int y)>(); // rooms connected to the start room
    private List<(int x, int y)> end = new List<(int x, int y)>();  // rooms connected to the end room
    private List<(int x, int y)> adjs = new List<(int x, int y)>(); // undiscovered rooms adjacent to current room from which to select target

    void Awake ()
    {
        var cur_room = (x:0, y:0); // current room, initialise as start room, should always be either a start or end room
        var end_room = (x:width-1, y:height-1); // end room
        var targ_room = (x:0, y:0); // target undiscovered room

        // populate undisc with undiscovered rooms
        for (int i=0; i<width; i++)
        {
            for (int j=0; j<height; j++)
            {
                undisc.Add((x:i, y:j));
            }
        }
        // set start and end rooms
        room_grid[cur_room.x, cur_room.y] = 's';
        room_grid[end_room.x, end_room.y] = 'e';
        undisc.Remove(cur_room);
        start.Add(cur_room);
        undisc.Remove(end_room);
        end.Add(end_room);

        // randomly remove walls between undiscovered rooms and start/end rooms until all rooms are discovered
        int counter = 0;
        while (undisc.Count>0)
        {
            Debug.Log("while" + counter);
            counter++;
            // randomise order of start rooms
            Shuffle(start);
            adjs.Clear();
            foreach ((int x, int y) c_room in start)
            {
                cur_room = c_room;
                CheckAdjs(undisc, cur_room);

                // first start room that has adjacents, pick one and make it a start room
                if (adjs.Count > 0)
                {
                    targ_room = adjs[Random.Range(0, adjs.Count)];
                    MoveRoom('s', cur_room, targ_room);
                    start.Add(targ_room);
                    break;
                }
            }

            // randomise order of end rooms
            Shuffle(end);
            adjs.Clear();
            foreach ((int x, int y) c_room in end)
            {
                cur_room = c_room;
                CheckAdjs(undisc, cur_room);

                // first end room that has adjacents, pick one and make it an end room
                if (adjs.Count > 0)
                {
                    targ_room = adjs[Random.Range(0, adjs.Count)];
                    MoveRoom('e', cur_room, targ_room);
                    end.Add(targ_room);
                    break;
                }
            }
            if (counter>1000)
            {
                Debug.Log("break");
                break;
            }
        }

        // make n_paths connections between start rooms and end rooms
        for (int i=0; i<n_paths; i++)
        {
            // randomise order of start rooms
            Shuffle(start);
            adjs.Clear();
            foreach ((int x, int y) c_room in start)
            {
                cur_room = c_room;
                CheckAdjs(end, cur_room);

                // first start room that has adjacents, pick one and connect it
                if (adjs.Count > 0)
                {
                    targ_room = adjs[Random.Range(0, adjs.Count)];
                    BreakWalls(cur_room, targ_room);
                    break;
                }
            }
        }
        
        // place the rooms
        GameObject room = rooms1[0]; // appease the unity gods
        bool final_room_placed = false;
        bool first_room_placed = false;
        for (int i=0; i<width; i++)
        {
            for (int j=0; j<height; j++)
            {
                // set current position on the grid
                transform.position = new Vector3(i*scale_factor*50f, 0f, j*scale_factor*50f);

                bool top    = wall_grid[i, j, 0];
                bool right  = wall_grid[i, j, 1];
                bool bottom = wall_grid[i, j, 2];
                bool left   = wall_grid[i, j, 3];

                // Debug.Log("i"+i+",j"+j+top+right+bottom+left);

                if (top && right && bottom && left)
                {
                    // 4-door room
                    int room_i = Random.Range(0, rooms4.Length);
                    room = rooms4[room_i];
                    // rotate room
                    int dir = Random.Range(0,4);
                    transform.rotation = Quaternion.Euler(0, dir*90, 0);
                }
                else if ( (top&&right&&bottom)  ||
                          (right&&bottom&&left) ||
                          (bottom&&left&&top)   ||
                          (left&&top&&right) )
                {
                    // 3-door room
                    int room_i = Random.Range(0, rooms3.Length);
                    room = rooms3[room_i];
                    // rotate room
                    if      (!top)    { transform.rotation = Quaternion.Euler(0, 270, 0); }
                    else if (!right)  { transform.rotation = Quaternion.identity; }
                    else if (!bottom) { transform.rotation = Quaternion.Euler(0, 90, 0); }
                    else if (!left)   { transform.rotation = Quaternion.Euler(0, 180, 0); }
                }
                else if ( (top && bottom) ^ (right && left) )
                {
                    // 2-door straight room
                    int room_i = Random.Range(0, rooms2s.Length);
                    room = rooms2s[room_i];
                    // rotate room
                    int dir = Random.Range(0,2);
                    if      (top && bottom) { transform.rotation = Quaternion.Euler(0, dir*180 + 90, 0); }
                    else if (right && left) { transform.rotation = Quaternion.Euler(0, dir*180, 0); }
                }
                else if ( (top && right) || (right && bottom) || (bottom && left) || (left && top) )
                {
                    // 2-door turning room
                    int room_i = Random.Range(0, rooms2t.Length);
                    room = rooms2t[room_i];
                    // rotate room
                    if      (top && right)    { transform.rotation = Quaternion.Euler(0, 90, 0); }
                    else if (right && bottom) { transform.rotation = Quaternion.Euler(0, 180, 0); }
                    else if (bottom && left)  { transform.rotation = Quaternion.Euler(0, 270, 0); }
                    else if (left && top)     { transform.rotation = Quaternion.identity; }
                }
                else if (top || right || bottom || left)
                {
                    // 1-door room
                    if ( !final_room_placed && j<(height/2) ) {
                        room = final_room;
                        final_room_placed = true;
                    }
                    else if ( !first_room_placed && j>(height*3/4) && i>(width*3/4) ) {
                        room = first_room;
                        first_room_placed = true;
                    }
                    else {
                    int room_i = Random.Range(0, rooms1.Length);
                    room = rooms1[room_i];
                    }
                    // rotate room
                    if      (top)    { transform.rotation = Quaternion.Euler(0, 90, 0); }
                    else if (right)  { transform.rotation = Quaternion.Euler(0, 180, 0); }
                    else if (bottom) { transform.rotation = Quaternion.Euler(0, 270, 0); }
                    else if (left)   { transform.rotation = Quaternion.identity; }
                }
                // place room
                GameObject new_room =  Instantiate(room, transform.position, transform.rotation);
                new_room.transform.localScale = new Vector3(scale_factor, scale_factor, scale_factor);
            }
        }

        GameObject startRoom = GameObject.Find("prefabstart(Clone)");
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = startRoom.transform.position + new Vector3(0, 5, 0);
    }

    private static System.Random rng = new System.Random();
    public static void Shuffle<T>(IList<T> list)
    {
        for (var i=0; i < list.Count - 1; i++)
        {
            Swap(list, i, rng.Next(i, list.Count));
        }
    }
    public static void Swap<T>(IList<T> list, int i, int j)
    {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }

    void CheckAdjs (List<(int x, int y)> room_type, (int x, int y) cur_room)
    // checks which of the 4 adjacent rooms to cur_room are of room_type and puts them in adjs
    {
        adjs.Clear();

        (int x, int y)[] adj_inds = {(x:-1,y:0), (x:0,y:-1), (x:1,y:0), (x:0,y:1)};

        foreach ((int x, int y) ind in adj_inds)
        {
            var adj = (x: cur_room.x + ind.x, y: cur_room.y + ind.y);
            if (room_type.Contains(adj))
                {
                    adjs.Add(adj);
                }
        }
    }

    void MoveRoom (char dest, (int x, int y) cur_room, (int x, int y) targ_room)
    // moves a room to the given destination: 's' for start, 'e' for end
    {
        room_grid[targ_room.x, targ_room.y] = dest;
        undisc.Remove((x:targ_room.x, y:targ_room.y));
        BreakWalls(cur_room, targ_room);
    }

    void BreakWalls ((int x, int y) cur_room, (int x, int y) targ_room)
    // makes a connection between cur_room and targ_room
    {
        if      (targ_room.y < cur_room.y) // top
        {
            wall_grid[cur_room.x, cur_room.y, 0] = true;
            wall_grid[targ_room.x, targ_room.y, 2] = true;
        }
        else if (targ_room.x < cur_room.x) // right
        {
            wall_grid[cur_room.x, cur_room.y, 1] = true;
            wall_grid[targ_room.x, targ_room.y, 3] = true;
        }
        else if (targ_room.y > cur_room.y) // bottom
        {
            wall_grid[cur_room.x, cur_room.y, 2] = true;
            wall_grid[targ_room.x, targ_room.y, 0] = true;
        }
        else if (targ_room.x > cur_room.x) // left
        {
            wall_grid[cur_room.x, cur_room.y, 3] = true;
            wall_grid[targ_room.x, targ_room.y, 1] = true;
        }
    }
}