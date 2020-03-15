using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;

namespace scrcpy_ui.ViewModels
{
    public class ShellViewModel : Screen
    {
        public ShellViewModel()
        {

        }       

        public Process ScrpyProcess
        {
            get => _scrpyProcess;
            set
            {
                _scrpyProcess = value;
                NotifyOfPropertyChange(() => ScrpyProcess);
            }
        }
        private bool _record = true;

        public bool Record
        {
            get => _record; set
            {
                _record = value;
                NotifyOfPropertyChange(() => Record);
            }
        }

        private string _output;

        public string Output
        {
            get { return _output; }
            set
            {
                _output = value;
                NotifyOfPropertyChange(() => Output);
            }
        }

        private bool _isRunning;
        private Process _scrpyProcess;

        public bool IsRunning
        {
            get => _isRunning; set
            {
                _isRunning = value;
                NotifyOfPropertyChange(() => _isRunning);
            }
        }



        public async void StartScrcpy()
        {
            //ScrpyProcess = Process.Start("notepad.exe");
            //return;
            var defaultArgs = "--bit-rate 16M --window-borderless";
            var args = defaultArgs;

            if (Record)
            {
                args = @"--record c:\source\tools\testfile.mp4 " + args;
            }

            var cPath = @"C:\source\tools\scrcpy";
            string filename = Path.Combine(cPath, "scrcpy.exe");

            var startInfo = new ProcessStartInfo()
            {
                FileName = filename,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true

            };

            try
            {
                var process = new Process()
                {
                    StartInfo = startInfo,
                    EnableRaisingEvents = true
                };
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.OutputDataReceived += ScrpyProcessOnOutputDataReceived;
                //Can't think of a better way to do this
                await Task.Delay(3000);
                //hread.Sleep(2000);
                /*while (process.MainWindowHandle == IntPtr.Zero)
                {
                    Thread.Sleep(200);
                    process.Refresh();
                }*/
                ScrpyProcess = process;
                NotifyOfPropertyChange(() => CanStopScrcpy);
                NotifyOfPropertyChange(() => CanStartScrcpy);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            finally
            {
                //StartScrcpy.IsEnabled = false;
                //StopScrcpy.IsEnabled = true;
                ScrpyProcess.Exited += ScrpyProcessOnExited;
            }

            /*while (!_scrpyProcess.StandardOutput.EndOfStream)
            {
                var line = _scrpyProcess.StandardOutput.ReadLine();
                if (line != null) OutputTextBox.Text += line;
            }*/
        }

        private void ScrpyProcessOnExited(object? sender, EventArgs e)
        {
            if (ScrpyProcess != null)
            {
                StopScrcpy();
            }

            NotifyOfPropertyChange(() => CanStopScrcpy);
            NotifyOfPropertyChange(() => CanStartScrcpy);
        }

        private void ScrpyProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Output += e.Data + "\r\n";
            });
        }

        public void StopScrcpy()
        {
            if (ScrpyProcess.HasExited == false)
            {
                ScrpyProcess.CloseMainWindow();
                //TODO: Handle timeout
                ScrpyProcess.WaitForExit(10000);
            }
        }
                
        public bool CanStopScrcpy => !ScrpyProcess?.HasExited ?? true;
        public bool CanStartScrcpy => ScrpyProcess?.HasExited ?? true;
    }
}
