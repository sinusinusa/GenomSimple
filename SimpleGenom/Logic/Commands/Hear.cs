namespace SimpleGenom.Logic.Commands;

public class Hear: Command
{
  public override int id { get; } = 8;

  public new void Do(Unit unit, Field field)
  {
    if (unit.Coord.X < field.Horizontal - 2 && unit.Coord.Y < field.Vertical - 2)
    {
      bool isUnit = false;
      bool isFood = false;
      bool isBox = false;
      for (int i = 0; i < 8; i++)
      {
        if (field.GetUnit(unit.Coord, i) != null)
        {
          isUnit = true;
        }

        if (field.GetBlock(unit.Coord, i).id == 2)
        {
          isBox = true;
        }

        if (field.GetBlock(unit.Coord, i).id == 1)
        {
          isFood = true;
        }
      }

      if (isUnit)
      {
        unit.Next();
        unit.Next();
        unit.Next();
        return;
      }
      if (isBox)
      {
        unit.Next();
        unit.Next();
        return;
      }
      if (isFood)
      {
        unit.Next();
        return;
      }
    }
  }

}