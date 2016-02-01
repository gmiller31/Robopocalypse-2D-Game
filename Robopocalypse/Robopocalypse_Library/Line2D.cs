using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Robopocalypse_Library;

namespace Robopocalypse_Library
{
    public class Line2D
    {

        private Vector2 startPos;
        private Vector2 endPos;
        private Color color;

        public Line2D(String point1, String point2, Color color)
        {
            startPos = new Vector2(Convert.ToInt16(point1.Split(',')[0]), Convert.ToInt16(point1.Split(',')[1]));
            endPos = new Vector2(Convert.ToInt16(point2.Split(',')[0]), Convert.ToInt16(point2.Split(',')[1]));
            this.color = color;
        }

        public Line2D(Vector2 startingPosition, Vector2 endingPosition, Color color)
        {
            startPos = startingPosition;
            endPos = endingPosition;
            this.color = color;
        }

        public Line2D(Vector2 startingPosition, Vector2 endingPosition, Color color, float angle, Vector2 center, float scale)
        {
            float xRotated = center.X + (startingPosition.X - center.X) * (float)Math.Cos(angle) - (startingPosition.Y - center.Y) * (float)Math.Sin(angle);
            float yRotated = center.Y + (startingPosition.X - center.X) * (float)Math.Sin(angle) + (startingPosition.Y - center.Y) * (float)Math.Cos(angle);

            startPos = new Vector2((xRotated * scale), (yRotated * scale));

            xRotated = center.X + (endingPosition.X - center.X) * (float)Math.Cos(angle) - (endingPosition.Y - center.Y) * (float)Math.Sin(angle);
            yRotated = center.Y + (endingPosition.X - center.X) * (float)Math.Sin(angle) + (endingPosition.Y - center.Y) * (float)Math.Cos(angle);

            endPos = new Vector2((xRotated * scale), (yRotated * scale));

            this.color = color;
        }

        public Line2D(VertexPositionColor startingPosition, VertexPositionColor endingPosition)
        {
            //
            // Since the VertexPositionColor object uses Vector3 object and this object requires
            // Vector2 objects, we need to convert the vector3 to vector2
            //
            startPos = new Vector2(startingPosition.Position.X, startingPosition.Position.Y);
            endPos = new Vector2(endingPosition.Position.X, endingPosition.Position.Y);

            //
            // use the starting position color as the color of the line.  If the starting position color
            // and the ending position's color differ, then too bad.
            //
            this.color = startingPosition.Color;
        }

        public Vector2 StartPosition
        {
            get
            {
                return startPos;
            }

            set
            {
                startPos = value;
            }
        }

        public Vector2 EndPosition
        {
            get
            {
                return endPos;
            }

            set
            {
                endPos = value;
            }
        }

        public Color Color
        {
            get
            {
                return color; ;
            }

            set
            {
                this.color = value;
            }
        }

        public override string ToString()
        {
            return String.Format("Line:[{0}, {1}]", startPos.ToString(), endPos.ToString());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Boolean eq = false;

            if (obj is Line2D)
            {
                Line2D l = (Line2D)obj;
                if (l.startPos == startPos && l.endPos == endPos)
                {
                    eq = true;
                }
            }

            return eq;
        }

        //
        // Converts a linked list of VertexPositionColor objectes into a linked list of Line2D objects
        //
        // Requires that the incoming list has an even number of VertexPositionColor objectes
        //
        public static LinkedList<Line2D> ToLineList(LinkedList<VertexPositionColor> points)
        {
            // This holds the resulting list of lines
            LinkedList<Line2D> lines = new LinkedList<Line2D>();

            // Holds an individual node from the linked list
            LinkedListNode<VertexPositionColor> point1, point2;

            // How many points do we have?
            int num_points = points.Count;

            // Check to see if the list length is even
            if ((num_points % 2) != 0)
            {
                throw new Exception("ToLineList requires an even number of points");
            }

            // Get the first node from the list 
            point1 = points.First;

            // here is what a LinkedListNode<VertexPositionColor> looks like
            //
            //              node
            //    /////////////////////////
            //    // VertexPositionColor //  <- to access this, use the Value property of the LinkedListNode  
            //    /////////////////////////
            //    //  next   //   prev   //  
            //    /////////////////////////
            //

            for (int i = 1; i < num_points; i++)  // Note that we start a 1 rather than 0 since we already have element 0 in point1
            {
                //
                // since the nodes are linked together, use the point1
                // to get the next point in the list
                //
                point2 = point1.Next;

                // Make a line2d and add it to the lines list
                lines.AddLast(new Line2D(point1.Value, point2.Value));

                // 
                // Get the next point after point2
                //
                point1 = point2.Next;

            }

            //
            // we are done!  return the list of lines!
            //
            return lines;

        }

