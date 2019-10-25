using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake_csharp
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            new Settings();

            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();
            StartGame();
        }

        // Method to update screen 
        private void UpdateScreen(object sender, EventArgs e)
        {
            // If game over is true then go on to check if ENTER key is pressed if yes then start game, if not then continue running code
            if(Settings.GameOver == true)
            {
                if(Input.KeyPress(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if(Input.KeyPress(Keys.Right) && Settings.direction != Directions.Left)
                {
                    Settings.direction = Directions.Right;
                }
                else if(Input.KeyPress(Keys.Left) && Settings.direction != Directions.Right)
                {
                    Settings.direction = Directions.Left;
                }
                else if(Input.KeyPress(Keys.Up) && Settings.direction != Directions.Down)
                {
                    Settings.direction = Directions.Up;
                }
                else if(Input.KeyPress(Keys.Down) && Settings.direction != Directions.Up)
                {
                    Settings.direction = Directions.Down;
                }

                MovePlayer();
            }

            // This function clear the canvas so that there is no trails left after snake passes
            pbCanvas.Invalidate();
        }

        // Method to run whenever player is moving 
        private void MovePlayer()
        {
            for(int i = Snake.Count - 1; i >= 0; i--)
            {
                // 
                if(i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Directions.Right:
                            Snake[i].X++;
                            break;
                        case Directions.Left:
                            Snake[i].X--;
                            break;
                        case Directions.Up:
                            Snake[i].Y--;
                            break;
                        case Directions.Down:
                            Snake[i].Y++;
                            break;
                    }

                    // Set the maximum position for the X and Y in the canvas
                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYpos = pbCanvas.Size.Height / Settings.Height;

                    if(Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X > maxXpos || Snake[i].Y > maxYpos)
                    {
                        Die();
                    }

                    for(int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        // Method to run when game is starting 
        // label 3 automatically is set to false
        // Call new Settings
        private void StartGame()
        {
            label3.Visible = false;
            new Settings();
            Snake.Clear();
            Circle head = new Circle {X = 10, Y = 5};
            Snake.Add(head);
            label2.Text = Settings.Score.ToString();
            GenerateFood();
            name_label.Text = Form2.NameText;
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void GenerateFood()
        {
            int maxXpos = pbCanvas.Size.Width / Settings.Width;
            int maxYpos = pbCanvas.Size.Height / Settings.Height;
            Random rnd = new Random();
            food = new Circle { X = rnd.Next(0, maxXpos), Y = rnd.Next(0, maxYpos) };
        }

        private void Eat()
        {
            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };

            Snake.Add(body);
            Settings.Score += Settings.Points;
            label2.Text = Settings.Score.ToString();
            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        // Private method t 
        private void UpdateGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (Settings.GameOver == false)
            {
                Brush snakeColour;

                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                    {
                        snakeColour = Brushes.Black;
                    }
                    else
                    {
                        snakeColour = Brushes.Green;
                    }

                    canvas.FillEllipse(
                        snakeColour,
                        new Rectangle(
                            Snake[i].X * Settings.Width,
                            Snake[i].Y * Settings.Height,
                            Settings.Width, Settings.Height
                        ));
                    canvas.FillEllipse(
                        Brushes.Red,
                        new Rectangle(
                            food.X * Settings.Width,
                            food.Y * Settings.Height,
                            Settings.Width, Settings.Height
                        ));
                }
            }
            else
            {
                string gameOver = "Game over \n" + "Final score: " + Settings.Score + "\n" + "Press enter to restart game \n";
                label3.Text = gameOver;
                label3.Visible = true;
            }
        }
    }
}
