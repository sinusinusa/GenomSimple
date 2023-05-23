namespace SimpleGenom.Logic.Commands;

public class ChangeDirection: Command
{
  public override int id { get; } = 6;

  public new void Do(Unit unit, Field field)
  {
    unit.direction = unit.direction != true;
  }
}