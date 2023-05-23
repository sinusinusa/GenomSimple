using System.Runtime.InteropServices.ComTypes;

namespace SimpleGenom.Logic.Commands;

public class Attack : Command
{
  public override int id { get; } = 1;
  public new void Do(Unit unit, Field field)
  {
    Unit? unitTarget = field.GetUnit(unit.Coord, unit.position);
    for (int i = 0; i < 8; i++)
    {
      if (field.GetUnit(unit.Coord, i) != null)
      {
        unitTarget = field.GetUnit(unit.Coord, i);
      }
    }
    if (unitTarget != null)
    {
      field.Died.Add(unitTarget);
      unit.AddEnergy(unitTarget.Energy);
    }
  }
}