using System.Windows.Navigation;

namespace LifeSharp
{
    internal interface ILifeCells
    {
        static LifeCells GetNew(int size, CellSeed[] starts) =>
            LifeCells.Create(size, starts);

        void Cycle();
        int[,] GeView();

    }
}