using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SudokuSolver
{
    public class SudokuSolverVM : ViewModelBase
    {
        public const int ROWS = 9;
        public const int COLS = 9;
        public const int QUADS = 9;
        public const int QUAD_ROWS = 3;
        public const int QUAD_COLS = 3;
        public const int QUAD_CELLS = QUAD_ROWS * QUAD_COLS;
        public const int CELLS_PER_THREE_QUADS = 27;
        public const int QUADRANT_ROW_INDEX_JUMP = 7;
        public const int LOW_VAL = 1;
        public const int NUM_VALS = 9;
        public const int TOTAL_CELLS = ROWS * COLS;

        Dictionary<int, CellHashset> _Cells = new Dictionary<int, CellHashset>();
        Queue<CellHashset> _AnswerProcessing = new Queue<CellHashset>();

        int _Selected;

        public SudokuSolverVM()
        {
            _Cells.Clear();

            for (int i = 0; i < TOTAL_CELLS; i++)
            {
                CellHashset Cell = new CellHashset(this, i, FindTheStartOfIndexRow(i), FindTheStartOfIndexColumn(i), FindTheStartOfIndexQuadrant(i));

                _Cells.Add(i, Cell);

                //Trace.WriteLine(String.Format("Index = {0} Row = {1} Column = {2} Quadrant = {3}", i, FindTheStartOfIndexRow(i), FindTheStartOfIndexColumn(i), FindTheStartOfIndexQuadrant(i)));
            }
        }

        private void Solve()
        {
            while (_AnswerProcessing.Count > 0)
            {
                CellHashset Answered = _AnswerProcessing.Dequeue();

                SolveForCell(Answered);

                SolveForRows();
                SolveForColumns();
                SolveForQuadrants();
            }
        }

        private void SolveForQuadrants()
        {
            for (int i = 0; i < QUADS; i++)
            {
                int[] PossibleAnswerArray = new int[NUM_VALS];
                int[] PossibleCellArray = new int[NUM_VALS];

                int StartOfQuadrantIndex = (i % QUAD_ROWS) * QUAD_ROWS + (i / QUAD_ROWS) * CELLS_PER_THREE_QUADS;
                for (int j = 0; j < QUAD_CELLS; j++)
                {
                    int idx = StartOfQuadrantIndex + j % QUAD_COLS + (int)Math.Floor((double)(j / QUAD_COLS)) * COLS;

                    if (_Cells.TryGetValue(idx, out CellHashset Cell))
                    {
                        if (!Cell.IsAnswered())
                        {
                            foreach (int answer in Cell.PossibleAnswers)
                            {
                                PossibleAnswerArray[answer - 1]++;
                                PossibleCellArray[answer - 1] = idx;
                            }
                        }
                        else
                        {
                            PossibleAnswerArray[Cell.Answer - 1] = 2;
                        }
                    }
                }

                // value is -1 from the actual value 
                for (int value = 0; value < NUM_VALS; value++)
                {
                    // if the PossibleAnswerArray holds a 1, only 1 cell can have that value so it's the answer
                    if (PossibleAnswerArray[value] == 1)
                    {
                        SetCellAnswer(PossibleCellArray[value], value + 1);
                    }
                }
            }
        }

        private void SolveForColumns()
        {
            for (int i = 0; i < COLS; i++)
            {
                int[] PossibleAnswerArray = new int[NUM_VALS];
                int[] PossibleCellArray = new int[NUM_VALS];

                for (int j = i; j < TOTAL_CELLS + i; j += ROWS)
                {
                    if (_Cells.TryGetValue(j, out CellHashset Cell))
                    {
                        if (!Cell.IsAnswered())
                        {
                            foreach (int answer in Cell.PossibleAnswers)
                            {
                                PossibleAnswerArray[answer - 1]++;
                                PossibleCellArray[answer - 1] = j;
                            }
                        }
                        else
                        {
                            PossibleAnswerArray[Cell.Answer - 1] = 2;
                        }
                    }
                }

                // value is -1 from the actual value 
                for (int value = 0; value < NUM_VALS; value++)
                {
                    // if the PossibleAnswerArray holds a 1, only 1 cell can have that value so it's the answer
                    if (PossibleAnswerArray[value] == 1)
                    {
                        SetCellAnswer(PossibleCellArray[value], value + 1);
                    }
                }
            }
        }


        private void SolveForRows()
        {
            for (int i = 0; i < ROWS; i++)
            {
                int[] PossibleAnswerArray = new int[NUM_VALS];
                int[] PossibleCellArray = new int[NUM_VALS];


                for (int j = i * COLS; j < (i + 1) * COLS; j++)
                {
                    if (_Cells.TryGetValue(j, out CellHashset Cell))
                    {
                        if (!Cell.IsAnswered())
                        {
                            foreach (int answer in Cell.PossibleAnswers)
                            {
                                PossibleAnswerArray[answer - 1]++;
                                PossibleCellArray[answer - 1] = j;
                            }
                        }
                        else
                        {
                            PossibleAnswerArray[Cell.Answer - 1] = 2;
                        }
                    }
                }

                // value is -1 from the actual value 
                for (int value = 0; value < NUM_VALS; value++)
                {
                    // if the PossibleAnswerArray holds a 1, only 1 cell can have that value so it's the answer
                    if (PossibleAnswerArray[value] == 1)
                    {
                        SetCellAnswer(PossibleCellArray[value], value + 1);
                    }
                }
            }
        }

        private void SolveForCell(CellHashset Cell)
        {
            MarkRowSolvedForValue(Cell.StartOfRow, Cell.Answer);
            MarkColumnSolvedForValue(Cell.StartOfColumn, Cell.Answer);
            MarkQuadrantSolvedForValue(Cell.StartOfQuadrant, Cell.Answer);
        }

        public void ReportSolvedIndex(CellHashset Cell)
        {
            _AnswerProcessing.Enqueue(Cell);

            if (_AnswerProcessing.Count == 1)
            {
                Solve();
            }
        }

        public void ReportSelected(int SelectedCell)
        {
            _Selected = SelectedCell;
        }

        int FindTheStartOfIndexRow(int Index)
        {
            return (int)Math.Floor((double)(Index / COLS)) * COLS;
        }

        void MarkRowSolvedForValue(int StartOfRowIndex, int Value)
        {
            for (int i = 0; i < COLS; i++)
            {
                int idx = StartOfRowIndex + i;

                CellHashset Cell;
                if (_Cells.TryGetValue(idx, out Cell))
                {
                    Cell.MarkValueSolvedInOtherCell(Value);
                }
            }
        }

        int FindTheStartOfIndexColumn(int Index)
        {
            return Index % COLS;
        }

        void MarkColumnSolvedForValue(int StartOfColumnIndex, int Value)
        {
            for (int i = 0; i < ROWS; i++)
            {
                int idx = StartOfColumnIndex + i * COLS;

                CellHashset Cell;
                if (_Cells.TryGetValue(idx, out Cell))
                {
                    Cell.MarkValueSolvedInOtherCell(Value);
                }
            }
        }

        public int FindTheStartOfIndexQuadrant(int Index)
        {
            int Row = FindTheStartOfIndexRow(Index);
            int Col = FindTheStartOfIndexColumn(Index);
            return (int)Math.Floor((double)(Row / CELLS_PER_THREE_QUADS)) * CELLS_PER_THREE_QUADS + (int)Math.Floor((double)(Col / QUAD_COLS)) * QUAD_COLS;
        }

        void MarkQuadrantSolvedForValue(int StartOfQuadrantIndex, int Value)
        {
            for (int i = 0; i < QUAD_CELLS; i++)
            {
                int idx = StartOfQuadrantIndex + i % QUAD_COLS + (int)Math.Floor((double)(i / QUAD_COLS)) * COLS;

                CellHashset Cell;
                if (_Cells.TryGetValue(idx, out Cell))
                {
                    Cell.MarkValueSolvedInOtherCell(Value);
                }
            }
        }

        public void HandleKeyDown(KeyEventArgs key)
        {
            switch (key.Key)
            {
                case Key.Enter:
                case Key.Escape:
                case Key.Back:
                    {
                        UnSelectAll();
                    }
                    break;
            }
        }

        private void SetCellAnswer(int CellIndex, int Answer)
        {
            CellHashset Current;
            if (_Cells.TryGetValue(CellIndex, out Current))
            {
                Current.SetAnswer(Answer);
            }
        }


        public void UnSelectAll()
        {
            foreach (CellHashset CH in Cells.Values)
            {
                CH.SetSelected(false);
            }
        }

        public Dictionary<int, CellHashset> Cells
        {
            get { return _Cells; }
            set
            {
                _Cells = value;
                OnPropertyChanged("Cells");
            }
        }
    }
}
