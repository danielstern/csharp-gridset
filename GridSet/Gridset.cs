using System;
namespace GridSet
{
    public class GridSet<T>
    {
        public T[,] grid;
        public int width;
        public int height;
        public delegate void GridDelegateFn(T element, int x, int y);
        // public delegate void GridDelegateRefFn(Reference reference);
        public GridSet(int x, int y)
        {
            this.width = x;
            this.height = y;
            grid = new T[x, y];
        }

        /*
        public class Reference
        {
            public int x;
            public int y;
            public T _value;
            public T value
            {
                get { return _value; }
                set { this.parent[x, y] = value; this._value = value; }
            }
            public GridSet<T> parent;

            public Reference(T value, int x, int y, GridSet<T> parent)
            {
                this.x = x;
                this.y = y;
                this.parent = parent;
                this.value = value;
            }



            public Reference Copy(Reference neighbour)
            {
                neighbour.value = this.value;
                return neighbour;
            }

            public void LoopNeighbours(GridDelegateFn g)
            {
                for (var i = -1; i <= 1; i++)
                {
                    for (var j = -1; j <= 1; j++)
                    {
                        if (i != j)
                        {
                            Reference neighbour = Neighbour(x + i, y + j);
                            if (neighbour)
                            {
                                g(neighbour.value, i, j);
                            }
                        }
                    }
                }
            }

            public Reference Neighbour(int offsetX, int offsetY = 0)
            {
                if (parent.WithinBounds(x + offsetX, y + offsetY))
                {
                    return parent.GetReference(x + offsetX, y + offsetY);
                }
                else
                {
                    return null;
                }

            }

            public static implicit operator bool (Reference instance)
            {
                if (instance == null)
                {
                    return false;
                }
                return true;
            }
        }
        */

        public bool WithinBounds(int x, int y)
        {
            return (x >= 0 && x < this.width &&
                y >= 0 && y < height);
        }

        public void Fill(T defaultValue)
        {
            Loop((T t, int x, int y) =>
            {
                grid[x, y] = defaultValue;
            });
        }

        public GridSet<T> GetSection(int startX, int startY, int width, int height)
        {
            int newx = Math.Min(width + startX, this.width - width);
            int newy = Math.Max(height + startY, this.height - height);
            var ret = new GridSet<T>(newx, newy);

            ret.Loop((T value, int x, int y) =>
            {
                ret[x, y] = grid[x + startX, y + startY]; ;
            });
            //Console.

            return ret;
        }

        public void Loop(GridDelegateFn fn)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    T s = grid[i, j];
                    fn(s, i, j);
                }
            }
        }

        /*
        public Reference GetReference(int x, int y)
        {
            return new Reference(grid[x, y], x, y, this);
        }
        */

        public T this[int x, int y]
        {
            get { return grid[x, y]; }
            set { grid[x, y] = value; }
        }
    }

}
