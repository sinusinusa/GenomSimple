namespace SimpleGenom.Logic.Commands;

public class Look : Command
{
  public override int id { get; } = 2;
  public new void Do(Unit unit, Field field)
  {
    Block blockTarget = field.ExpancedGetBlock(unit.Coord, unit.position);
    Unit? unitTarget = field.GetUnit(unit.Coord, unit.position);
    if (unitTarget == null && blockTarget.id == 0)
    {
      return;
    }

    if (unitTarget != null)
    {
      unit.Next();
      unit.Next();
      unit.Next();
      return;
    }

    if (blockTarget.id == 2)
    {
      unit.Next();
      unit.Next();
      return;
    }
    if (blockTarget.id == 1)
    {
      unit.Next();
      return;
    }
  }
}