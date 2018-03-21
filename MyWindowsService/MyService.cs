using System;
using System.ServiceProcess;
using System.IO;
using System.Timers;

namespace MyWindowsService
{
    public partial class MyService : ServiceBase
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        static string filePath = @"D:\MyServiceLog.txt";
        static bool isOpen = false;
        static bool theThreadIsRunning = false;

        public MyService()
        {
            InitializeComponent();
       
        }
      

        protected override void OnStart(string[] args)
        {
            log("服务启动");
            StartTheUpdateTask();
        }

        protected override void OnStop()
        {
            log("服务停止");
            //EndUpdateTask();

        }



       /// <summary>
       /// 开启定时任务
       /// </summary>
        private void StartTheUpdateTask()
        {
            log("启动计划任务：开始");
            timer.Enabled = true;
            timer.Interval = 2 * 1000; //执行间隔时间,单位为毫秒; 这里实际间隔为10分钟  
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimeArrival);
            isOpen = true;
            log("启动计划任务：成功");

        }

        /// <summary>
        /// 结束定时任务
        /// </summary>
        private void EndUpdateTask()
        {
            log("计划任务：结束");
            isOpen = false;
            timer.Stop();
            timer.Close();

        }


        /// <summary>
        /// 这里写要执行的任务
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static  void TimeArrival(object source, ElapsedEventArgs e)
        {
            if(!isOpen)
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Append))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now}," + "执行计划任务：取消，因为服务即将关闭");
            
                }
              
                return;
            }

            
            if(theThreadIsRunning)//防止上一次的任务执行时间太长，导致与这一次任务冲突
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Append))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now}," + "执行计划任务：取消，因为上一次任务还没执行完毕");

                }
     
                return;
            }

           

            theThreadIsRunning = true;
            #region 这里是执行定期任务代码
     
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{DateTime.Now}," + "执行计划任务：开始");

            }

            /////////////////////////代码开始
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{DateTime.Now},到计时器执行方法，更新数据库！");
            }


            //////////////////////////代码结束
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{DateTime.Now}," + "执行计划任务：结束");

            }
        
            #endregion
            theThreadIsRunning = false; 
      

        }



        private void log(string message)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{DateTime.Now},"+ message); 
            }
        }

    }
}
