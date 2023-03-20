using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Newtonsoft.Json;
using System.IO;


// public static class MapDataInstance
// {


// }

public class MapData
{

    static public MapData instance;

    public int[,] mapTiles;
    public LinkedList<MapSpriteDataRepresentation> mapSprites;

    public int numTilesX = 20;
    public int numTilesY = 15;

    int selectedEditorButtonTextureID;
    int selectedEditorButtonMapObjectType;

    Queue<ShowTileMoveContainer> showTileMoveContainers;

    public TileLocation[,] mapTileLocations;

    Queue<ControlBase> controlQueue;

    public void Init()
    {
        controlQueue = new Queue<ControlBase>();
        CreateMapTiles();
        mapSprites = new LinkedList<MapSpriteDataRepresentation>();
        mapSprites.AddLast(new MapSpriteDataRepresentation(TextureSpriteID.Fighter1, 10, 2));
        mapSprites.AddLast(new MapSpriteDataRepresentation(TextureSpriteID.BlackMage4, 3, 2));
        mapSprites.AddLast(new MapSpriteDataRepresentation(TextureSpriteID.BlackMage1, 2, 3));
    }
    private void CreateMapTiles()
    {
        mapTiles = new int[numTilesX, numTilesY];
        for (int i = 0; i < numTilesX; i++)
        {
            for (int j = 0; j < numTilesY; j++)
            {
                mapTiles[i, j] = TextureSpriteID.Grass;
            }
        }
    }
    public void ProcessEditorButtonPressed(int mapObjectType, int textureID)
    {
        selectedEditorButtonMapObjectType = mapObjectType;
        selectedEditorButtonTextureID = textureID;
    }
    public void ProcessMapTilePressed(int x, int y)
    {
        if (selectedEditorButtonMapObjectType == MapObjectTypeID.Tile)
            mapTiles[x, y] = selectedEditorButtonTextureID;
        else if (selectedEditorButtonMapObjectType == MapObjectTypeID.Sprite)
        {
            if (selectedEditorButtonTextureID != TextureSpriteID.SpriteEraser)
            {
                MapSpriteDataRepresentation removeMe = null;
                foreach (MapSpriteDataRepresentation s in mapSprites)
                {
                    if (s.x == x && s.y == y)
                    {
                        removeMe = s;
                        break;
                    }
                }
                if (removeMe != null)
                    mapSprites.Remove(removeMe);

                mapSprites.AddLast(new MapSpriteDataRepresentation(selectedEditorButtonTextureID, x, y));

            }
            else if (selectedEditorButtonTextureID == TextureSpriteID.SpriteEraser)
            {
                MapSpriteDataRepresentation removeMe = null;
                foreach (MapSpriteDataRepresentation s in mapSprites)
                {
                    if (s.x == x && s.y == y)
                    {
                        removeMe = s;
                        break;
                    }
                }
                if (removeMe != null)
                    mapSprites.Remove(removeMe);
            }

        }
    }
    public void ProcessResize(int x, int y)
    {
        Debug.Log("Processing Resize: " + x + "," + y);

        numTilesX = x;
        numTilesY = y;


        LinkedList<MapSpriteDataRepresentation> removeMes = new LinkedList<MapSpriteDataRepresentation>();

        foreach (MapSpriteDataRepresentation s in mapSprites)
        {
            if (s.x >= numTilesX || s.y >= numTilesY)
                removeMes.AddLast(s);
        }

        foreach (MapSpriteDataRepresentation s in removeMes)
            mapSprites.Remove(s);


    }
    public void ProcessSaveMap(string name)
    {
        Debug.Log("Process SaveMap: " + name);
    }
    public void ProcessLoadMap(string name)
    {
        Debug.Log("Process LoadMap: " + name);
    }

    public MapData MakeDeepCopyOfModelData()
    {
        MapData c = new MapData();

        c.numTilesX = numTilesX;
        c.numTilesY = numTilesY;

        c.mapTiles = new int[numTilesX, numTilesY];

        for (int i = 0; i < numTilesX; i++)
        {
            for (int j = 0; j < numTilesY; j++)
            {
                c.mapTiles[i, j] = mapTiles[i, j];
            }
        }

        c.mapSprites = new LinkedList<MapSpriteDataRepresentation>();

        foreach (MapSpriteDataRepresentation s in mapSprites)
            c.mapSprites.AddLast(s.DeepCopy());

        return c;

    }

