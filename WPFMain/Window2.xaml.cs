using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Base.BaseWindow
    {
        public Window2()
        {
            InitializeComponent();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Text.Content = "Demo-Window2";
        }

        private void btnDemo_Click(object sender, RoutedEventArgs e)
        {
            //ReturnObject loadReturnObject = WaitingWindow.Show("正在加载", aBO, "GetA", new Object[] { "", "", "", "", m_IteratorValues, false }) as ReturnObject;

            IList<Dto> dt = new List<Dto>();

            //new Wait.WindowWait(() =>
            //{

            //    Debug.WriteLine("开始：" + DateTime.Now);
            //    //多线程模拟
            //    dt = GetThreadData();

            //    Debug.WriteLine("结束：" + DateTime.Now);

            //    //System.Threading.Thread.Sleep(3000);

            //    Dispatcher.BeginInvoke(new Action(delegate
            //    {
            //        dtMain.ItemsSource = dt;
            //    }));

            //}).ShowDialog();

            //Wait.WindowWait.Show(this, () =>
            //{
            //    Debug.WriteLine("开始：" + DateTime.Now);
            //    //多线程模拟
            //    dt = GetThreadData();

            //    //同步
            //    //dt = GetSyncData();

            //    Debug.WriteLine("结束：" + DateTime.Now);

            //    System.Threading.Thread.Sleep(3000);

            //    Dispatcher.BeginInvoke(new Action(delegate
            //    {
            //        dtMain.ItemsSource = dt;
            //    }));

            //}, "请稍后");

            Wait.WindowWait1.Show(this, () =>
            {
                Debug.WriteLine("开始：" + DateTime.Now);
                //多线程模拟
                dt = GetThreadData();

                //同步
                //dt = GetSyncData();

                Debug.WriteLine("结束：" + DateTime.Now);

                System.Threading.Thread.Sleep(10000);

                Dispatcher.BeginInvoke(new Action(delegate
                {
                    dtMain.ItemsSource = dt;
                }));

            }, "数据刷新中");


        }

        /// <summary>
        /// 同步
        /// </summary>
        /// <returns></returns>
        public IList<Dto> GetSyncData()
        {
            IList<Dto> dt = new List<Dto>();
            for (int i = 0; i < 100; i++)
            {
                var trd = GetData(i);
                dt.Add(trd);
            }
            return dt;
        }

        /// <summary>
        /// 多线程模拟
        /// </summary>
        /// <returns></returns>
        public IList<Dto> GetThreadData()
        {
            IList<Dto> dt = new List<Dto>();

            Task[] tasks = new Task[100];
            Task task;
            Debug.WriteLine("模仿" + tasks.Length + "次数据接口请求开始：" + DateTime.Now);
            for (int i = 0; i < tasks.Length; i++)
            {
                task = new Task(o =>
                {
                    var trd = GetData(Convert.ToInt32(o));
                    Debug.WriteLine($"子线程ID：{Thread.CurrentThread.ManagedThreadId}");
                    dt.Add(trd);
                }, i);
                tasks[i] = task;
                tasks[i].Start();
            }
            Task.WaitAll(tasks);
            Debug.WriteLine($"主线程ID：{Thread.CurrentThread.ManagedThreadId}");
            Debug.WriteLine("模仿" + tasks.Length + "次数据接口请求结束：" + DateTime.Now);
            return dt;
        }

        public Dto GetData(int id)
        {
            var dto = new Dto();
            dto.ID = id + 1;

            dto.Name = new List<string>() { "张三", "李四", "王五", "赵六", "刘七", "马八" }.OrderBy(u => Guid.NewGuid()).First() + (id + 1).ToString();

            Thread.Sleep(100);
            dto.Age = new Random().Next(16, 40);

            dto.Sex = new List<string>() { "男", "女" }.OrderBy(u => Guid.NewGuid()).First();

            dto.Money = new Random().Next(0, 10000);

            dto.Email = new List<string>() { "528414865@qq.com", "123456@qq.com", "mwl_mark@qq.com", "77777@qq.com", "qwerto@qq.com" }.OrderBy(u => Guid.NewGuid()).First();

            dto.QQ = new List<string>() { "528414865", "123456", "7777777", "8715373" }.OrderBy(u => Guid.NewGuid()).First();

            dto.WeiXin = new List<string>() { "ma528414865", "zhang1111111", "wang7777777", "li123456" }.OrderBy(u => Guid.NewGuid()).First();

            dto.Address = new List<string>() { "上海-浦东", "陕西-西安", "湖北-武汉" }.OrderBy(u => Guid.NewGuid()).First();

            dto.Phone = new List<string>() { "13189016728", "17612475521", "13811381627", "16601826471" }.OrderBy(u => Guid.NewGuid()).First();

            dto.Dept = new List<string>() { "信息技术部", "运维部", "运营部", "营销部" }.OrderBy(u => Guid.NewGuid()).First();

            dto.Hobby = new List<string>() { "Game", "tomcat", "baidu", "google" }.OrderBy(u => Guid.NewGuid()).First();

            dto.Accout = new List<string>() { "528414865", "7777777", "zhangsan", "lisi", "wangwu" }.OrderBy(u => Guid.NewGuid()).First();

            dto.Password = "******";

            return dto;
        }

        private void btnMsg_Click(object sender, RoutedEventArgs e)
        {
            //decimal a = 1704048728;
            //decimal banlance = Math.Abs( a / 100000000);
            //Debug.WriteLine(banlance);
            //decimal banlance1 = Math.Ceiling(banlance*100)/100;
            //Debug.WriteLine(banlance1);

            //decimal[] values = { 7.037817862631m, 7.6413261536815m, 0.121213m, -0.123213m, -7.1m, -7.6m, 6.0000m,-6.0000m };
            //Debug.WriteLine("  Value          Ceiling          Floor\n");
            //foreach (decimal value in values)
            //    Debug.WriteLine("{0,7} {1,16} {2,14}",
            //                      value, Math.Ceiling(value), Math.Floor(value));
            //return;

            string msg = "这是一个非常严重的误操作，继续运行将产生未知错误!" + "\r\n" + "要获取详细信息请与管理员联系。" + "\r\n" + "错误码：001" + "\r\n" + "是否关闭该程序？";
            if (WindowMsg.Show(this, msg, "系统提示") == true)
            {
                 WindowMsg.ShowInfo(this,"true", "系统提示");
            }
            else
            {
                WindowMsg.ShowInfo(this, "false", "系统提示");
            }

        }
    }

    public class Dto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public decimal Money { get; set; }
        public string Email { get; set; }
        public string QQ { get; set; }
        public string WeiXin { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Dept { get; set; }
        public string Hobby { get; set; }
        public string Accout { get; set; }
        public string Password { get; set; }
    }
}
