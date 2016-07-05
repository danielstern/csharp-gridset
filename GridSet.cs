
public class GridSet<T>
{
    public T[,] grid;
    public int width;
    public int height;
    public delegate void GridDelegateFn(T element, int x, int y);
    public delegate void GridDelegateRefFn(Reference reference);
    public GridSet(int x, int y) 
    {
        this.width = x;
        this.height = y;
        grid = new T[x, y];
    }

    public class Reference
    {
        public int x;
        public int y;
        public T value;
        public GridSet<T> parent;
        public Reference(T value, int x, int y, GridSet<T> parent)
        {
            this.x = x;
            this.y = y;
            this.value = value;
            this.parent = parent;
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
            if (parent.WithinBounds(x + offsetX, y + offsetY)) {
                return parent.GetReference(x + offsetX, y + offsetY);
            } else
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

    public bool WithinBounds(int x, int y)
    {
        return (x > 0 && x < this.width &&
            y > 0 && y < height);
    }


    public void Loop(GridDelegateRefFn fn)
    {
        Loop((T a, int x, int y) =>
        {
            fn(GetReference(x, y));
        });
    }

    public GridSet<T> GetSection(int startX, int startY, int width, int height)
    {
        var ret = new GridSet<T>(width, height);
        ret.Loop((T value, int x, int y) =>
        {
            //refer.value = grid[refer.x + x, refer.y + x];
            ret[x,y] = grid[x + startX, y + startY]; 
        });

        return ret;
    }
    /*
    public Reference[] GetNeighbours(Reference r)
    {
        //var neighbours = new 
    }
    */
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

    public Reference GetReference(int x, int y)
    {
        return new Reference(grid[x, y],x, y, this);
    }

    public T this[int x, int y]
    {
        get { return grid[x,y]; }
        set { grid[x,y] = value; }
    }
}