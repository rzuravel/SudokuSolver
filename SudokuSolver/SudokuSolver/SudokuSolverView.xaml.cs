using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SudokuSolver
{
   /// <summary>
   /// Interaction logic for SudokuSolverView.xaml
   /// </summary>
   public partial class SudokuSolverView : Window
   {
      SudokuSolverVM _VM;

      public SudokuSolverView()
      {
         _VM = new SudokuSolverVM();

         DataContext = _VM;

         InitializeComponent();

         for(int i = 0; i < SudokuSolverVM.ROWS; i++)
         {
            SudokuGameboard.RowDefinitions.Add(new RowDefinition());
         }

         for (int i = 0; i < SudokuSolverVM.COLS; i++)
         {
            SudokuGameboard.ColumnDefinitions.Add(new ColumnDefinition());
         }

         for (int i = 0; i < SudokuSolverVM.TOTAL_CELLS; i++)
         {
            CellView Cell = new CellView(_VM.Cells[i]);

            Grid.SetRow(Cell, (int)Math.Floor((double)(i / SudokuSolverVM.COLS)));
            Grid.SetColumn(Cell, (i % SudokuSolverVM.COLS));

            SudokuGameboard.Children.Add(Cell);

            // start of a quadrant
            if (_VM.FindTheStartOfIndexQuadrant(i) == i)
            {
               Border QuadBorder = new Border();
               Grid.SetRow(QuadBorder, (int)Math.Floor((double)(i / SudokuSolverVM.COLS)));
               Grid.SetColumn(QuadBorder, (i % SudokuSolverVM.COLS));

               Grid.SetRowSpan(QuadBorder, SudokuSolverVM.QUAD_ROWS);
               Grid.SetColumnSpan(QuadBorder, SudokuSolverVM.QUAD_COLS);

               QuadBorder.BorderThickness = new Thickness(3);
               QuadBorder.BorderBrush = new SolidColorBrush(Colors.White);

               SudokuGameboard.Children.Add(QuadBorder);
            }
         }
      }

      private void Grid_KeyDown(object sender, KeyEventArgs e)
      {
         _VM.HandleKeyDown(e);
      }

      //private void SudokuGameboard_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
      //{
      //   int i = 0;
      //   i++;
      //}
   }
}
