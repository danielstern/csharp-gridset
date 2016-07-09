using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace GridSet
{
    public class GridCoordRef
    {
        public int x;
        public int y;

        public GridCoordRef(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            GridCoordRef p = obj as GridCoordRef;
            if ((System.Object)p == null)
            {
                return false;
            }



            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }

        public static bool operator ==(GridCoordRef a, GridCoordRef b)
        {
            return (a.x == b.x && a.y == b.y);
        }

        public static bool operator !=(GridCoordRef a, GridCoordRef b)
        {
            return !(a == b);
        }
        public override string ToString()
        {
            return "Grid Coordinate - [x:" + x + ", y:" + y + "]";
        }

    }

}