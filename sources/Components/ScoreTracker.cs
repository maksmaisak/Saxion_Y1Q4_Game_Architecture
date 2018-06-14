﻿using System;
using System.Drawing;
using Engine;

namespace Spaghetti
{
    /// Keeps track of and renders the score of the players.
    public class ScoreTracker : Component, IRenderer, IEventReceiver<PointScoreEvent>
    {
        private Image fontSheet;
        private uint leftScore, rightScore;

        public ScoreTracker()
        {
            fontSheet = Image.FromFile("assets/digits.png");
        }

        void IRenderer.Render(Graphics graphics)
        {
            // BUG: Start is not guarranteed to be called before the first Render.
            // Ps: have an EnsureStart method on UpdateManager. Call before update and before rendering.
            DisplayScore(graphics);
        }

        public void On(PointScoreEvent pointScore)
        {
            if (pointScore.rightPlayerScored)
            {
                ++rightScore;
            }
            else
            {
                ++leftScore;
            }
        }

        private void DisplayScore(Graphics graphics)
        {
            int y = 25;
            float middleOffset = 20f;

            int numDigits = 2;
            int width = numDigits * (fontSheet.Width / 10);
            float middle = game.size.x * 0.5f;

            DrawNumber(graphics, leftScore , middle - middleOffset - width, y, numDigits);
            DrawNumber(graphics, rightScore, middle + middleOffset        , y, numDigits);
        }

        private void DrawNumber(Graphics graphics, uint number, float x, float y, int numDigitsToDraw = 3)
        {
            const int NumDigits = 10;
            int digitWidth = fontSheet.Width / NumDigits;
            int digitHeight = fontSheet.Height;

            string digits = number.ToString("D" + numDigitsToDraw);

            if (numDigitsToDraw < 0) numDigitsToDraw = digits.Length;
            else numDigitsToDraw = Math.Min(numDigitsToDraw, digits.Length);

            for (int digitIndex = 0; digitIndex < numDigitsToDraw; ++digitIndex)
            {
                int digit = digits[digitIndex] - '0';
                var sourceRect = new Rectangle(digit * digitWidth, 0, digitWidth, digitHeight);

                graphics.DrawImage(
                    fontSheet, 
                    x + digitIndex * digitWidth, y,
                    sourceRect,
                    GraphicsUnit.Pixel
                );
            }
        }
    }
}