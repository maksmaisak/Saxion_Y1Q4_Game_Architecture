﻿using System;
using Engine;
using System.Drawing;

namespace Spaghetti
{
    public class MainLevel : GameObject
    {
        private GameObject ball;
        private GameObject leftPaddle;
        private GameObject rightPaddle; // TEMP. Also AI for now. TODO add a player-controlled paddle

        public MainLevel() : base("MainLevel") {}

        void Start()
        {
            ball = new GameObject("Ball");
            ball.Add<Ball>();
            ball.Add<ImageRenderer>().SetImage("assets/ball.png");
            ball.Add<Rigidbody>();
            ball.Add<AABB>().rect = new Rect(-4f, -4f, 8f, 8f);

            leftPaddle  = AddPaddle(20f, "PaddleLeft", ball);
            rightPaddle = AddPaddle(game.size.x - 1f - 20f, "PaddleRight", ball);

            AddBooster();

            new GameObject("ScoreTracker").Add<ScoreTracker>();
        }

        void Update()
        {
            //CheckPaddleHit();
        }

        private void CheckPaddleHit()
        {
            Rigidbody ballRb = ball.Get<Rigidbody>();
            // check for collisions with both paddles or when behind it and resolve
            if (ball.x < leftPaddle.x + 8 && ball.x + 16 > leftPaddle.x && ball.y < leftPaddle.y + 64 && ball.y + 16 > leftPaddle.y)
            {
                Console.WriteLine("Left Hit " + Math.Abs((leftPaddle.y - 32) - (ball.y + 8))); // just for testing
                                                                                               // change y speed depending on hit offset in y
                float dy = Math.Abs((leftPaddle.y - 32) - (ball.y + 8)) / 50.0f;

                ball.Get<Ball>().Resolve(leftPaddle.x + 8, ball.y, +Math.Abs(ballRb.velocity.x), dy * ballRb.velocity.y); // hmm
            }

            if (ball.x < rightPaddle.x + 8 && ball.x + 16 > rightPaddle.x && ball.y < rightPaddle.y + 64 && ball.y + 16 > rightPaddle.y)
            {
                Console.WriteLine("Right Hit");
                float dy = Math.Abs((rightPaddle.y - 32) - (ball.y + 8)) / 50.0f;
                ball.Get<Ball>().Resolve(rightPaddle.x - 16, ball.y, -Math.Abs(ballRb.velocity.x), dy * ballRb.velocity.y); // awful, but who cares, no one is gonna see this.
            }
        }

        private void AddBooster()
        {
            // Boost zone
            var go = new GameObject("Booster");

            // TODO Add the collision detection engine to remove this dependency.
            go.Add<Booster>().ball = ball;

            var imageRenderer = go.Add<ImageRenderer>();
            imageRenderer.SetImage("assets/booster.png");
            imageRenderer.pivot = Vector2.half;

            go.position = new Vector2(game.size.x * 0.5f, game.size.y * 0.25f);
        }

        private static GameObject AddPaddle(float x, string name, GameObject ball)
        {
            var go = new GameObject(name);
            go.position.x = x;
            go.Add<ImageRenderer>().SetImage("assets/paddle.png");
            go.Add<AABB>().rect = new Rect(-4f, -32f, 8f, 64f);
            go.Add<Paddle>();
            go.Add<PaddleAI>().SetBall(ball);

            return go;
        }
    }
}
