using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WPFMain.Base
{
    public partial class BaseWindow:Window
    {
        private Image imgMax;

        /// <summary>
        /// 窗体提示
        /// </summary>
        public Label Text { get; set; }

        /// <summary>
        /// 最小化
        /// </summary>
        public Button btnMin { get; set; }

        /// <summary>
        /// 最大化
        /// </summary>
        public Button btnMax { get; set; }

        /// <summary>
        /// 顶部边框
        /// </summary>
        public Border MenuGrid { get; set; }

        /// <summary>
        /// 底部边框
        /// </summary>
        public Border FooterGrid { get; set; }

        public BaseWindow()
        {
            InitializeStyle();
            this.Loaded += delegate
            {
                InitializeEvent();
            };
        }

        private void InitializeStyle()
        {
            this.Style = (Style)App.Current.Resources["BaseWindowStyle"];
        }

        private void InitializeEvent()
        {
            SizeChanged += Window_SizeChanged;

            ControlTemplate baseWindowTemplate = (ControlTemplate)App.Current.Resources["BaseWindowControlTemplate"];

            Button minBtn = (Button)baseWindowTemplate.FindName("btnMin", this);
            minBtn.Click += btnMin_Click;
            btnMin = minBtn;

            Button maxBtn = (Button)baseWindowTemplate.FindName("btnMax", this);
            maxBtn.Click += btnMax_Click;
            btnMax = maxBtn;

            Button closeBtn = (Button)baseWindowTemplate.FindName("btnClose", this);
            closeBtn.Click += btnClose_Click;

            //顶部边框
            MenuGrid = (Border)baseWindowTemplate.FindName("MenuGrid", this);
            MenuGrid.MouseLeftButtonDown += MenuGrid_MouseLeftButtonDown;
            MenuGrid.MouseMove += MenuGrid_MouseMove;
            MenuGrid.MouseLeftButtonUp += MenuGrid_MouseLeftButtonUp;

            imgMax = (Image)baseWindowTemplate.FindName("imgMax", this);

            Label lb = (Label)baseWindowTemplate.FindName("Demo", this);
            Text = lb;

            TextBlock txtVersion = (TextBlock)baseWindowTemplate.FindName("txtVersion", this); 
            string now = DateTime.Now.ToString("yyyyMMdd");
            txtVersion.Text = "当前版本：" + now.Substring(2, 2) + "." + now.Substring(4, 2) + "." + now.Substring(6, 2);



            //底部边框
            FooterGrid = (Border)baseWindowTemplate.FindName("FooterGrid", this);
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
            if (WindowMsg.Show(this, "确认关闭系统？", "系统提示") == true)
            {
                this.Close();
            }
        }

        public void Window_SizeChanged(object sender, SizeChangedEventArgs e)
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



        private void MenuGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void MenuGrid_MouseMove(object sender, MouseEventArgs e)
        {
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

        private void MenuGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
    }
}
