using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.ServiceProcess;
using System.Configuration.Install;
using System.Collections;
using System.Threading;

namespace GfanesServiceClient
{
    
    public partial class Form1 : Form
    {

        string serviceFilePath = $"{Application.StartupPath}\\MyWindowsService.exe";
        string serviceName = "MyService";


        public Form1()
        {
            InitializeComponent(); 
          
            lblTS.Text = getState();//获取当前服务状态


        }
        
        /// <summary>
        /// 获取当前服务状态
        /// </summary>
        /// <returns></returns>
        private string getState()
        {
            Thread.Sleep(200);
            string ts="服务未安装";
            if (this.IsServiceExisted(serviceName))
            {
                ts = "服务已安装";

                using (ServiceController control = new ServiceController(serviceName))
                {
                    if (control.Status == ServiceControllerStatus.Running)
                    {
                        ts = "服务已运行";
                    }
                    else
                    {
                        ts = "服务已停止";
                    }
                }



            }
            return ts;
        }

        #region 窗体点击事件
        /// <summary>
        /// 点击“安装服务”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName))
            {
                this.UninstallService(serviceFilePath);
            }
            this.InstallService(serviceFilePath);
            lblTS.Text = getState();
        }
        /// <summary>
        /// 点击“启动服务”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName))
            {
                this.ServiceStart(serviceName);
            }
            lblTS.Text = getState();
        }


        /// <summary>
        /// 点击“停止服务”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName))
            {
                this.ServiceStop(serviceName);
            }
            lblTS.Text = getState();
        }


        /// <summary>
        /// 点击“卸载服务”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName))
            {
                this.ServiceStop(serviceName);
                this.UninstallService(serviceFilePath);
            }
            lblTS.Text = getState();
        }
        #endregion



        #region 操作服务方法
        /// <summary>
        /// 判断服务是否存在
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private bool IsServiceExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController sc in services)
            {
                if (sc.ServiceName.ToLower() == serviceName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// 安装服务
        /// </summary>
        /// <param name="serviceFilePath"></param>
        private void InstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
               
                IDictionary savedState = new Hashtable();
                installer.Install(savedState);
                installer.Commit(savedState);

            }
        }


        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="serviceFilePatch"></param>
        private void UninstallService(string serviceFilePatch)
        {
            using (AssemblyInstaller install = new AssemblyInstaller())
            {
                install.UseNewContext = true;
                install.Path = serviceFilePatch;
                install.Uninstall(null);
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceName"></param>
        private void ServiceStart(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Stopped)
                {
                    control.Start();
                }
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceName"></param>
        private void ServiceStop(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Running)
                {
                    control.Stop();
                }
            }
        }




        #endregion

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName))//判断是否已经存在服务
            {
                //已经存在这个服务了
                this.ServiceStop(serviceName);//如果该服务正在运行则停止
                this.UninstallService(serviceFilePath);//卸载该服务
            }
            this.InstallService(serviceFilePath);//重新安装该服务
            this.ServiceStart(serviceName);//启动服务
        
            lblTS.Text = getState(); 
        }
    }


    
}
