using Microsoft.Xna.Framework;

namespace SimpleGenom.Logic.Commands;

public class Move : Command
{
  public override int id { get; } = 3;
  public new void Do(Unit unit, Field field)
  {
    if (field.cellPos(unit.Coord, unit.position).X > field.Horizontal-1)
    {
      unit.Coord = new Vector2(0, unit.Coord.Y);
    }
    if (field.cellPos(unit.Coord, unit.position).Y > field.Vertical-1)
    {
      unit.Coord = new Vector2(unit.Coord.X, 0);
    }
    if (field.cellPos(unit.Coord, unit.position).X <0)
    {
      unit.Coord = new Vector2(field.Horizontal-1, unit.Coord.Y);
    }
    if (field.cellPos(unit.Coord, unit.position).Y < 0)
    {
      unit.Coord = new Vector2(unit.Coord.X, field.Vertical-1);
    }
    if (field.GetBlock(unit.Coord, unit.position).id != 2 &&
        field.GetUnit(unit.Coord, unit.position) == null)
      {
        unit.Coord = field.cellPos(unit.Coord, unit.position);
        if (field.GetBlock(unit.Coord, -1).id == 1)
        {
         field.Cells[(int)unit.Coord.X, (int)unit.Coord.Y] = new Block();
         unit.AddEnergy(field.foodCapacity);
        }
      }


  }
}