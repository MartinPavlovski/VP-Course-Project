using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Labyrinth
{
    class Ball
    {
        public enum DIRECTION
        {
            RIGHT,
            LEFT,
            UP,
            DOWN,
            NONE
        }

        public int X { get; set; }
        public int Y { get; set; }
        Brush brush;
        public DIRECTION Direction;
        public int RADIUS { get; set; }
        public int start_i = -1;
        public int start_j = -1;

        public Ball(float x, float y, float radius)
        {
            X = (int)x;
            Y = (int)y;
            Direction = DIRECTION.NONE;
            brush = new SolidBrush(Color.Red);
            RADIUS = (int)radius;
        }

        public void ChangeDirection(DIRECTION direction)
        {
            Direction = direction;
        }

        public void Move(int distance)
        {
            if (Direction == DIRECTION.RIGHT)
            {
                X += distance;
                    start_j++;
            }
            if (Direction == DIRECTION.LEFT)
            {
                X -= distance;
                    start_j--;
            }
            if (Direction == DIRECTION.UP)
            {
                Y -= distance;
                    start_i--;
            }
            if (Direction == DIRECTION.DOWN)
            {
                Y += distance;
                    start_i++;
            }
        }

        public void Draw(Graphics g)
        {
            if (true)
                g.FillEllipse(brush, new Rectangle(X,Y,RADIUS,RADIUS));
        }
    }
}
