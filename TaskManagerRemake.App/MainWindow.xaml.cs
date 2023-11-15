using System;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManagerRemake.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void CreateProcessesTab()
        {
            // ProcessTab processTab = new ProcessTab();
            // processTab.InitProcessesTab();
        }

        public void CreatePerformanceTab()
        {
            // PerformanceTab perfTab = new PerformanceTab();
            // perfTab.InitPerformanceTab();

            // perfTab.GetCurrentCpuUsage();
            // perfTab.GetAvailableRAM();
            // Thread.Sleep(100);
            // string cpuRes = perfTab.GetCurrentCpuUsage();
            // string ramRes = perfTab.GetAvailableRAM();
            // Debug.WriteLine($"CPU USAGE {cpuRes}, RAM available {ramRes}");
        }
    }
}