        /**  -- Adapted to C# from the Java JDK * Copyright (c) 2006, Oracle and/or its affiliates. All rights reserved.
         * * ORACLE PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
         * 
         * Returns an indicator of where the specified point 
         * {@code (px,py)} lies with respect to the line segment from 
         * {@code (x1,y1)} to {@code (x2,y2)}.
         * The return value can be either 1, -1, or 0 and indicates
         * in which direction the specified line must pivot around its
         * first end point, {@code (x1,y1)}, in order to point at the
         * specified point {@code (px,py)}.
         * <p>A return value of 1 indicates that the line segment must
         * turn in the direction that takes the positive X axis towards
         * the negative Y axis.  In the default coordinate system used by
         * Java 2D, this direction is counterclockwise.  
         * <p>A return value of -1 indicates that the line segment must
         * turn in the direction that takes the positive X axis towards
         * the positive Y axis.  In the default coordinate system, this 
         * direction is clockwise.
         * <p>A return value of 0 indicates that the point lies
         * exactly on the line segment.  Note that an indicator value 
         * of 0 is rare and not useful for determining colinearity 
         * because of floating point rounding issues. 
         * <p>If the point is colinear with the line segment, but 
         * not between the end points, then the value will be -1 if the point
         * lies "beyond {@code (x1,y1)}" or 1 if the point lies 
         * "beyond {@code (x2,y2)}".
         *
         * @param x1 the X coordinate of the start point of the
         *           specified line segment
         * @param y1 the Y coordinate of the start point of the
         *           specified line segment
         * @param x2 the X coordinate of the end point of the
         *           specified line segment
         * @param y2 the Y coordinate of the end point of the
         *           specified line segment
         * @param px the X coordinate of the specified point to be
         *           compared with the specified line segment
         * @param py the Y coordinate of the specified point to be
         *           compared with the specified line segment
         * @return an integer that indicates the position of the third specified
         *			coordinates with respect to the line segment formed
         *			by the first two specified coordinates.
         * @since 1.2
         */
        public static int relativeCCW(double x1, double y1,
                      double x2, double y2,
                      double px, double py)
        {
            x2 -= x1;
            y2 -= y1;
            px -= x1;
            py -= y1;
            double ccw = px * y2 - py * x2;
            if (ccw == 0.0)
            {
                // The point is colinear, classify based on which side of
                // the segment the point falls on.  We can calculate a
                // relative value using the projection of px,py onto the
                // segment - a negative value indicates the point projects
                // outside of the segment in the direction of the particular
                // endpoint used as the origin for the projection.
                ccw = px * x2 + py * y2;
                if (ccw > 0.0)
                {
                    // Reverse the projection to be relative to the original x2,y2
                    // x2 and y2 are simply negated.
                    // px and py need to have (x2 - x1) or (y2 - y1) subtracted
                    //    from them (based on the original values)
                    // Since we really want to get a positive answer when the
                    //    point is "beyond (x2,y2)", then we want to calculate
                    //    the inverse anyway - thus we leave x2 & y2 negated.
                    px -= x2;
                    py -= y2;
                    ccw = px * x2 + py * y2;
                    if (ccw < 0.0)
                    {
                        ccw = 0.0;
                    }
                }
            }
            return (ccw < 0.0) ? -1 : ((ccw > 0.0) ? 1 : 0);
        }

        /**
         * Tests if the line segment from {@code (x1,y1)} to 
         * {@code (x2,y2)} intersects the line segment from {@code (x3,y3)} 
         * to {@code (x4,y4)}.
         *
         * @param x1 the X coordinate of the start point of the first
         *           specified line segment
         * @param y1 the Y coordinate of the start point of the first
         *           specified line segment
         * @param x2 the X coordinate of the end point of the first
         *           specified line segment
         * @param y2 the Y coordinate of the end point of the first
         *           specified line segment
         * @param x3 the X coordinate of the start point of the second
         *           specified line segment
         * @param y3 the Y coordinate of the start point of the second
         *           specified line segment
         * @param x4 the X coordinate of the end point of the second
         *           specified line segment
         * @param y4 the Y coordinate of the end point of the second
         *           specified line segment
         * @return <code>true</code> if the first specified line segment 
         *			and the second specified line segment intersect  
         *			each other; <code>false</code> otherwise.  
         * @since 1.2
         */
        public static Boolean Intersects(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            return ((relativeCCW(x1, y1, x2, y2, x3, y3) *
                 relativeCCW(x1, y1, x2, y2, x4, y4) <= 0)
                && (relativeCCW(x3, y3, x4, y4, x1, y1) *
                    relativeCCW(x3, y3, x4, y4, x2, y2) <= 0));
        }

        public static Boolean Intersects(Line2D l1, Line2D l2)
        {
            return Intersects(l1.StartPosition.X, l1.StartPosition.Y, l1.EndPosition.X, l1.EndPosition.Y, l2.StartPosition.X, l2.StartPosition.Y, l2.EndPosition.X, l2.EndPosition.Y);
        }




    }
}
