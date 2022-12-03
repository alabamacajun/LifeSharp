namespace LifeSharp
{
    internal record CellSeed
    {
        public int x;
        public int y;
        public int value;
    }

    internal class LifeCells : ILifeCells
    {
        private int[,] Cells;
        private readonly int[,] View;
        private readonly int Wide;
        private readonly int High;

        public int[,] GeView() => View;

        public static LifeCells Create(int size, CellSeed[] starts)
        {
            return new LifeCells(size, starts);
        }

        private LifeCells(int size, CellSeed[] starts)
        {
            Cells = new int[size, size];
            View = new int[size, size];

            Wide = size;
            High = size;
            InitField();
            LoadStarts(starts);
        }

        private void LoadStarts(CellSeed[] starts)
        {
            foreach (CellSeed seed in starts)
            {
                Cells[seed.x, seed.y] = seed.value;
                View[seed.x, seed.y] = seed.value;
            }
        }

        private void InitField()
        {
            for (int x = 0; x < Wide; ++x)
            {
                for (int y = 0; y < High; ++y)
                {
                    Cells[x, y] = 0;
                }
            }
        }

        public void Cycle()
        {
            int maxX = Wide - 1;
            int maxY = High - 1;
            var newCells = new int[Wide, High];

            for (int x = 0; x < Wide; ++x)
            {
                for (int y = 0; y < Wide; ++y)
                {
                    var px = x < 1 ? Wide - 1 : x - 1;
                    var py = y < 1 ? High - 1 : y - 1;
                    var nx = x >= maxX ? 0 : x + 1;
                    var ny = y >= maxY ? 0 : y + 1;
                    var ne = Cells[px, py]
                        + Cells[x, py]
                        + Cells[nx, py]
                        + Cells[px, y]
                        + Cells[nx, y]
                        + Cells[px, ny]
                        + Cells[x, ny]
                        + Cells[nx, ny];
                    var c = Cells[x, y];
                    newCells[x, y] = c & 0x1;
                    if (c == 0)
                    {
                        if (ne == 3)
                        {
                            newCells[x, y] = 1;
                            View[x, y] = 1;
                        }
                        else View[x, y] = 0;
                    }
                    else
                    {
                        if (ne == 2 || ne == 3) View[x, y] = 2;
                        else
                        {
                            newCells[x, y] = 0;
                            View[x, y] = 3;
                        }
                    }
                }
            }
            Cells = newCells;
        }
    }
}
