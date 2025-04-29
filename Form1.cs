using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Diagnostics.Contracts;
using static ProductsWinForms.Form1;
using static ProductsWinForms.DllImports;
using System;

namespace ProductsWinForms
{
    public partial class Form1 : Form
    {
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        private System.Windows.Forms.Timer refreshTimer;

        /*
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public struct POINT
        {
            public int x;
            public int y;
        }

        public class Data
        {
            public IntPtr hWnd;
            public RECT rect;
            public Direction direction;
            public IntPtr bang;
            public int step;
        }

        public enum Direction
        {
            LeftUp = 1,
            LeftDown,
            RightUp,
            RightDown
        }


        static List<Data> hList = new List<Data>();
        static int high = 100;
        static int width = 100;
        static RECT rectScr;
        */

        static bool IsRunAsAdministrator()
        {
            var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static void RestartAsAdministrator()
        {
            try
            {
                string exePath = Environment.ProcessPath;

                var startInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Verb = "runas",
                    UseShellExecute = true
                };

                // запускаем новый процесс
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error when reloading application with administrator permissions: {ex.Message}");
            }
        }

        public Form1()
        {
            if (!IsRunAsAdministrator())
            {
                RestartAsAdministrator();
                Environment.Exit(0);
                return;
            }

            InitializeComponent();
            this.DoubleBuffered = true; // Enabling double buffering for the form
            EnableListViewDoubleBuffering(listView1); // Enabling double buffering for the ListView
            LoadProcesses();
            ListViewSettings();

            SetupAutoRefresh();
        }

        private void ListViewSettings()
        {
            listView1.FullRowSelect = true;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
        }

        private void SetupAutoRefresh()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 10000;
            refreshTimer.Tick += (s, e) => LoadProcesses();
            refreshTimer.Start();
        }


        private void LoadProcesses()
        {
            try
            {
                // Clearing ListView before adding new data
                listView1.Items.Clear();
                listView1.View = View.Details; // Setting data display in form of a table

                // Adding columns
                listView1.Columns.Clear();
                listView1.Columns.Add("ID", 50, HorizontalAlignment.Left); // 0
                listView1.Columns.Add("Name", 200, HorizontalAlignment.Left); // 1
                listView1.Columns.Add("Window title", 100, HorizontalAlignment.Right); // 2
                listView1.Columns.Add("Start time", 120, HorizontalAlignment.Right); // 3

                var processes = Process.GetProcesses().OrderBy(p => p.ProcessName);

                foreach (var process in processes)
                {
                    string windowTitle = string.IsNullOrWhiteSpace(process.MainWindowTitle) ? "N/A" : process.MainWindowTitle;
                    string startTime = "N/A";

                    // System Processes can refuse to give the information.
                    try
                    {
                        startTime = process.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch
                    {

                    }

                    // Создание строки (ListViewItem)
                    var item = new ListViewItem(process.Id.ToString()); // ID
                    item.SubItems.Add(process.ProcessName);             // Name
                    item.SubItems.Add(windowTitle);                     // Window title
                    item.SubItems.Add(startTime);                       // Start time

                    listView1.Items.Add(item); // Добавляем строку в ListView
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when loading data: " + ex.Message);
            }
        }

        // List item selected event handler
        private void listView1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // проверяем, что выбран элемент в списке
            if (listView1.SelectedItems.Count > 0)
            {
                try
                {
                    // Getting data from selected item
                    var selectedItem = listView1.SelectedItems[0];

                    var processId = listView1.SelectedItems[0].Text;
                    var processName = selectedItem.SubItems[1].Text;
                    var processWindowTitle = selectedItem.SubItems[2].Text;
                    var processStartTime = selectedItem.SubItems[3].Text;

                    Process selectedProcess = Process.GetProcessById(Int32.Parse(processId));
                    if (selectedProcess.HasExited)
                    {
                        return;
                    }

                    // Updating information in labels
                    label1.Text = "ID: " + processId;
                    label2.Text = "Name: " + processName;
                    label3.Text = "Window Title: " + processWindowTitle;
                    label4.Text = "Main Window Handle: " + selectedProcess.MainWindowHandle.ToString();
                    label5.Text = "Thread count: " + selectedProcess.Threads.Count.ToString();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Encountered error! {ex.Message}");
                }
            }
        }

        // для списка двойная буферизация включается только через рефлексию
        private void EnableListViewDoubleBuffering(ListView listView)
        {
            typeof(ListView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, listView, [true]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                // Due to settings, we can't choose multiple items from the list, so we are taking first one: [0]
                var selectedItem = listView1.SelectedItems[0];

                // Getting data from sub-elements
                var processId = listView1.SelectedItems[0].Text;
                var processName = selectedItem.SubItems[1].Text;
                var processWindowTitle = selectedItem.SubItems[2].Text;
                var processStartTime = selectedItem.SubItems[3].Text;

                Process selectedProcess = Process.GetProcessById(Int32.Parse(processId));
                if (selectedProcess.HasExited)
                {
                    return;
                }

                long bytes = selectedProcess.WorkingSet64;
                double mb = bytes / 1024.0 / 1024.0;
                double gb = bytes / 1024.0 / 1024.0 / 1024.0;

                // отображаем сообщение с информацией о выбранном продукте
                MessageBox.Show(
                    $"Process name: {selectedProcess.ProcessName} \n" +
                    $"PID: {selectedProcess.Id} \n" +
                    $"Window title: {selectedProcess.MainWindowTitle} \n" +
                    $"Start time: {selectedProcess.StartTime} \n" +
                    $"Window handle: {selectedProcess.MainWindowHandle} \n" +
                    $"Process native handle: {selectedProcess.Handle} \n" +
                    $"Process handle count: {selectedProcess.HandleCount} \n" +
                    $"Process memory: {mb:F2} MB ({gb:F2} GB) \n" +
                    $"Process priority: {selectedProcess.BasePriority} \n" +
                    $"Process threads: {selectedProcess.Threads} \n" +
                    $"Is running: {selectedProcess.HasExited} \n"
                );

                /*
                $"ProcessId: {processId}\n" +
                    $"Name: {processName}\n" +
                    $"WindowTitle: {processWindowTitle}\n" +
                    $"StartTime: {processStartTime}\n" +
                    $"Main Window Handle: " + selectedProcess.MainWindowHandle.ToString() +
                    $"Thread count: " + selectedProcess.Threads.Count.ToString()
                */
            }
            else
            {
                MessageBox.Show("First, select process from the list.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var selectedItem = listView1.SelectedItems[0];
                var processId = listView1.SelectedItems[0].Text;

                Process selectedProcess = Process.GetProcessById(Int32.Parse(processId));

                if (selectedProcess.HasExited)
                {
                    return;
                }

                bool result = DllImports.TerminateProcess(selectedProcess.Handle, 1);
                if (!result)
                {
                    MessageBox.Show($"Couldn't terminate process! Error code: {Marshal.GetLastWin32Error()}");
                    Debug.WriteLine($"Couldn't terminate process! Error code: {Marshal.GetLastWin32Error()}");
                }
            }
            else
            {
                MessageBox.Show("First, select process from the list.");
            }
        }
    }

    public static class DllImports
    {
        /*
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc enumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreatePolygonRgn(POINT[] lpPoints, int nCount, int fnPolyFillMode);

        [DllImport("gdi32.dll")]
        public static extern bool RectInRegion(IntPtr hRegion, ref RECT rect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string className, string? windowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string? lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint PostMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowText(IntPtr HWND, string Text);

        const uint WM_CHAR = 0x0102;
        */

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);
    }
}