    public void DoTheConwayThing()
    {
        int[,] nextGenMapTiles = new int[numTilesX, numTilesY];

        for (int i = 0; i < numTilesX; i++)
        {
            for (int j = 0; j < numTilesY; j++)
            {
                if (mapTiles[i, j] == TextureSpriteID.Grass)
                {
                    int count = GetNeighbourCount(i, j);

                    if (count == 2 || count == 3)
                        nextGenMapTiles[i, j] = TextureSpriteID.Grass;
                    else
                        nextGenMapTiles[i, j] = TextureSpriteID.Water;
                }
                else
                {
                    int count = GetNeighbourCount(i, j);

                    if (count == 3)
                        nextGenMapTiles[i, j] = TextureSpriteID.Grass;
                    else
                        nextGenMapTiles[i, j] = TextureSpriteID.Water;
                }

                // if(mapTiles[i, j] == TextureSpriteID.Grass)
                // {

                // }
            }
        }

        mapTiles = nextGenMapTiles;
    }

    public int GetNeighbourCount(int x, int y)
    {
        int neighbourCount = 0;


        if (y < numTilesY - 1)
        {
            if (mapTiles[x, y + 1] == TextureSpriteID.Grass)
                neighbourCount++;
        }

        if (y < numTilesY - 1 && x < numTilesX - 1)
        {
            if (mapTiles[x + 1, y + 1] == TextureSpriteID.Grass)
                neighbourCount++;
        }

        if (x < numTilesX - 1)
        {
            if (mapTiles[x + 1, y] == TextureSpriteID.Grass)
                neighbourCount++;
        }

        if (x < numTilesX - 1 && y > 0)
        {
            if (mapTiles[x + 1, y - 1] == TextureSpriteID.Grass)
                neighbourCount++;
        }

        if (y > 0)
        {
            if (mapTiles[x, y - 1] == TextureSpriteID.Grass)
                neighbourCount++;
        }

        if (x > 0 && y > 0)
        {
            if (mapTiles[x - 1, y - 1] == TextureSpriteID.Grass)
                neighbourCount++;
        }
        if (x > 0)
        {
            if (mapTiles[x - 1, y] == TextureSpriteID.Grass)
                neighbourCount++;
        }
        if (x > 0 && y < numTilesY - 1)
        {
            if (mapTiles[x - 1, y + 1] == TextureSpriteID.Grass)
                neighbourCount++;
        }


        return neighbourCount;


        // if(x > 0)

        // 


        // if(y > 0)

        // 





        // {
        //     if(mapTiles[x-1, y] == TextureSpriteID.Grass)
        //         neighbourCount++;
        // }


        // {
        //     if(mapTiles[x+1, y] == TextureSpriteID.Grass)
        //         neighbourCount++;
        // }


        // {
        //     if(mapTiles[x, y - 1] == TextureSpriteID.Grass)
        //         neighbourCount++;
        // }


        // {
        //     if(mapTiles[x, y -1] == TextureSpriteID.Grass)
        //         neighbourCount++;
        // }

    }


    public bool DoesPathExist(TileLocation start, TileLocation end)
    {
        //something-someting to determine shortest possible path
        //prioritize next step to be taken by the shortest possible path
        //

        if (start.x == end.x && start.y == end.y)
            return true;

        LinkedList<TileLocation> checkedTileLocations = new LinkedList<TileLocation>();
        LinkedList<TileLocation> toBeCheckedTileLocations;// = new LinkedList<TileLocation>();

        toBeCheckedTileLocations = GetTraversableNeighbours(start.x, start.y);
        checkedTileLocations.AddLast(start);

        Debug.Log("********");
        Debug.Log("Initial Neighbours:");
        foreach (TileLocation tl in toBeCheckedTileLocations)
            Debug.Log(tl.x + "," + tl.y);

        while (toBeCheckedTileLocations.Count > 0)
        {
            TileLocation tileToCheck = toBeCheckedTileLocations.First.Value;
            toBeCheckedTileLocations.RemoveFirst();

            if (tileToCheck.x == end.x && tileToCheck.y == end.y)
                return true;

            checkedTileLocations.AddLast(tileToCheck);

            foreach (TileLocation tl in GetTraversableNeighbours(tileToCheck.x, tileToCheck.y))
            {
                if (DoesListContainTileLocation(tl, checkedTileLocations))
                    continue;
                if (DoesListContainTileLocation(tl, toBeCheckedTileLocations))
                    continue;

                toBeCheckedTileLocations.AddLast(tl);

                Debug.Log("Adding: " + tl.x + "," + tl.y);
            }
        }

        return false;
    }

