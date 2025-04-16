using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Presentation.ViewModel;

namespace PresentationView
{
    /// <summary>
    /// View implementation
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //double tableWidth = TableBorder.ActualWidth;
            //double tableHeight = TableBorder.ActualHeight;
            //double diameter = 50.0;

            //if (DataContext is MainWindowViewModel vm)
            //{
            //    vm.InitializeTableSettings(tableWidth, tableHeight, diameter);
            //}

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (TableBorder != null)
            {
                double newWidth = TableBorder.ActualWidth;
                double newHeight = TableBorder.ActualHeight;
                double diameter = 50.0;

                ViewModel.InitializeTableSettings(newWidth, newHeight, diameter);
            }
        }


        /// <summary>
        /// Raises the <seealso cref="System.Windows.Window.Closed"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
                viewModel.Dispose();
            base.OnClosed(e);
        }
    }
}