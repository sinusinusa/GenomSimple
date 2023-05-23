

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SimpleGenom.Logic.Blocks;
using SimpleGenom.Logic.Commands;

namespace SimpleGenom.Logic;

public class Unit
{
  private int Id;
  //0 - blue (full), 1 - yellow (hungry), 2 - red (dying)
  public int Color;
  public int Energy;
  public int Health;
  public Field Field;
  //private List<int> Commands { get; set; } = new List<int>();
  public int[,] Commands { get; }
  public Vector2 Coord { get; set; }
  // 8 positions 
  public int position;
  // step in genom's line
  public int stateX;

  public int stateY;
  // is Oy direction in Genom's steping
  public bool direction;

  public void Next()
  {
    if (direction == false)
    {
      if (stateX + 1 == Commands.GetLength(0))
      {
        stateY = (stateY + 1) % Commands.GetLength(0);
      }

      stateX = (stateX + 1) % Commands.GetLength(0);
    }

    if (direction == true)
    {
      if (stateY + 1 == Commands.GetLength(0))
      {
        stateX = (stateX + 1) % Commands.GetLength(0);
      }
      stateY = (stateY + 1) % Commands.GetLength(0);
    }
  }

  public int getNext()
  {
    return Commands[stateX, stateY];
  }
  public Unit(Vector2 coord, int[,] commands, int energy, int healh, Field field)
  {
    direction = false;
    stateX = 0;
    stateY = 0;
    position = 0;
    this.Coord = coord;
    Commands = commands;
    Energy = energy;
    Health = healh;
    Field = field;
  }

  public void AddEnergy(int count)
  {
    Energy += count;
    if (Energy > Field.maxEnergy)
    {
      using (var minosis = new Minosis())
      {
        minosis.Do(this, this.Field);
      }
      Energy = Field.maxEnergy;
    }
  }
  public void Do()
  {
    if (getNext() == 1)
    {
      using (var attack = new Attack())
      {
        attack.Do(this, Field);
      }
    }
    if (getNext() == 2)
    {
      using (var look = new Look())
      {
        look.Do(this, Field);
      }
    }
    if (getNext() == 3)
      {
        using (var move = new Move())
        {
          move.Do(this, Field);
        }
      }
    if (getNext() == 4)
      {
        using (var rotateRight = new RotateRight())
        {
          rotateRight.Do(this, Field);
        }
      }
    if (getNext() == 5)
      {
        using (var rotateLeft = new RotateLeft())
        {
          rotateLeft.Do(this, Field);
        }
      }
    if (getNext() == 6)
    {
      using (var changeDirection = new ChangeDirection())
      {
        changeDirection.Do(this, Field);
      }
    }
    if (getNext() == 7)
    {
      using (var minosis = new Minosis())
      {
        minosis.Do(this, Field);
      }
    }
    if (getNext() == 8)
    {
      using (var hear = new Hear())
      {
        hear.Do(this, Field);
      }
    }
    if (getNext() == 9)
    {
      using (var photosynth = new Photosynth())
      {
        photosynth.Do(this, Field);
      }
    }
    if (Energy < Field.lowEnergy)
    {
      Color = 2;
    }
    if (Energy > Field.lowEnergy)
    {
      Color = 1;
    }

    if (Energy > Field.midEnergy)
    {
      Color = 0;
    }

    Energy -= 1;
    Health -= 1;
    if (Energy < 0 || Health <0)
      {
        Field.Died.Add(this);
      }
    Next();
  }

}