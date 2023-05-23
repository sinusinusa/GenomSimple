namespace SimpleGenom.Logic.Commands;

public class RotateLeft : Command
{
  public override int id { get; } = 5;

  public new void Do(Unit unit, Field field)
  {
    if ((unit.position - 1) % 8 >= 0)
      unit.position = (unit.position - 1) % 8;
    else unit.position = 8;
  }
}