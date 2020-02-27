using IdentiGo.Transversal.IoC;
using IdentiGo.Transversal.Utilities;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FTPService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            FileUtil.SendFileFTP();
            //Stop();
        }

        protected override void OnStop()
        {
        }
    }
}
