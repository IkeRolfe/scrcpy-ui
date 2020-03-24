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

        public Process ScrcpyProcess
        {
            get => _scrcpyProcess;
            set
            {
                _scrcpyProcess = value;
                NotifyOfPropertyChange(() => ScrcpyProcess);
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
        private Process _scrcpyProcess;

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
            //ScrcpyProcess = Process.Start("notepad.exe");
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
                process.OutputDataReceived += ScrcpyProcessOnOutputDataReceived;
                //Can't think of a better way to do this
                await Task.Delay(3000);
                //hread.Sleep(2000);
                /*while (process.MainWindowHandle == IntPtr.Zero)
                {
                    Thread.Sleep(200);
                    process.Refresh();
                }*/
                ScrcpyProcess = process;
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
                ScrcpyProcess.Exited += ScrcpyProcessOnExited;
            }

            /*while (!_scrcpyProcess.StandardOutput.EndOfStream)
            {
                var line = _scrcpyProcess.StandardOutput.ReadLine();
                if (line != null) OutputTextBox.Text += line;
            }*/
        }

        private void ScrcpyProcessOnExited(object? sender, EventArgs e)
        {
            if (ScrcpyProcess != null)
            {
                StopScrcpy();
            }

            NotifyOfPropertyChange(() => CanStopScrcpy);
            NotifyOfPropertyChange(() => CanStartScrcpy);
        }

        private void ScrcpyProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Output += e.Data + "\r\n";
            });
        }

        public void StopScrcpy()
        {
            if (ScrcpyProcess.HasExited == false)
            {
                //Wait for the process to exit or time out.
                ScrcpyProcess.WaitForExit(1000);
                //Check to see if the process is still running.
                if (ScrcpyProcess.HasExited == false)
                    //Process is still running.
                    //Test to see if the process is hung up.
                    if (ScrcpyProcess.Responding)
                    {
                        //Process was responding; close the main window.
                        ScrcpyProcess.CloseMainWindow();
                    }
                    else
                    {
                        //Process was not responding; force the process to close.
                        ScrcpyProcess.Kill();
                    }
            }
        }
                
        public bool CanStopScrcpy => !ScrcpyProcess?.HasExited ?? true;
        public bool CanStartScrcpy => ScrcpyProcess?.HasExited ?? true;

        public Process ScrcpyProcess2
        {
            get => _scrcpyProcess2;
            set
            {
                _scrcpyProcess2 = value;
                NotifyOfPropertyChange(() => ScrcpyProcess2);
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
        private Process _scrcpyProcess2;

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
            //ScrcpyProcess2 = Process.Start("notepad.exe");
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
                process.OutputDataReceived += ScrcpyProcessOnOutputDataReceived2;
                //Can't think of a better way to do this
                await Task.Delay(3000);
                //hread.Sleep(2000);
                /*while (process.MainWindowHandle == IntPtr.Zero)
                {
                    Thread.Sleep(200);
                    process.Refresh();
                }*/
                ScrcpyProcess2 = process;
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
                ScrcpyProcess2.Exited += ScrcpyProcessOnExited2;
            }




            /*while (!_scrcpyProcess.StandardOutput.EndOfStream)
            {
                var line = _scrcpyProcess.StandardOutput.ReadLine();
                if (line != null) OutputTextBox.Text += line;
            }*/
        }

        private void ScrcpyProcessOnExited2(object? sender, EventArgs e)
        {
            if (ScrcpyProcess2 != null)
            {
                StopScrcpy2();
            }

            NotifyOfPropertyChange(() => CanStopScrcpy2);
            NotifyOfPropertyChange(() => CanStartScrcpy2);
        }

        private void ScrcpyProcessOnOutputDataReceived2(object sender, DataReceivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Output += e.Data + "\r\n";
            });
        }

        public void StopScrcpy2()
        {
            if (ScrcpyProcess2.HasExited == false)
            {
                //Wait for the process to exit or time out.
                ScrcpyProcess2.WaitForExit(1000);
                //Check to see if the process is still running.
                if (ScrcpyProcess2.HasExited == false)
                    //Process is still running.
                    //Test to see if the process is hung up.
                    if (ScrcpyProcess2.Responding)
                    {
                        //Process was responding; close the main window.
                        ScrcpyProcess2.CloseMainWindow();
                    }
                    else
                    {
                        //Process was not responding; force the process to close.
                        ScrcpyProcess2.Kill();
                    }
            }
        }

        public bool CanStopScrcpy2 => !ScrcpyProcess2?.HasExited ?? true;
        public bool CanStartScrcpy2 => ScrcpyProcess2?.HasExited ?? true;
    }
}

