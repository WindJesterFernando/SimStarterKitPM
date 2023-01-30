using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapData
{

    static public MapData mapData;

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


        //c.numTilesX = 99999;

        // foreach(MapSpriteDataRepresentation s in mapSprites)
        // {
        //     MapSpriteDataRepresentation deepCopy = new MapSpriteDataRepresentation(s.id, s.x, s.y);
        //     c.mapSprites.AddLast(deepCopy);
        // }


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

                    if(count == 2 || count == 3)
                        nextGenMapTiles[i,j] = TextureSpriteID.Grass;
                    else
                        nextGenMapTiles[i,j] = TextureSpriteID.Water;
                }
                else
                {
                    int count = GetNeighbourCount(i, j);

                    if(count == 3)
                        nextGenMapTiles[i,j] = TextureSpriteID.Grass;
                    else
                        nextGenMapTiles[i,j] = TextureSpriteID.Water;
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
