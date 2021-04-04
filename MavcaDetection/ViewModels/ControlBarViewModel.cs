using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MavcaDetection.ViewModels
{
    public class ControlBarViewModel : BaseViewModel
    {
        #region Commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseDownWindowCommand { get; set; }
        public ICommand MoveBackCommand { get; set; }

        #endregion
        public string Title { get; set; }

        public ControlBarViewModel()
        {
            CloseWindowCommand = new RelayCommand<UserControl>(p => { return p == null ? false : true; },
                p => {
                    Window.GetWindow(p).Close();
                });
            MaximizeWindowCommand = new RelayCommand<UserControl>(p => { return p == null ? false : true; },
                p => {
                    var window = Window.GetWindow(p);
                    if (window.WindowState != WindowState.Maximized)
                    {
                        window.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        window.WindowState = WindowState.Normal;
                    }
                });
            MinimizeWindowCommand = new RelayCommand<UserControl>(p => { return p == null ? false : true; },
                p => {
                    var window = Window.GetWindow(p);
                    if (window.WindowState != WindowState.Minimized)
                    {
                        window.WindowState = WindowState.Minimized;
                    }
                    else
                    {
                        window.WindowState = WindowState.Normal;
                    }
                });

            MouseDownWindowCommand = new RelayCommand<UserControl>(p => { return p == null ? false : true; },
               p => {
                   var window = Window.GetWindow(p);
                   if (window != null)
                   {
                       window.DragMove();
                   }
               });
            //MoveBackCommand = new RelayCommand<UserControl>(p => { return p == null ? false : true; },
            //   p => {
            //       Window window = Window.GetWindow(p);
            //       if (window != null)
            //       {
            //           window.SetPage(new MainWindow());
            //       }
            //   });
        }
    }
}
