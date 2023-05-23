using System;

namespace SimpleGenom.Logic;

public class Command: IDisposable
{
  public virtual int id { get; } = 0;

  public void Dispose()
  {
  }

  public void Do(Unit unit, Field field)
  {
  }
  
}