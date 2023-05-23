using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Numerics;
using System.Security.AccessControl;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleGenom.Logic.Blocks;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using LibNoise;
using LibNoise.Primitive;

namespace SimpleGenom.Logic;

public class Field
{
  public string path = @".\WriteLines.txt";
  //parameters
  public int maxEnergy = 3000;
  public int cellSize = 6;
  public int foodCapacity = 1000;
  public int genomSize = 60;
  public int lifeTime = 15000;
  //.parameters
  private SpriteBatch Graphics;
  public int Horizontal { get;}
  public int Vertical { get; }
  public Block [,] Cells;
  public List<Unit> Units = new List<Unit>();
  public List<Unit> Died;
  public int midEnergy;
  public int lowEnergy;
  public List<Unit> Borned;
  public double lastFeed = 0;
  public double lastWrite = 0;
  public List<int> Data = new List<int>();


  public List<int> randomList = new List<int>();
  private List<int> allowedCommands = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 8};
  public List<Texture2D> Textures { get; set; } = new List<Texture2D>();
  //public Texture2D UnitTexture { get; set; }
  //public Texture2D UnitTextureHungry { get; set; }
  //public Texture2D UnitTextureDying { get; set; }
  public List<Texture2D> UnitStatesTexture { get; set; } = new List<Texture2D>();

  private void fillRandom()
  {
    for (int i = 0; i < 100; i++)
    {
      if (new Random().Next(100) > 99)
      {
        randomList.Add(7);
      }
      else
      {
        randomList.Add(allowedCommands[new Random().Next(allowedCommands.Count)]);
      }
    }
  }
  public void addPopulation(Vector2 pos, int count, int energy, int health)
  {
    fillRandom();
    for (int i = 0; i < count; i++)
    {
      int[,] Genom = new int[genomSize, genomSize];
      for (int j = 0; j < genomSize; j++)
      {
        for (int k = 0; k < genomSize; k++)
        {
          Genom[j, k] = randomList[new Random().Next(randomList.Count)];
        }
      }
      this.Units.Add( new Unit(pos, Genom, energy, health, this));
    }
  }

  public Unit? getUnitByCord(int x, int y)
  {
    foreach (var unit in Units)
    {
      if (unit.Coord.X == x && unit.Coord.Y == y)
      {
        return unit;
      }
      else return null;
    }

    return null; 
    /*Unit? _unit = null;
    Parallel.ForEach(Units, unit =>
    {
      if (unit.Coord.X == x && unit.Coord.Y == y)
      {
        _unit = unit;
      }
    });
    return _unit;*/

  }

  public Vector2 cellPos(Vector2 coord, int pos)
  {
    if (pos == 0)
    {
      return new Vector2((int)coord.X + 1, (int)coord.Y);
    }
    if (pos == 1)
    {
      return new Vector2((int)coord.X + 1, (int)coord.Y + 1);
    }
    if (pos == 2)
    {
      return new Vector2((int)coord.X, (int)coord.Y + 1);
    }
    if (pos == 3)
    {
      return new Vector2((int)coord.X - 1, (int)coord.Y + 1);
    }
    if (pos == 4)
    {
      return new Vector2((int)coord.X - 1, (int)coord.Y);
    }
    if (pos == 5)
    {
      return new Vector2((int)coord.X - 1, (int)coord.Y - 1);
    }
    if (pos == 6)
    {
      return new Vector2((int)coord.X, (int)coord.Y - 1);
    }
    if (pos == 7)
    {
      return new Vector2((int)coord.X + 1, (int)coord.Y - 1);
    }
    else return new Vector2((int)coord.X, (int)coord.Y);
  }
  public Block GetBlock(Vector2 coord, int pos)
  {
    Vector2 coordPos = cellPos(coord, pos);
    if(coord.X < 0) coord.X = 0;
    if(coord.Y < 0) coord.Y = 0;
    if (coord.X > Horizontal-1) coord.X = Horizontal - 1;
    if (coord.Y > Vertical-1) coord.Y = Vertical - 1;
    if (coordPos.X < Horizontal && coordPos.Y < Vertical && coordPos.X > 0 && coordPos.Y > 0 )
    {
      return Cells[(int)coordPos.X, (int)coordPos.Y];
    }
    //TODO: Прописать все условия
    else
    {
      return Cells[(int)coord.X, (int)coord.Y];
    }
  }

  public Block ExpancedGetBlock(Vector2 coord, int pos)
  {
    Block result = GetBlock(coord, pos);
    if (result.id != 0)
    {
      return result;
    }
    else
    {
      result = GetBlock(cellPos(coord, pos), pos);
      if (result.id != 0)
      {
        return result;
      }
      else
      {
        result = GetBlock(cellPos(coord, pos), pos);
        if (result.id != 0)
        {
          return result;
        }
      }
    }
    return new Block();
  }

  public Unit? GetUnit(Vector2 coord, int pos)
  {
    Vector2 coordPos = cellPos(coord, pos);
    return getUnitByCord((int)coordPos.X, (int)coordPos.Y);
  }

  private int[,] PerlinNoise(int seed)
  {
    SimplexPerlin perlin = new SimplexPerlin();
    perlin.Seed = -123;
    int[,] result = new int[Horizontal, Vertical];
    var rand = new Random();
    for (int i = 0; i < Horizontal; i++)
    {
      for (int j = 0; j < Vertical; j++)
      {
        var value = (float)(perlin.GetValue(i, j, rand.NextSingle())+1)/2;
        if (value > (float)seed/100)
        {
          result[i, j] = 1;
        }
        if(value <= (float)seed/100)
        {
          result[i,j] = 0;
        }

      }
    }

    return result;
  }

  private int[,] Noise(int seed)
  {
    int[,] result = new int[Horizontal, Vertical];
    for (int i = 0; i < Horizontal; i++)
    {
      for (int j = 0; j < Vertical; j++)
      {
        var rand = new Random();
        if (rand.Next(101) > seed)
        {
          result[i, j] = 1;
        }
        else result[i, j] = 0;
      }
    }
    return result;
  }

  private void AddFood(int seed, int beginX, int beginY)
  {
    int[,] generation = Noise(seed);
    for (int i = beginX; i < Horizontal; i++)
    {
      for (int j = beginY; j < Vertical; j++)
      {
        if (generation[i, j] == 1 && Cells[i, j].id == 0)
        {
          Cells[i, j] = new Food();
        }
      }
    }
  }
  private void GenerateField()
  {
    int[,] genBox = PerlinNoise(75);
    for (int i = 0; i < Horizontal; i++)
    {
      for (int j = 0; j < Vertical; j++)
      {
        Cells[i, j] = new Block();

        if (genBox[i, j] == 1)
        {
          Cells[i, j] = new Box();
        }

      }
    }

  }

  public Field(int horizontal, int vertical, SpriteBatch graphics)
  {
    midEnergy = (int)maxEnergy * 2 / 3;
    lowEnergy = (int)maxEnergy / 3;
    Horizontal = horizontal;
    Vertical = vertical;
    Cells = new Block[horizontal, vertical];
    Graphics = graphics;
    GenerateField();
    AddFood(99, 0, 0);
    //addPopulation(new Vector2(250, 250), 5, maxEnergy, lifeTime);
    allowedCommands.Add(9);
    fillRandom();
    addPopulation(new Vector2(50, 50), 10, maxEnergy, lifeTime);

  }

  public void Update(GameTime gameTime)
  {
    if (gameTime.TotalGameTime.TotalMinutes - lastFeed >= 1)
    {
   //   AddFood(99, 200, 150);
   //   lastFeed = gameTime.TotalGameTime.TotalMinutes;
    }

    if (gameTime.TotalGameTime.TotalSeconds - lastWrite >= 1)
    {
      Data.Add(Units.Count);
      lastWrite = gameTime.TotalGameTime.TotalSeconds;
    }

    Borned = new List<Unit>();
    Died = new List<Unit>();
    foreach (var unit in Units)
    {
      if (unit != null)
      {
        unit.Do();
      }
    }
    /*
    Parallel.ForEach(Units, unit =>
    {
      unit.Do();
    });
    */
    /*
    Parallel.ForEach(Died, unit =>
    {
      Cells[(int)unit.Coord.X, (int)unit.Coord.Y] = new Food();
      Units.Remove(unit);
    });
    Parallel.ForEach(Borned, child =>
    {
      Units.Add(child);
    });
    */
    
    foreach (var unit in Died)
    {
      Cells[(int)unit.Coord.X, (int)unit.Coord.Y] = new Food();
      Units.Remove(unit);
    }

    foreach (var child in Borned)
    {
      Units.Add(child);
    }
    

  }
  public void Draw(Vector2 start)
  {
    Graphics.Draw(Textures[0], new Microsoft.Xna.Framework.Rectangle((int)start.X, (int)start.Y, Vertical* cellSize, Horizontal* cellSize), Color.AliceBlue);
    for (int i = 0; i < Horizontal; i++)
    {
      for (int j = 0; j < Vertical; j++)
      {
        if (Cells[i,j].id == 0)
        {
          Graphics.Draw(Textures[0], new Microsoft.Xna.Framework.Rectangle((int)start.X + j* cellSize, (int)start.Y + i* cellSize, cellSize, cellSize), Color.AliceBlue);
        }
        if (Cells[i,j].id == 1)
        {
          Graphics.Draw(Textures[1], new Microsoft.Xna.Framework.Rectangle((int)start.X + j * cellSize, (int)start.Y + i * cellSize, cellSize, cellSize), Color.AliceBlue);
        }
        if (Cells[i, j].id == 2)
        {
          Graphics.Draw(Textures[2], new Microsoft.Xna.Framework.Rectangle((int)start.X + j * cellSize, (int)start.Y + i * cellSize, cellSize, cellSize), Color.AliceBlue);
        }

      }
    }
    
    foreach (var unit in Units)
    {
      if (unit != null)
      {
        Graphics.Draw(UnitStatesTexture[unit.Color],
          new Microsoft.Xna.Framework.Rectangle((int)(start.X + unit.Coord.Y * cellSize),
            (int)(start.Y + unit.Coord.X * cellSize), cellSize, cellSize), Color.AliceBlue);
      }
    }
    
    /*
    Parallel.ForEach(Units, unit =>
    {
      Graphics.Draw(UnitStatesTexture[unit.Color],
        new Microsoft.Xna.Framework.Rectangle((int)(start.X + unit.Coord.Y * cellSize),
          (int)(start.Y + unit.Coord.X * cellSize), cellSize, cellSize), Color.AliceBlue);
    });
    */
  }
}