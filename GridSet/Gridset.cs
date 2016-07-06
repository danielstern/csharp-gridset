using System;
using System.Collections;
namespace GridSet
{
    public class GridSet<T>
    {
        public T[,] grid;
        public int width;
        public int height;
        public delegate void GridDelegateFn(T element, int x, int y);
        public delegate T TAccessorFunction(int x, int y);

        public class GridElementRelationship
        {
            public T element;
            public T original;
            public int offsetX;
            public int offsetY;
        }

        public GridSet(int x, int y, TAccessorFunction a) : this(x, y, default(T)) {
            Fill(a);
        }

        public override string ToString()
        {
            return "Grid ["+width+"x"+height+"]";
        }

        public GridSet(int x, int y) : this(x,y,default(T))  { }
        public GridSet(int x, int y, T def)
        {
            this.width = x;
            this.height = y;
            grid = new T[x, y];
            this.Fill(def);
        }

        public GridElementRelationship[] GetNeighbours(int x, int y)
        {
            var rels = new System.Collections.Generic.List<GridElementRelationship>();
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (WithinBounds(x + i, y + j))
                    {
                        if (!(j == 0 && i == 0))
                        {
                            GridElementRelationship g = new GridElementRelationship();
                            g.original = grid[x, y];
                            g.element = grid[x + i, y + j];
                            g.offsetX = i;
                            g.offsetY = j;

                            rels.Add(g);
                        }                        
                    }
                }
            }
            return rels.ToArray();
        }

        public bool WithinBounds(int x, int y)
        {
            return (x >= 0 && x < this.width &&
                y >= 0 && y < height);
        }

        public void Fill(TAccessorFunction accessor)
        {
            Loop((T t, int x, int y) =>
            {
                grid[x, y] = accessor(x,y);
            });
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
            int maxX = startX + width;
            int maxY = startY + height;
            if (maxX > this.width || maxY > this.height)
            {
                throw new Exception("Invalid section size");
            }

            var gridSection = new GridSet<T>(width, height);

            gridSection.Loop((T value, int x, int y) =>
            {
                gridSection[x, y] = grid[x + startX, y + startY];
            });
            //Console.

            return gridSection;
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
