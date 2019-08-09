using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Timers;
using System.IO;

namespace NotRespondingKiller
{
    public partial class Form1 : Form
    {
        string name;
        string path;
        int kontrol_sn = 3000;
        int bekleme_sn = 1000;
        public Form1()
        {
            InitializeComponent();
        }
        void timer_start()
        {
            timer1.Interval = kontrol_sn;
            timer1.Enabled = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Programın Adı", 150);
            listView1.Columns.Add("Path", 300);
            listView2.View = View.Details;
            listView2.FullRowSelect = true;
            listView2.Columns.Add("Name", 150);
            listView2.Columns.Add("Path", 150);
            listView2.Columns.Add("Time", 150);
            timer_start();            
        }
        
        private void Button1_Click(object sender, EventArgs e)
        {
            string[] arr = new string[4];
            ListViewItem itm;
            name = textBox1.Text;
            path = textBox2.Text+ "\\"+ name;
            arr[0] = name;
            arr[1] =  path;            
            itm = new ListViewItem(arr);
            listView1.Items.Add(itm);
            StreamWriter sw =File.AppendText("C:\\kayıt.txt");
            sw.WriteLine(arr[0] + " "+arr[1] + " Kontrol mSn: "+ kontrol_sn+" Bekleme mS "+bekleme_sn);            
            sw.Flush();
            sw.Close();           
        }
        
        private void Timer1_Tick(object sender, EventArgs e)
        {
           
            List<Process> prs= GetProcessByFilename(path);                               
            string[] arr = new string[3];
            foreach (Process pr in prs)
            {
                ListViewItem itm;
                itm = new ListViewItem(pr.ProcessName.ToLower());                              
                if (!pr.Responding)
                {
                    pr.Kill();                  
                    arr[0] = name;
                    arr[1] = path;
                    arr[2] = DateTime.Now.ToString()+"kapandı.";                                        
                    itm = new ListViewItem(arr);
                    listView2.Items.Add(itm);               
                    islem(DateTime.Now);
                    timer1.Stop();
                    Thread.Sleep(bekleme_sn);
                    start();
                    arr[0] = name;
                    arr[1] = path;
                    arr[2] = DateTime.Now.ToString() + "açıldı.";
                    itm = new ListViewItem(arr);
                    listView2.Items.Add(itm);
                    timer1.Start();                
                }
            }
        }
        void start()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = name;
            //startInfo.Arguments = file;
            Process.Start(startInfo);
        }
        private static List<Process> GetProcessByFilename(string filename)
        {
            List<Process> processes = new List<Process>();
            processes= Process.GetProcesses().Where(process => !string.IsNullOrEmpty(process.MainWindowTitle) &&
                process.MainModule.FileName == filename).ToList();
           
            return processes;
        }
        void islem(DateTime aa)
        {
            string[] arr = new string[4];
            ListViewItem itm;
            arr[0] = name;
            arr[1] = DateTime.Now.ToString();
            itm = new ListViewItem(arr);
            listView2.Items.Add(itm);
        }    
        private void Button2_Click(object sender, EventArgs e)
        {           
            if (textBox3.Text != "")
            {
                kontrol_sn = Convert.ToInt32(textBox3.Text)*1000;
            }
            if (textBox4.Text != "")
            {
                bekleme_sn = Convert.ToInt32(textBox4.Text)*1000;
            }
            timer_start();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listView1.SelectedItems)
            {
                item.Remove();
            }
        }
    }
}
