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
    private Point food;
    private Random random;
    
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

    private void ResetGame() //resets game, delete previous snake, reset snake in start position, and add a food
    {
        snake.Clear();
        snake.Add(new Point(10, 10));
        snake.Add(new Point(9, 10));
        snake.Add(new Point(8, 10));
        
        direction = new Point(1, 0);
        moveTimer = 0f;
        SpawnFood();
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = gridWidth * tileSize;                                                                                                                       
        _graphics.PreferredBackBufferHeight = gridHeight * tileSize;                                                                                                                     
        _graphics.ApplyChanges();
        
        //food
        random = new Random();
        SpawnFood();
        
        //snake starting possition
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

    private void SpawnFood()
    {
        food = new Point(random.Next(0, gridWidth), random.Next(0, gridHeight));
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
            
            //wall colision
            if (newHead.X < 0 || newHead.X >= gridWidth || newHead.Y < 0 || newHead.Y >= gridHeight)
            {
                ResetGame();
                return;
            }
            
            //snake hitbox
            if (snake.Contains(newHead))
            {
                ResetGame();
                return;
            }
            
            snake.Insert(0, newHead);
            
            //respawn after eating food
            if (newHead == food)
            {
                SpawnFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }
            
            
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
        
        //food
        _spriteBatch.Draw(pixel, new Rectangle(food.X * tileSize, food.Y *tileSize, tileSize, tileSize), Color.Red);
        
        _spriteBatch.End();
        base.Draw(gameTime);
        
        
        

        // TODO: Add your drawing code here
    }
}