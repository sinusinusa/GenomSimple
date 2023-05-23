using System.Runtime.InteropServices.ComTypes;

namespace SimpleGenom.Logic.Commands;

public class Photosynth : Command
{
  public override int id { get; } = 9;
  public new void Do(Unit unit, Field field)
  {
    if (unit.Coord.X < field.Horizontal / 4 && unit.Coord.Y < field.Vertical)
    {
      unit.AddEnergy(8);
      return;
    }
    if (unit.Coord.X < field.Horizontal / 2 && unit.Coord.Y < field.Vertical)
    {
      //unit.AddEnergy(1);
    }
  }
}