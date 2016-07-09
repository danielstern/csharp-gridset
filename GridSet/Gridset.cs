using System;
using System.Collections.Generic;
using System.Collections;

using System.Linq;
namespace GridSet
{

    public class GridSet<T>
    {
        public T[,] grid;
        public int width;
        public int height;
        public int length { get { return width * height; } }
        public delegate void GridDelegateFn(T element, int x, int y);

        //public static bool Tru = new RecursionValidityDelegate(_Tru);

        public static bool _Tru(int x, int y)
        {
            return true;
        }

        public class GridElementRelationship
        {
            public T element;
            public T original;
            public int offsetX;
            public int offsetY;
            public int elementX;
            public int elementY;
        }

        

        public delegate T TAccessorFunction(int x, int y);

        

        public GridSet(int x, int y, TAccessorFunction a) : this(x, y, default(T)) {
            Fill(a);
        }

        public override string ToString()
        {
            return "Grid ["+width+"x"+height+"]";
        }

        //public static void Tilehop

        public GridSet(int x, int y) : this(x,y,default(T))  { }
        public GridSet(int x, int y, T def)
        {
            this.width = x;
            this.height = y;
            grid = new T[x, y];
            this.Fill(def);
        }

        public GridCoordRef[] GetNeighboursCoord(GridCoordRef coord, bool diagonals = true)
        {
            var coords = new List<GridCoordRef>();
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (WithinBounds(coord.x + i, coord.y + j))
                    {
                        if (!(j == 0 && i == 0))
                        {
                            //Console.Write(diagonals);
                            if (diagonals || (i == 0 || j == 0))
                            {
                                coords.Add(new GridCoordRef(coord.x + i, coord.y + j));
                            }

                        }
                    }
                }
            }
            return coords.ToArray();
        }

        public GridElementRelationship[] GetNeighbours(GridCoordRef coord, bool diagonals = true)
        {
            return GetNeighbours(coord.x, coord.y, diagonals);
        }

        public GridElementRelationship[] GetNeighbours(int x, int y, bool diagonals = true)
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
                            //Console.Write(diagonals);
                            if (diagonals || (i == 0 || j == 0))
                            {
                                GridElementRelationship g = new GridElementRelationship();
                                g.original = grid[x, y];
                                g.element = grid[x + i, y + j];
                                g.offsetX = i;
                                g.offsetY = j;
                                g.elementX = x;
                                g.elementY = y;

                                rels.Add(g);
                            }
                            
                        }                        
                    }
                }
            }
            return rels.ToArray();
        }

        public IEnumerable<GridCoordRef[]> NeighbourRecurse(GridCoordRef startCoord, bool useDiagonals, RecursionValidityDelegate ShouldRecurse, int maxSteps = 1000)
        {
            int count = 0;
            bool allHaveBeenFilled = false;
            HashSet<GridCoordRef> recursedTiles = new HashSet<GridCoordRef>() { startCoord };
            LinkedList<GridCoordRef> stack = new LinkedList<GridCoordRef>();
            stack.AddFirst(startCoord);
            while (stack.Count > 0 && !allHaveBeenFilled)
            {
                if (count++ == maxSteps)
                {
                    Console.WriteLine("Warning: Exceeded max recurse steps");
                    Console.WriteLine(count + "," + maxSteps);
                    break;
                }

                GridCoordRef current = stack.First();
                stack.RemoveFirst();

                GridCoordRef[] newRels = GetNeighboursCoord(current, useDiagonals)
                    .Where((GridCoordRef g) => { return ShouldRecurse(g); })
                    .ToArray();

                foreach (GridCoordRef rel in newRels)
                {
                    bool newReference = recursedTiles.Add(rel);
                    if (newReference)
                    {
                        stack.AddLast(rel);
                    }
                }

                if (recursedTiles.Count == length)
                {
                    allHaveBeenFilled = true;
                    Console.Write("All are filled");
                    
                }
                //throw new NotImplementedException();
                    


               // TileCoordRef[] neighbours = GetNeighbours(startCoord.x, startCoord.y, useDiagonals);
            }
            //return recursedTiles.ToArray();
            yield return recursedTiles.ToArray();
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

        public GridSet<T> GetSection(int bottom, int top)
        {
            return GetSection(0, bottom, width, top - bottom);
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

        public T this[GridCoordRef r]
        {
            get { return grid[r.x, r.y]; }
            set { grid[r.x, r.y] = value; }
        }

        public T this[int x, int y]
        {
            get { return grid[x, y]; }
            set { grid[x, y] = value; }
        }

        public static bool operator !(GridSet<T> a)
        {
            return a == null;
        }
    }

}