    public bool DoAStarThing(TileLocation start, TileLocation end)
    {
        showTileMoveContainers = new Queue<ShowTileMoveContainer>();

        mapTileLocations = new TileLocation[mapTiles.GetLength(0), mapTiles.GetLength(1)];
        for (int x = 0; x < mapTiles.GetLength(0); x++)
        {
            for (int y = 0; y < mapTiles.GetLength(1); y++)
            {
                mapTileLocations[x, y] = new TileLocation(x, y);
            }
        }

        //something-someting to determine shortest possible path
        //prioritize next step to be taken by the shortest possible path
        //

        if (start.x == end.x && start.y == end.y)
            return true;

        LinkedList<TileLocation> checkedTileLocations = new LinkedList<TileLocation>();
        LinkedList<TileLocation> toBeCheckedTileLocations = new LinkedList<TileLocation>();

        start.heuristicCost = 0;

        foreach (TileLocation tl in GetTraversableNeighbours(start.x, start.y))
        {
            tl.distanceToEndTile = GetDistanceBetweenTileLocations(tl, end);
            tl.connectingPreviousTile = start;
            tl.heuristicCost = start.heuristicCost + 10;
            toBeCheckedTileLocations.AddLast(tl);
            showTileMoveContainers.Enqueue(new ShowTileMoveContainer(tl.x, tl.y, 0.1f, TextureSpriteID.WindmillTop));
        }


        checkedTileLocations.AddLast(start);
        showTileMoveContainers.Enqueue(new ShowTileMoveContainer(start.x, start.y, 0.25f, TextureSpriteID.WindmillBase));
        // Debug.Log("********");
        // Debug.Log("Initial Neighbours:");
        // foreach (TileLocation tl in toBeCheckedTileLocations)
        //     Debug.Log(tl.x + "," + tl.y);

        while (toBeCheckedTileLocations.Count > 0)
        {
            //GetLowestCostFromList()

            TileLocation lowestTile = null;


            Debug.Log("****************");
            foreach (TileLocation tl in toBeCheckedTileLocations)
            {
                //if()
                Debug.Log("x = " + tl.x + ", y = " + tl.y +  ", h = " + tl.heuristicCost + ", e = " + tl.distanceToEndTile);

                if (lowestTile == null)
                    lowestTile = tl;
                else if (lowestTile.distanceToEndTile + lowestTile.heuristicCost > tl.distanceToEndTile + tl.heuristicCost)
                    lowestTile = tl;
            }


            //TileLocation tileToCheck = lowestTile;//toBeCheckedTileLocations.First.Value;
            Debug.Log("Currently checking " + lowestTile.x + "," + lowestTile.y + "  : dist " + lowestTile.distanceToEndTile);
            toBeCheckedTileLocations.Remove(lowestTile);

            showTileMoveContainers.Enqueue(new ShowTileMoveContainer(lowestTile.x, lowestTile.y, 0.25f, TextureSpriteID.WindmillBase));
            if (lowestTile.x == end.x && lowestTile.y == end.y)
            {
                // Debug.Log("----FOUND-----");
                // Debug.Log(lowestTile.connectingPreviousTile.x + "," + lowestTile.connectingPreviousTile.y);
                TileLocation prev = lowestTile.connectingPreviousTile;
                while (prev != null)
                {
                    Debug.Log(prev.x + "," + prev.y);
                    showTileMoveContainers.Enqueue(new ShowTileMoveContainer(prev.x, prev.y, 0.1f, TextureSpriteID.FarmFieldGrowing));
                    prev = prev.connectingPreviousTile;
                }



                return true;//path has been found
            }

            checkedTileLocations.AddLast(lowestTile);


            //Debug.Log("Adding Traversable Neighbours--");
            foreach (TileLocation tl in GetTraversableNeighbours(lowestTile.x, lowestTile.y))
            {

                //In the event that tl.connectingPreviousTile 
                //has been assigned another, we need to assess which shortest path
                //from the start.

                if (tl.x == start.x && tl.y == start.y)
                    ;
                else if (tl.connectingPreviousTile == null)
                {
                    tl.connectingPreviousTile = lowestTile;
                    tl.heuristicCost = lowestTile.heuristicCost + 10;
                    //Debug.Log("Setting heuristic for " + tl.x + "," + tl.y + "  : h == " + tl.heuristicCost);
                }
                else
                {
                    // Debug.Log("override being consider: " + tl.heuristicCost + ">" + (lowestTile.heuristicCost + 1));
                    // Debug.Log("override being consider for tile : " + tl.x + "," + tl.y);

                    if (tl.heuristicCost > lowestTile.heuristicCost + 10)
                    {
                        tl.connectingPreviousTile = lowestTile;
                        tl.heuristicCost = lowestTile.heuristicCost + 10;
                        //Debug.Log("overriding: setting heuristic for " + tl.x + "," + tl.y + "  : h == " + tl.heuristicCost);
                    }
                }

                if (DoesListContainTileLocation(tl, checkedTileLocations))
                    continue;
                if (DoesListContainTileLocation(tl, toBeCheckedTileLocations))
                    continue;

                tl.distanceToEndTile = GetDistanceBetweenTileLocations(tl, end);



                toBeCheckedTileLocations.AddLast(tl);
                showTileMoveContainers.Enqueue(new ShowTileMoveContainer(tl.x, tl.y, 0.2f, TextureSpriteID.WindmillTop));

                //Debug.Log("Adding: " + tl.x + "," + tl.y + " : dist == " + GetDistanceBetweenTileLocations(tl, end));
            }


        }

        return false;
    }

