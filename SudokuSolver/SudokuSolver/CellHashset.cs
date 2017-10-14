using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SudokuSolver
{
   public class CellHashset : ViewModelBase
   {
      SudokuSolverVM _Parent;

      HashSet<int> _PossibleAnswers;
      int _Index;
      public int StartOfRow;
      public int StartOfColumn;
      public int StartOfQuadrant;
      int _Answer;

      bool _Selected = false;

      public CellHashset(SudokuSolverVM InParent, int InIndex, int Row, int Col, int Quad)
      { 
         _Parent = InParent;
         _Index = InIndex;
         StartOfRow = Row;
         StartOfColumn = Col;
         StartOfQuadrant = Quad;

         _PossibleAnswers = new HashSet<int>();

         for (int i = 1; i <= SudokuSolverVM.NUM_VALS; i++)
         {
            _PossibleAnswers.Add(i);
         }
      }

      public void SetSelected(bool InSelected)
      {
         Selected = InSelected;

         if (Selected)
         {
            _Parent.ReportSelected(_Index);
         }
      }

      public void SetAnswer(int InAnswer)
      {
         Answer = InAnswer;
      }

      public void OnMouseDown()
      {
         _Parent.UnSelectAll();

         SetSelected(true);
      }

      public void MarkValueSolvedInOtherCell(int Value)
      {
         if (_PossibleAnswers.Count > 1)
         {
            _PossibleAnswers.Remove(Value);

            CheckIfSolved();

            UpdatePossibleView();
         }
      }

      void CheckIfSolved()
      {
         if (_PossibleAnswers.Count == 1)
         {
            Answer = _PossibleAnswers.Max();

            ReportSolved();
         }
      }

      private void ReportSolved()
      {
         _Parent.ReportSolvedIndex(this);
      }

      private bool IsAnswerValid(int Input)
      {
         return (Input >= SudokuSolverVM.LOW_VAL && Input <= SudokuSolverVM.NUM_VALS) && _PossibleAnswers.Contains(Input);
      }

      public Visibility IsOnePossible
      {
         get
         {
            return (IsAnswerAvailable == Visibility.Hidden && _PossibleAnswers.Contains(1)) ? Visibility.Visible : Visibility.Hidden;
         }
      }

      public Visibility IsTwoPossible
      {
         get
         {
            return (IsAnswerAvailable == Visibility.Hidden && _PossibleAnswers.Contains(2)) ? Visibility.Visible : Visibility.Hidden;
         }
      }

      public Visibility IsThreePossible
      {
         get
         {
            return (IsAnswerAvailable == Visibility.Hidden && _PossibleAnswers.Contains(3)) ? Visibility.Visible : Visibility.Hidden;
         }
      }

      public Visibility IsFourPossible
      {
         get
         {
            return (IsAnswerAvailable == Visibility.Hidden && _PossibleAnswers.Contains(4)) ? Visibility.Visible : Visibility.Hidden;
         }
      }

      public Visibility IsFivePossible
      {
         get
         {
            return (IsAnswerAvailable == Visibility.Hidden && _PossibleAnswers.Contains(5)) ? Visibility.Visible : Visibility.Hidden;
         }
      }

      public Visibility IsSixPossible
      {
         get
         {
            return (IsAnswerAvailable == Visibility.Hidden && _PossibleAnswers.Contains(6)) ? Visibility.Visible : Visibility.Hidden;
         }
      }

      public Visibility IsSevenPossible
      {
         get
         {
            return (IsAnswerAvailable == Visibility.Hidden && _PossibleAnswers.Contains(7)) ? Visibility.Visible : Visibility.Hidden;
         }
      }

      public Visibility IsEightPossible
      {
         get
         {
            return (IsAnswerAvailable == Visibility.Hidden && _PossibleAnswers.Contains(8)) ? Visibility.Visible : Visibility.Hidden;
         }
      }

      public Visibility IsNinePossible
      {
         get
         {
            return (IsAnswerAvailable == Visibility.Hidden && _PossibleAnswers.Contains(9)) ? Visibility.Visible : Visibility.Hidden;
         }
      }

      public Visibility IsAnswerAvailable
      {
         get 
         {
            return (IsAnswerValid(_Answer) || Selected) ? Visibility.Visible : Visibility.Hidden;
         }
      }

      private void UpdatePossibleView()
      {
         OnPropertyChanged("IsOnePossible");
         OnPropertyChanged("IsTwoPossible");
         OnPropertyChanged("IsThreePossible");
         OnPropertyChanged("IsFourPossible");
         OnPropertyChanged("IsFivePossible");
         OnPropertyChanged("IsSixPossible");
         OnPropertyChanged("IsSevenPossible");
         OnPropertyChanged("IsEightPossible");
         OnPropertyChanged("IsNinePossible");
      }

      public bool Selected
      {
         get { return _Selected; }
         set
         {
            _Selected = value;
            UpdatePossibleView();
            OnPropertyChanged("IsAnswerAvailable");
            OnPropertyChanged("Selected");
         }
      }

      public int Answer
      {
         get { return _Answer; }
         set
         {
            if (IsAnswerValid(value))
            {
               _Answer = value;

               _PossibleAnswers.Clear();
               _PossibleAnswers.Add(value);

               UpdatePossibleView();
               OnPropertyChanged("IsAnswerAvailable");
               OnPropertyChanged("Answer");

               ReportSolved();
            }
         }
      }
   }
}
