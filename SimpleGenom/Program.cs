
using System;

using var game = new SimpleGenom.Game1();
game.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 900.0);
game.Run();
