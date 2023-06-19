using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();
            string now = DateTime.Now.ToString("yyyyMMdd");
            txtVersion.Text = "当前版本：" + now.Substring(2, 2) + "." + now.Substring(4, 2) + "." + now.Substring(6, 2);
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            SwitchState();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if ((ResizeMode == ResizeMode.CanResize) ||
                    (ResizeMode == ResizeMode.CanResizeWithGrip))
                {
                    SwitchState();
                }
            }
            else
            {
                if (WindowState == WindowState.Maximized)
                {
                    restoreIfMove = true;
                }

                DragMove();
            }

        }

        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            //方式1：在拉拽完非最大化窗体后最大化拖动位置会有偏移
            //if (restoreIfMove)
            //{
            //    restoreIfMove = false;
            //    var mouseX = e.GetPosition(this).X;
            //    //先获取原来最小化时窗口的宽度
            //    var width = RestoreBounds.Width;
            //    //在拿现在的最大化时的宽度
            //    var maxwidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            //    //计算得出同比缩小后的鼠标x位置
            //    var x = (mouseX / maxwidth) * width;

            //    //控制左侧距离
            //    if (mouseX < (width / 2))
            //    {
            //        x = 0;
            //    }
            //    else  //控制右侧距离
            //    {
            //        x = x + (width / 6);
            //    }

            //    WindowState = WindowState.Normal;
            //    imgMax.Source = BitmapToBitmapImage(Properties.Resources.Maximize_1);
            //    Left = x;
            //    Top = 0;
            //    DragMove();
            //}

            //方式2：拉拽完非最大化窗体后不会偏移
            if (restoreIfMove)
            {
                restoreIfMove = false;
                //取当前鼠标指针位置
                var mouseX = e.GetPosition(this).X;
                //先获取原来最小化时窗口的宽度
                var width = RestoreBounds.Width;
                //在拿现在的最大化时的宽度
                //var maxwidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                var maxwidth = ActualWidth;
                //计算得出同比缩小后的鼠标x位置
                var x = (mouseX / maxwidth) * width;

                POINT lMousePosition;
                GetCursorPos(out lMousePosition);

                WindowState = WindowState.Normal;
                imgMax.Source = BitmapToBitmapImage(Properties.Resources.Maximize_1);
                Left = lMousePosition.X - x;
                Top = 0;
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }

            }

            //方式3：加高度
            //if (restoreIfMove)
            //{
            //    restoreIfMove = false;

            //    double percentHorizontal = e.GetPosition(this).X / ActualWidth;
            //    double targetHorizontal = RestoreBounds.Width * percentHorizontal;

            //    double percentVertical = e.GetPosition(this).Y / ActualHeight;
            //    double targetVertical = RestoreBounds.Height * percentVertical;

            //    WindowState = WindowState.Normal;

            //    POINT lMousePosition;
            //    GetCursorPos(out lMousePosition);

            //    Left = lMousePosition.X - targetHorizontal;
            //    Top = lMousePosition.Y - targetVertical;

            //    DragMove();
            //}


            //方式4：只要拖动鼠标就固定在窗体最中间
            //if (restoreIfMove)
            //{
            //    restoreIfMove = false;

            //    var point = PointToScreen(e.MouseDevice.GetPosition(this));

            //    Left = point.X - (RestoreBounds.Width * 0.5);
            //    Top = point.Y;

            //    WindowState = WindowState.Normal;

            //    DragMove();
            //}
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);


        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        bool restoreIfMove = false;

        private void MainGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            restoreIfMove = false;
        }

        private void SwitchState()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    {
                        WindowState = WindowState.Maximized;
                        imgMax.Source = BitmapToBitmapImage(Properties.Resources.Maximize_3);
                        MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                        break;
                    }
                case WindowState.Maximized:
                    {
                        WindowState = WindowState.Normal;
                        imgMax.Source = BitmapToBitmapImage(Properties.Resources.Maximize_1);
                        break;
                    }
            }
        }

        /// <summary>
        /// Bitmap转BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, bitmap.RawFormat);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.BorderThickness = new System.Windows.Thickness(7);
            }
            else
            {
                this.BorderThickness = new System.Windows.Thickness(0);
            }
        }
    }
}
