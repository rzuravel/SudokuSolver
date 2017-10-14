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
   /// Interaction logic for CellView.xaml
   /// </summary>
   public partial class CellView : UserControl
   {
      CellHashset _Cell;

      public CellView(CellHashset InCell)
      {
         _Cell = InCell;

         DataContext = _Cell;

         InitializeComponent();
      }

      private void Label_MouseUp(object sender, MouseButtonEventArgs e)
      {
         //CellBorder.BorderBrush = new SolidColorBrush(Colors.CornflowerBlue);
         //CellBorder.BorderThickness = new Thickness(3);

         _Cell.OnMouseDown();
      }

      //private void CellBorder_KeyDown(object sender, KeyEventArgs e)
      //{
      //   _Cell.KeyDown(e);
      //}

      //private void CellBorder_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
      //{
      //   int i = 0;
      //   i++;
      //}
   }
}
