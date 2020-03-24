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

            // You can scrcpy via Wifi-ADB 
            // By adding ex. "-s 175.20.3.45" in defaultArgs
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
                //Wait for the process to exit or time out.
                ScrpyProcess.WaitForExit(1000);
                //Check to see if the process is still running.
                if (ScrpyProcess.HasExited == false)
                    //Process is still running.
                    //Test to see if the process is hung up.
                    if (ScrpyProcess.Responding)
                    {
                        //Process was responding; close the main window.
                        ScrpyProcess.CloseMainWindow();
                    }
                    else
                    {
                        //Process was not responding; force the process to close.
                        ScrpyProcess.Kill();
                    }
            }
        }
                
        public bool CanStopScrcpy => !ScrpyProcess?.HasExited ?? true;
        public bool CanStartScrcpy => ScrpyProcess?.HasExited ?? true;

        public Process ScrpyProcess2
        {
            get => _scrpyProcess2;
            set
            {
                _scrpyProcess2 = value;
                NotifyOfPropertyChange(() => ScrpyProcess2);
            }
        }
        private bool _record2 = true;

        public bool Record2
        {
            get => _record2; set
            {
                _record2 = value;
                NotifyOfPropertyChange(() => Record2);
            }
        }

        private string _output2;

        public string Output2
        {
            get { return _output2; }
            set
            {
                _output2 = value;
                NotifyOfPropertyChange(() => Output2);
            }
        }

        private bool _isRunning2;
        private Process _scrpyProcess2;

        public bool IsRunning2
        {
            get => _isRunning2; set
            {
                _isRunning2 = value;
                NotifyOfPropertyChange(() => _isRunning2);
            }
        }

        public async void StartScrcpy2()
        {
            //ScrpyProcess2 = Process.Start("notepad.exe");
            //return;

            // You can scrcpy 2nd device even via Wifi-ADB 
            // By adding ex. "-s 175.20.3.45" in defaultArgs 
            // Or if you are connected via cable
            // add device id like "-s R27M10F1ZXP"
            var defaultArgs = "--bit-rate 16M --window-borderless";
            var args = defaultArgs;

            if (Record2)
            {
                args = @"--record c:\source\tools\testfile2.mp4 " + args;
            }

            var cPath = @"C:\source\tools\scrcpy2";
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
                process.OutputDataReceived += ScrpyProcessOnOutputDataReceived2;
                //Can't think of a better way to do this
                await Task.Delay(3000);
                //hread.Sleep(2000);
                /*while (process.MainWindowHandle == IntPtr.Zero)
                {
                    Thread.Sleep(200);
                    process.Refresh();
                }*/
                ScrpyProcess2 = process;
                NotifyOfPropertyChange(() => CanStopScrcpy2);
                NotifyOfPropertyChange(() => CanStartScrcpy2);
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
                ScrpyProcess2.Exited += ScrpyProcessOnExited2;
            }




            /*while (!_scrpyProcess.StandardOutput.EndOfStream)
            {
                var line = _scrpyProcess.StandardOutput.ReadLine();
                if (line != null) OutputTextBox.Text += line;
            }*/
        }

        private void ScrpyProcessOnExited2(object? sender, EventArgs e)
        {
            if (ScrpyProcess2 != null)
            {
                StopScrcpy2();
            }

            NotifyOfPropertyChange(() => CanStopScrcpy2);
            NotifyOfPropertyChange(() => CanStartScrcpy2);
        }

        private void ScrpyProcessOnOutputDataReceived2(object sender, DataReceivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Output += e.Data + "\r\n";
            });
        }

        public void StopScrcpy2()
        {
            if (ScrpyProcess2.HasExited == false)
            {
                //Wait for the process to exit or time out.
                ScrpyProcess2.WaitForExit(1000);
                //Check to see if the process is still running.
                if (ScrpyProcess2.HasExited == false)
                    //Process is still running.
                    //Test to see if the process is hung up.
                    if (ScrpyProcess2.Responding)
                    {
                        //Process was responding; close the main window.
                        ScrpyProcess2.CloseMainWindow();
                    }
                    else
                    {
                        //Process was not responding; force the process to close.
                        ScrpyProcess2.Kill();
                    }
            }
        }

        public bool CanStopScrcpy2 => !ScrpyProcess2?.HasExited ?? true;
        public bool CanStartScrcpy2 => ScrpyProcess2?.HasExited ?? true;
    }
}

