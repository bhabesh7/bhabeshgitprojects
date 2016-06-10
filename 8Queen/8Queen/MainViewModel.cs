using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _8Queen
{
    public class MainViewModel : BaseModel
    {
        const int MAX = 8;

        private int[,] _board;


        public int[,] Board
        {
            get { return _board; }
            set { _board = value; }
        }

        List<List<Cell>> _cellBoard;
        public List<List<Cell>> CellBoard
        {
            get
            {
                return _cellBoard;
            }
            set
            {
                _cellBoard = value;
                RaisePropertyChanged("CellBoard");
            }
        }

        public MainViewModel()
        {
            CellBoard = new List<List<Cell>>(MAX);

            for (int i = 0; i < MAX; i++)
            {
                CellBoard.Add(new List<Cell>(MAX));

                for (int j = 0; j < MAX; j++)
                {
                    CellBoard[i].Add(new Cell());
                }
            }

            Board = new int[MAX, MAX];

            AsyncCall();
        }

        async Task AsyncCall()
        {
            //col 0 
            var solution = await Solve8Queen(0);
            return;
        }

        async Task<bool> Solve8Queen(int col)
        {            
            //base conbdition: if all queens are placed return true
            if (col >= MAX)
            {
                return true;
            }

            for (int row = 0; row < MAX; row++)
            {
                if (IsSafe(row, col))
                {
                    Board[row, col] = 1;
                    CellBoard[row][col].Value = 1;

                    if (await Solve8Queen(col + 1))
                    {
                        return true;
                    }

                    Board[row, col] = 0;
                    CellBoard[row][col].Value = 0;
                }
            }

            return false;
        }

        bool IsSafe(int row, int col)
        {
            //from the left until the current col
            for (int i = 0; i < col; i++)
            {
                if (Board[row, i] == 1)
                {
                    return false;
                }

            }
            

            //upper diagonal
            for (int i = row, j = col; i >= 0 && j >= 0; i--, j--)
            {
                if (Board[i, j] == 1)
                {
                    return false;
                }
                //if ((CellBoard[row][i].Value == 1) || (Board[row, i] == 1))
                //{
                //    return false;
                //}
            }

            //lower diagonal
            for (int i = row, j = col; i < MAX && j >= 0; i++, j--)
            {
                if (Board[i, j] == 1)
                {
                    return false;
                }
                //if ((CellBoard[row][i].Value == 1) || (Board[row, i] == 1))
                //{
                //    return false;
                //}
            }

            return true;
        }

    }
}
