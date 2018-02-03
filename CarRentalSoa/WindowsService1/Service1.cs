using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        // ServiceHost definition at a class level

        protected override void OnStart(string[] args)
        {

            //create service host
            //open the host
        }

        protected override void OnStop()
        {
            //close the host
        }
    }
}
