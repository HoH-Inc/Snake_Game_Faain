using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Snake;


public class Game1 : Game
{
    private Texture2D pixel;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private float moveTimer = 0f;
    private float moveInterval = 0.5f;
    
    private List<Point> snake;
    private Point direction;
    
    private int tileSize = 32;
    private int gridWidth = 20;
    private int gridHeight = 20;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = gridWidth * tileSize;                                                                                                                       
        _graphics.PreferredBackBufferHeight = gridHeight * tileSize;                                                                                                                     
        _graphics.ApplyChanges();
        
        snake = new List<Point>();
        snake.Add(new Point(10, 10)); //head
        snake.Add(new Point(9, 10)); //body
        snake.Add(new Point(8, 10)); //tail
        
        direction = new Point(1, 0); //moving right
        
        
        base.Initialize();
}

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        //inputs
        var keyboard = Keyboard.GetState();
        //arrow keys
        if (keyboard.IsKeyDown(Keys.Up)) direction = new Point(0, -1);
        if (keyboard.IsKeyDown(Keys.Down)) direction = new Point(0, 1);
        if (keyboard.IsKeyDown(Keys.Left)) direction = new Point(-1, 0);
        if (keyboard.IsKeyDown(Keys.Right)) direction = new Point(1, 0);
        //wasd keys
        if (keyboard.IsKeyDown(Keys.W)) direction = new Point(0, -1);
        if (keyboard.IsKeyDown(Keys.S)) direction = new Point(0, 1);
        if (keyboard.IsKeyDown(Keys.A)) direction = new Point(-1, 0);
        if (keyboard.IsKeyDown(Keys.D)) direction = new Point(1, 0);
        
        //movung
        moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (moveTimer > moveInterval)
        {
            moveTimer = 0f;
            
            Point newHead = new Point(snake[0].X + direction.X, snake[0].Y + direction.Y);
            snake.Insert(0, newHead);
            snake.RemoveAt(snake.Count - 1);
            
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();
        foreach (var point in snake)
        {
            _spriteBatch.Draw(pixel, new Rectangle(point.X * tileSize, point.Y * tileSize, tileSize, tileSize), Color.Green);
            
        }
        
        _spriteBatch.End();
        base.Draw(gameTime);
        

        // TODO: Add your drawing code here
    }
}