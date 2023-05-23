using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleGenom.Logic;

namespace SimpleGenom
{
  public class Game1 : Game
  {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Field _field;
    private Vector2 _fieldPos;

    public Game1()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
      
    }

    protected override void Initialize()
    {
      // TODO: Add your initialization logic here

      base.Initialize();
      _graphics.PreferredBackBufferHeight = (int)Decimal.Round((decimal)(GraphicsDevice.Adapter.CurrentDisplayMode.Height/1));
      _graphics.PreferredBackBufferWidth = (int)Decimal.Round((decimal)(GraphicsDevice.Adapter.CurrentDisplayMode.Width / 1));
      _graphics.ApplyChanges();
      _fieldPos = new Vector2(200, 0);
      
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);
      _field = new Field(150, 250, _spriteBatch);
      //textures
      _field.Textures.Add(Content.Load<Texture2D>("block"));
      _field.Textures.Add(Content.Load<Texture2D>("food"));
      _field.Textures.Add(Content.Load<Texture2D>("box"));
      _field.UnitStatesTexture.Add(Content.Load<Texture2D>("unit"));
      _field.UnitStatesTexture.Add(Content.Load<Texture2D>("unit_hungry"));
      _field.UnitStatesTexture.Add(Content.Load<Texture2D>("unit_dying"));
      //.textures

      // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
          Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        using (StreamWriter sw = new StreamWriter(_field.path))
        {
          foreach (int i in _field.Data)
          {
            sw.WriteLine(i);
          }
          sw.Close();
        }

        Exit();
      }

      _field.Update(gameTime);
      // TODO: Add your update logic here

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.WhiteSmoke);
      _spriteBatch.Begin();
      _field.Draw(_fieldPos);
      _spriteBatch.End();
      // TODO: Add your drawing code here

      base.Draw(gameTime);
    }
  }
}