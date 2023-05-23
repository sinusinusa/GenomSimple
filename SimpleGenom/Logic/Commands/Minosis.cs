using System;
using System.Globalization;

namespace SimpleGenom.Logic.Commands;

public class Minosis: Command
{
  public override int id { get; } = 7;

  public new void Do(Unit unit, Field field)
  {
    int minosisEnergy = unit.Energy / 2;
    unit.Energy = minosisEnergy;
    int[,] mutatedGenom = unit.Commands;
    if (new Random().Next(100) > 80)
    {
      for (int i = 0; i < mutatedGenom.GetLength(0); i++)
      {
        for (int j = 0; j < mutatedGenom.GetLength(1); j++)
        {
          if (new Random().Next(100) > 95)
          {
            mutatedGenom[i, j] = field.randomList[new Random().Next(field.randomList.Count)];
          }
        }
      }
    }
    field.Borned.Add(new Unit(field.cellPos(unit.Coord, unit.position), mutatedGenom, minosisEnergy, field.lifeTime, unit.Field));
  }
}