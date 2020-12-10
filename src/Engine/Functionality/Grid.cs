using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Functionality
{
    public class Grid
    {
        public Rectangle Size;
        public bool[,] Weight;

        public Grid(bool[,] weight)
        {
            Size = new Rectangle(0, 0, weight.GetLength(0), weight.GetLength(1));
            Weight = weight;
        }

        public List<Point> Pathfind(Point start, Point end, bool allowDiagonalPathing)
        {
            // nodes that have already been analyzed and have a path from the start to them
            var closedSet = new List<Point>();
            // nodes that have been identified as a neighbor of an analyzed node, but have 
            // yet to be fully analyzed
            var openSet = new List<Point> { start };
            // a dictionary identifying the optimal origin point to each node. this is used 
            // to back-track from the end to find the optimal path
            var cameFrom = new Dictionary<Point, Point>();
            // a dictionary indicating how far each analyzed node is from the start
            var currentDistance = new Dictionary<Point, float>();
            // a dictionary indicating how far it is expected to reach the end, if the path 
            // travels through the specified node. 
            var predictedDistance = new Dictionary<Point, float>();

            // initialize the start node as having a distance of 0, and an estmated distance 
            // of y-distance + x-distance, which is the optimal path in a square grid that 
            // doesn't allow for diagonal movement
            currentDistance.Add(start, 0);
            if(allowDiagonalPathing)
                predictedDistance.Add(start, (end.ToVector2() - start.ToVector2()).Length());
            else
                predictedDistance.Add(start, Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y));

            // if there are any unanalyzed nodes, process them
            while (openSet.Count > 0)
            {
                // get the node with the lowest estimated cost to finish
                var current = (from p in openSet orderby predictedDistance[p] ascending select p).First();

                // if it is the finish, return the path
                if (current.X == end.X && current.Y == end.Y)
                {
                    // generate the found path
                    var path = ReconstructPath(cameFrom, end);
                    return path;
                }

                // move current node from open to closed
                openSet.Remove(current);
                closedSet.Add(current);

                // process each valid node around the current node
                foreach (var neighbor in GetNeighborNodes(current, allowDiagonalPathing))
                {
                    var tempCurrentDistance = currentDistance[current] + (neighbor.ToVector2() - current.ToVector2()).Length();

                    // if we already know a faster way to this neighbor, use that route and 
                    // ignore this one
                    if (closedSet.Contains(neighbor) && tempCurrentDistance >= currentDistance[neighbor])
                    {
                        continue;
                    }

                    // if we don't know a route to this neighbor, or if this is faster, 
                    // store this route
                    if (!closedSet.Contains(neighbor) || tempCurrentDistance < currentDistance[neighbor])
                    {
                        if (cameFrom.ContainsKey(neighbor))
                        {
                            cameFrom[neighbor] = current;
                        }
                        else
                        {
                            cameFrom.Add(neighbor, current);
                        }

                        currentDistance[neighbor] = tempCurrentDistance;
                        predictedDistance[neighbor] = currentDistance[neighbor] + Math.Abs(neighbor.X - end.X) + Math.Abs(neighbor.Y - end.Y);

                        // if this is a new node, add it to processing
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            return new List<Point>();
            // unable to figure out a path, abort.
            /*throw new Exception(
                string.Format(
                    "unable to find a path between {0},{1} and {2},{3}",
                    start.X, start.Y,
                    end.X, end.Y
                )
            );*/
        }

        /// <summary>
        /// Return a list of accessible nodes neighboring a specified node
        /// </summary>
        /// <param name="node">The center node to be analyzed.</param>
        /// <returns>A list of nodes neighboring the node that are accessible.</returns>
        private IEnumerable<Point> GetNeighborNodes(Point node, bool allowDiagonalPathing)
        {
            var nodes = new List<Point>();
            bool up = node.Y - 1 >= 0 && Weight[node.X, node.Y - 1];
            bool right = node.X + 1 < Weight.GetLength(0) && Weight[node.X + 1, node.Y];
            bool down = node.Y + 1 < Weight.GetLength(1) && Weight[node.X, node.Y + 1];
            bool left = node.X - 1 >= 0 && Weight[node.X - 1, node.Y];
            // up
            if (up)
                nodes.Add(new Point(node.X, node.Y - 1));
            // right
            if(right)
                nodes.Add(new Point(node.X + 1, node.Y));
            // down
            if(down)
                nodes.Add(new Point(node.X, node.Y + 1));
            // left
            if(left)
                nodes.Add(new Point(node.X - 1, node.Y));

            if (allowDiagonalPathing)
            {
                // up left
                if(up && left)
                    if (Weight[node.X - 1, node.Y - 1])
                        nodes.Add(new Point(node.X - 1, node.Y - 1));
                // up right
                if (up && right)
                    if (Weight[node.X + 1, node.Y - 1])
                        nodes.Add(new Point(node.X + 1, node.Y - 1));
                // down left
                if (down && left)
                    if (Weight[node.X - 1, node.Y + 1])
                        nodes.Add(new Point(node.X - 1, node.Y + 1));
                // down right
                if (down && right)
                    if (Weight[node.X + 1, node.Y + 1])
                        nodes.Add(new Point(node.X + 1, node.Y + 1));

            }
            return nodes;
        }

        /// <summary>
        /// Process a list of valid paths generated by the Pathfind function and return 
        /// a coherent path to current.
        /// </summary>
        /// <param name="cameFrom">A list of nodes and the origin to that node.</param>
        /// <param name="current">The destination node being sought out.</param>
        /// <returns>The shortest path from the start to the destination node.</returns>
        private List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            if (!cameFrom.Keys.Contains(current))
            {
                return new List<Point> { current };
            }

            var path = ReconstructPath(cameFrom, cameFrom[current]);
            path.Add(current);
            return path;
        }
    }
}
