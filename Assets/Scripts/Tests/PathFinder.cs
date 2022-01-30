using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tests{
    public class PathFinder : MonoBehaviour
    {
        public static Vector3[] GetPath (int x, int y, int x2, int y2) {
            // bresenham line algorithm
            int w = x2 - x;
            int h = y2 - y;
            int absW = System.Math.Abs (w);
            int absH = System.Math.Abs (h);

            // Is neighbouring tile
            if (absW <= 1 && absH <= 1) {
                return null;
            }

            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) {
                dx1 = -1;
                dx2 = -1;
            } else if (w > 0) {
                dx1 = 1;
                dx2 = 1;
            }
            if (h < 0) {
                dy1 = -1;
            } else if (h > 0) {
                dy1 = 1;
            }

            int longest = absW;
            int shortest = absH;
            if (longest <= shortest) {
                longest = absH;
                shortest = absW;
                if (h < 0) {
                    dy2 = -1;
                } else if (h > 0) {
                    dy2 = 1;
                }
                dx2 = 0;
            }

            int numerator = longest >> 1;
            Vector3[] path = new Vector3[longest];
            for (int i = 1; i <= longest; i++) {
                numerator += shortest;
                if (numerator >= longest) {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                } else {
                    x += dx2;
                    y += dy2;
                }

                // If not walkable, path is invalid so return null
                // (unless is target tile, which may be unwalkable e.g water)
                if (i != longest && !Entities.EntityMap.walkable[x, y]) {
                    return null;
                }
                path[i - 1] = Entities.EntityMap.tileCenters[x, y];
            }
            return path;
        }
    }
}