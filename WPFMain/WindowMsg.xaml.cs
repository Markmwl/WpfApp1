using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFMain
{
    /// <summary>
    /// WindowMsg.xaml 的交互逻辑
    /// </summary>
    public partial class WindowMsg : Base.BaseWindow
    {
        private static string Msg;
        private static string Caption;

        public WindowMsg(bool isInfo = false)
        {
            InitializeComponent();
            if (isInfo)
            {
                this.SureBtn.Visibility = Visibility.Hidden;
                this.CancelBtn.Content = "确定";
            }
        }

        /// <summary>
        /// 确认窗体
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="mes"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static bool? Show(FrameworkElement owner, string mes, string caption = "系统提示")
        {
            Msg = mes;
            Caption = caption;
            WindowMsg win = new WindowMsg();
            Window pwin = Window.GetWindow(owner);
            win.Owner = pwin;
            var loc = owner.PointToScreen(new Point());
            win.Left = loc.X + (owner.ActualWidth - win.Width) / 2;
            win.Top = loc.Y + (owner.ActualHeight - win.Height) / 2;
            //win.ShowInTaskbar = true;
            return win.ShowDialog();
        }

        /// <summary>
        /// 提示窗体
        /// </summary>
        public static void ShowInfo(FrameworkElement owner, string mes, string caption = "系统提示")
        {
            Msg = mes;
            Caption = caption;
            WindowMsg win = new WindowMsg(true);
            Window pwin = Window.GetWindow(owner);
            win.Owner = pwin;
            var loc = owner.PointToScreen(new Point());
            win.Left = loc.X + (owner.ActualWidth - win.Width) / 2;
            win.Top = loc.Y + (owner.ActualHeight - win.Height) / 2;
            //win.ShowInTaskbar = false;
            win.ShowDialog();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Text.Content =  Caption;
            Text.FontSize = 14;
            btnMin.Visibility = Visibility.Collapsed;
            btnMax.Visibility = Visibility.Collapsed;

            FooterGrid.Visibility = Visibility.Collapsed;

            this.ContentText.Text = Msg;
            this.Height = Grid_Content.ActualHeight;
        }

        private void SureBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
