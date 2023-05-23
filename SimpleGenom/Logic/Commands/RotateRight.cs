namespace SimpleGenom.Logic.Commands;

public class RotateRight: Command
{
  public override int id { get; } = 4;

  public new void Do(Unit unit, Field field)
  {
    unit.position = (unit.position + 1) % 8;
  }
}