    public void Update()
    {
        if (showTileMoveContainers != null)
        {
            if (showTileMoveContainers.Count > 0)
            {
                ShowTileMoveContainer stmc = showTileMoveContainers.Peek();
                stmc.holdTime -= Time.deltaTime;

                if (stmc.holdTime <= 0)
                {
                    stmc = showTileMoveContainers.Dequeue();
                    mapTiles[stmc.x, stmc.y] = stmc.tileToSet;
                    TileEditorLogic.tileEditorLogic.GetComponent<TileEditorLogic>().DestoryMapVisuals();
                    TileEditorLogic.tileEditorLogic.GetComponent<TileEditorLogic>().CreateMapVisuals();
                }
            }
        }

        while (controlQueue.Count > 0)
        {
            ControlBase cb = controlQueue.Dequeue();

            if(cb.controlKey == ControlKey.keyI
                && cb.controlKeyState == ControlKeyState.Press)
            {
                SerializeMapData();
            }
            else if(cb.controlKey == ControlKey.keyO
                && cb.controlKeyState == ControlKeyState.Press)
            {
                DeserializeAndLoadMapData();
                TileEditorLogic.tileEditorLogic.GetComponent<TileEditorLogic>().DestoryMapVisuals();
                TileEditorLogic.tileEditorLogic.GetComponent<TileEditorLogic>().CreateMapVisuals();
            }
            else if(cb.controlKey == ControlKey.keyP
                && cb.controlKeyState == ControlKeyState.Press)
            {
                DoAStarThing(new TileLocation(0, 0), new TileLocation(10, 10));
            }


            // if(cb is Control)
            // {

            // }

            // if(cb.GetType() == typeof(CollectionBase))
            // {

            // }




            //if(cb.controlKeyState == )
        }


        //         if (Input.GetKeyDown(KeyCode.D))
        //     MapData.instance.


        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     MapData.instance.
        //     DestoryMapVisuals();
        //     CreateMapVisuals();
        // }

        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     if (MapData.instance.
        //         Debug.Log("Path Found!!!");
        //     else
        //         Debug.Log("Path Not Found!!!");
        // }


    }

    private bool DoesListContainTileLocation(TileLocation tl, LinkedList<TileLocation> list)
    {
        foreach (TileLocation t in list)
        {
            if (t.x == tl.x && t.y == tl.y)
                return true;
        }

        return false;
    }


    public LinkedList<TileLocation> GetTraversableNeighbours(int x, int y)
    {
        LinkedList<TileLocation> neighbours = new LinkedList<TileLocation>();

        if (y < numTilesY - 1)
        {
            if (mapTiles[x, y + 1] == TextureSpriteID.Grass)
                neighbours.AddLast(mapTileLocations[x, y + 1]);
        }

        // if (y < numTilesY - 1 && x < numTilesX - 1)
        // {
        //     if (mapTiles[x + 1, y + 1] == TextureSpriteID.Grass)
        //         neighbours.AddLast(new TileLocation(x + 1, y + 1));
        // }

        if (x < numTilesX - 1)
        {
            if (mapTiles[x + 1, y] == TextureSpriteID.Grass)
                neighbours.AddLast(mapTileLocations[x + 1, y]);//new TileLocation(x + 1, y));
        }

        // if (x < numTilesX - 1 && y > 0)
        // {
        //     if (mapTiles[x + 1, y - 1] == TextureSpriteID.Grass)
        //         neighbours.AddLast(new TileLocation(x + 1, y - 1));
        // }

        if (y > 0)
        {
            if (mapTiles[x, y - 1] == TextureSpriteID.Grass)
                neighbours.AddLast(mapTileLocations[x, y - 1]);
        }

        // if (x > 0 && y > 0)
        // {
        //     if (mapTiles[x - 1, y - 1] == TextureSpriteID.Grass)
        //         neighbours.AddLast(new TileLocation(x - 1, y - 1));
        // }
        if (x > 0)
        {
            if (mapTiles[x - 1, y] == TextureSpriteID.Grass)
                neighbours.AddLast(mapTileLocations[x - 1, y]);
        }
        // if (x > 0 && y < numTilesY - 1)
        // {
        //     if (mapTiles[x - 1, y + 1] == TextureSpriteID.Grass)
        //         neighbours.AddLast(new TileLocation(x - 1, y + 1));
        // }

        return neighbours;

    }


    private float GetDistanceBetweenTileLocations(TileLocation tl1, TileLocation tl2)
    {
        const float cardinalMovement = 10;
        //const float diagonalMovement = 14;

        float xDif = Mathf.Abs(tl1.x - tl2.x);
        float yDif = Mathf.Abs(tl1.y - tl2.y);

        float distance = cardinalMovement * (xDif + yDif);

        return distance;

    }


    public void SerializeMapData()
    {
        string jsonData = JsonConvert.SerializeObject(this);

        Debug.Log(jsonData);

        string filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "saveThing.txt";

        StreamWriter sw = new StreamWriter(filePath);
        sw.WriteLine(jsonData);
        sw.Close();
    }

    public void DeserializeAndLoadMapData()
    {
        string filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "saveThing.txt";
        StreamReader sr = new StreamReader(filePath);
        string jsonReadData = sr.ReadLine();

        MapData md = JsonConvert.DeserializeObject<MapData>(jsonReadData);
        md.controlQueue = new Queue<ControlBase>();

        MapData.instance = md;
    }

    public void EnqueueControl(ControlBase controlToAdd)
    {
        controlQueue.Enqueue(controlToAdd);
    }

}

public class MapSpriteDataRepresentation
{
    public int id;
    public int x, y;

    public MapSpriteDataRepresentation(int ID, int X, int Y)
    {
        id = ID;
        x = X;
        y = Y;
    }

    public MapSpriteDataRepresentation DeepCopy()
    {
        return new MapSpriteDataRepresentation(id, x, y);
    }
}


public static class MapObjectTypeID
{
    public const int Tile = 1;
    public const int Sprite = 2;
}

public class TileLocation
{
    public int x, y;

    public float distanceToEndTile;

    public TileLocation connectingPreviousTile;

    public float heuristicCost;

    //add variables, maybe distance from start/to end

    public TileLocation(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

//save dist variables on tile
//Save path
//save other ???? variables
//Figure out the question to ask about why we are not generating a shortest path.


public class ShowTileMoveContainer
{
    public int x, y;
    public float holdTime;
    public int tileToSet;

    public ShowTileMoveContainer(int x, int y, float holdTime, int tileToSet)
    {
        this.x = x;
        this.y = y;
        this.holdTime = holdTime;
        this.tileToSet = tileToSet;
    }
}