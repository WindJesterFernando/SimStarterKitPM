using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapData
{

    static public MapData instance;

    public int[,] mapTiles;
    public LinkedList<MapSpriteDataRepresentation> mapSprites;

    public int numTilesX = 20;
    public int numTilesY = 15;

    int selectedEditorButtonTextureID;
    int selectedEditorButtonMapObjectType;

    public void Init()
    {
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
        //if (mapTiles[i, j] == TextureSpriteID.Water)

        //GetTraversableNeighbours()

        //mark neighbours as checked
        //get neighbours' neighbours

        //looping through GetTraversableNeighbours()
        //check if one of the neighbors == end

        //

        if (start.x == end.x && start.y == end.y)
            return true;

        LinkedList<TileLocation> checkedTileLocations = new LinkedList<TileLocation>();
        LinkedList<TileLocation> toBeCheckedTileLocations;// = new LinkedList<TileLocation>();

        // foreach (TileLocation tl in GetTraversableNeighbours(start.x, start.y))
        // {
        //     if (tl.x == end.x && tl.y == end.y)
        //         return true;

        //     toBeCheckedTileLocations.AddLast(tl);
        // }

        toBeCheckedTileLocations = GetTraversableNeighbours(start.x, start.y);
        checkedTileLocations.AddLast(start);

        Debug.Log("********");
        Debug.Log("Initial Neighbours:");
        foreach (TileLocation tl in toBeCheckedTileLocations)
            Debug.Log(tl.x + "," + tl.y);

        // foreach (TileLocation tl in toBeCheckedTileLocations)
        while (toBeCheckedTileLocations.Count > 0)
        {
            TileLocation tileToCheck = toBeCheckedTileLocations.First.Value;
            toBeCheckedTileLocations.RemoveFirst();

            if (tileToCheck.x == end.x && tileToCheck.y == end.y)
                return true;

            // if (DoesListContainTileLocation(tl, checkedTileLocations))
            //     continue;

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
                neighbours.AddLast(new TileLocation(x, y + 1));
        }

        if (y < numTilesY - 1 && x < numTilesX - 1)
        {
            if (mapTiles[x + 1, y + 1] == TextureSpriteID.Grass)
                neighbours.AddLast(new TileLocation(x + 1, y + 1));
        }

        if (x < numTilesX - 1)
        {
            if (mapTiles[x + 1, y] == TextureSpriteID.Grass)
                neighbours.AddLast(new TileLocation(x + 1, y));
        }

        if (x < numTilesX - 1 && y > 0)
        {
            if (mapTiles[x + 1, y - 1] == TextureSpriteID.Grass)
                neighbours.AddLast(new TileLocation(x + 1, y - 1));
        }

        if (y > 0)
        {
            if (mapTiles[x, y - 1] == TextureSpriteID.Grass)
                neighbours.AddLast(new TileLocation(x, y - 1));
        }

        if (x > 0 && y > 0)
        {
            if (mapTiles[x - 1, y - 1] == TextureSpriteID.Grass)
                neighbours.AddLast(new TileLocation(x - 1, y - 1));
        }
        if (x > 0)
        {
            if (mapTiles[x - 1, y] == TextureSpriteID.Grass)
                neighbours.AddLast(new TileLocation(x - 1, y));
        }
        if (x > 0 && y < numTilesY - 1)
        {
            if (mapTiles[x - 1, y + 1] == TextureSpriteID.Grass)
                neighbours.AddLast(new TileLocation(x - 1, y + 1));
        }

        return neighbours;

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

    public TileLocation(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}


// public class MapDataDeepCopy
// {
//     public int[,] mapTiles;
//     public LinkedList<MapSpriteDataRepresentation> mapSprites;

//     public int numTilesX;
//     public int numTilesY;

// }






// resize
// Serialization and Deserialization
// Unit test Input
// Deep copy model data
// Conway’s game of life
