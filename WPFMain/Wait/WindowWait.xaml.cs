using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFMain.Wait
{
    /// <summary>
    /// WindowWait.xaml 的交互逻辑
    /// </summary>
    public partial class WindowWait : Window
    {
        public Action Act;

        public WindowWait(Action act)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Act = act;
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Act.BeginInvoke(this.OnComplate, null);
        }

        private void OnComplate(IAsyncResult ar)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.Close();
            }));
        }

        /// <summary>
        /// 显示等待框，owner指定宿主视图元素，callback为需要执行的方法体（需要自己做异常处理）。
        /// 目前等等框为模式窗体
        /// </summary>
        public static void Show(FrameworkElement owner, Action callback, string mes = "有一种幸福，叫做等待...")
        {
            WindowWait win = new WindowWait(callback);
            Window pwin = Window.GetWindow(owner);
            win.Owner = pwin;
            //win.Text = mes;
            var loc = owner.PointToScreen(new Point());
            win.Left = loc.X + (owner.ActualWidth - win.Width) / 2;
            win.Top = loc.Y + (owner.ActualHeight - win.Height) / 2;
            win.ShowInTaskbar = false;
            win.ShowDialog();
        }


        private void Window_ContentRendered(object sender, EventArgs e)
        {
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            
        }
    }
}
