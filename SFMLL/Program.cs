using System;
using SFML.Learning;
using SFML.Graphics;
using SFML.Window;

namespace SFMLL
{
    class Program : Game
    {
        static string backgroundTexture = LoadTexture("background.png");
        static string playerTexture = LoadTexture("player.png");
        static string foodTexture = LoadTexture("food.png");

        static string meowSound = LoadSound("meow_sound.wav");
        static string crashSound = LoadSound("cat_crash_sound.wav");
        static string bgMusic = LoadMusic("bg_music.wav");

        static float playerX = 300;
        static float playerY = 220;
        static int playerSize = 56;

        static float playerspeed = 400;
        static int playerDirection = 1;

        static int playerScore = 0;
        static int playerhighScore = 0;

        static float foodX;
        static float foodY;
        static int foodSize = 32;

        static void PlayerMove()
        {
            if (GetKey(Keyboard.Key.W) == true) playerDirection = 0;
            if (GetKey(Keyboard.Key.D) == true) playerDirection = 1;
            if (GetKey(Keyboard.Key.S) == true) playerDirection = 2;
            if (GetKey(Keyboard.Key.A) == true) playerDirection = 3;

            if (playerDirection == 0) playerY -= playerspeed * DeltaTime;
            if (playerDirection == 1) playerX += playerspeed * DeltaTime;
            if (playerDirection == 2) playerY += playerspeed * DeltaTime;
            if (playerDirection == 3) playerX -= playerspeed * DeltaTime;
        }
        static void DrawPlayer()
        {
            if (playerDirection == 0) DrawSprite(playerTexture, playerX, playerY, 64, 64, playerSize, playerSize);
            if (playerDirection == 1) DrawSprite(playerTexture, playerX, playerY, 0, 0, playerSize, playerSize);
            if (playerDirection == 2) DrawSprite(playerTexture, playerX, playerY, 0, 64, playerSize, playerSize);
            if (playerDirection == 3) DrawSprite(playerTexture, playerX, playerY, 64, 0, playerSize, playerSize);
        }

        static void Main()
        {
            InitWindow(800, 600, "Meow");

            SetFont("comic.ttf");

            Random rnd = new Random();

            foodX = rnd.Next(0, 800 - foodSize);
            foodY = rnd.Next(200, 600 - foodSize);

            bool islose = false;

            PlayMusic(bgMusic, 10);
            
            while (true)
            {
                // 1. Расчет
                DispatchEvents();

                if (islose == false)
                {
                    PlayerMove();

                    if (playerX + playerSize > foodX && playerX < foodX + foodSize
                     && playerY + playerSize > foodY && playerY < foodY + foodSize)
                    {
                        foodX = rnd.Next(0, 800 - foodSize);
                        foodY = rnd.Next(200, 600 - foodSize);

                        playerScore += 1;
                        playerspeed += 10;

                        PlaySound(meowSound);
                    }

                    if (playerX + playerSize > 800 || playerX < 0 || playerY + playerSize > 600 || playerY < 150)
                    {
                        islose = true;

                        PlaySound(crashSound);
                    }
                    if (playerScore > playerhighScore)
                    {
                        playerhighScore = playerScore;
                    }
                }

                if (islose == true)
                {
                    if (GetKeyDown(Keyboard.Key.R) == true)
                    {
                        islose = false;
                        playerX = 300;
                        playerY = 220;
                        playerspeed = 400;
                        playerDirection = 1;
                        playerScore = 0;
                    }
                }               
                // Игровая логика

                // 2. Очистка буфера и окна
                ClearWindow();

                // 3. Отрисовка буфера на окне

                DrawSprite(backgroundTexture, 0, 0);

                if (islose == true)
                {
                    SetFillColor(50, 50, 50);
                    DrawText(200, 300, "Ну и чего ты носишься по кухне?!", 24);

                    SetFillColor(70, 70, 70);
                    DrawText(232, 350, "Нажми \"R\" чтобы перезапустить игру!", 18);
                }

                DrawPlayer();

                DrawSprite(foodTexture, foodX, foodY);

                SetFillColor(70, 70, 70);
                DrawText(20, 8, "Съедено корма: " + playerScore.ToString(), 18);

                SetFillColor(70, 70, 70);
                DrawText(210, 8, "Рекорд: " + playerhighScore.ToString(), 18);          

                DisplayWindow();

                // 4. Ожидание
                Delay(1);
            }
        }
    }
}
