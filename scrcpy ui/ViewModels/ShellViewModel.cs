using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace scrcpy_ui.ViewModels
{
    public class ShellViewModel : INotifyPropertyChanged
    {
        public ShellViewModel()
        {

        }

        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region INotifyPropertyChanged Methods

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public Process ScrcpyProcess
        {
            get => _scrcpyProcess;
            set
            {
                _scrcpyProcess = value;
                RaisePropertyChanged("ScrcpyProcess");
            }
        }
        private bool _record = true;

        public bool Record
        {
            get => _record; set
            {
                _record = value;
                RaisePropertyChanged("Record");
            }
        }

        private string _output;

        public string Output
        {
            get { return _output; }
            set
            {
                _output = value;
                RaisePropertyChanged("Output");
            }
        }

        private bool _isRunning;
        private Process _scrcpyProcess;

        public bool IsRunning
        {
            get => _isRunning; set
            {
                _isRunning = value;
                 RaisePropertyChanged("_isRunning");
            }
        }

        private ICommand startScrcpyCommand;
        public ICommand StartScrcpyCommand
        {
            get { return (this.startScrcpyCommand) ?? (this.startScrcpyCommand = new DelegateCommand(StartScrcpy)); }
        }

        bool isProcRunSuccess;
        public async void StartScrcpy()
        {
            //ScrcpyProcess = Process.Start("notepad.exe");
            //return;

            // You can scrcpy via Wifi-ADB 
            // By adding ex. "-s 175.20.3.45" in defaultArgs
            var defaultArgs = "--bit-rate 16M --window-borderless";
            var args = defaultArgs;

            isProcRunSuccess = false;

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
                if (isProcRunSuccess)
                {
                    ScrcpyProcess = process;
                    if (ScrcpyProcess == null)
                    {
                        Debug.WriteLine("Error : scrcpyProcess is null");
                    }
                    else
                    {
                        RaisePropertyChanged("CanStopScrcpy");
                        RaisePropertyChanged("CanStartScrcpy");
                    }
                }
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
                if (isProcRunSuccess)
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

            RaisePropertyChanged("CanStopScrcpy");
            RaisePropertyChanged("CanStartScrcpy");
        }

        private void ScrcpyProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Output += e.Data + "\r\n";
            });
            if (e.Data != null && !e.Data.Contains("error"))
            {
                isProcRunSuccess = true;

                RaisePropertyChanged("CanStopScrcpy");
                RaisePropertyChanged("CanStartScrcpy");
            }
        }

        private ICommand stopScrcpyCommand;
        public ICommand StopScrcpyCommand
        {
            get { return (this.stopScrcpyCommand) ?? (this.stopScrcpyCommand = new DelegateCommand(StopScrcpy)); }
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
                    //if (ScrcpyProcess.Responding)
                    //{
                    //    //Process was responding; close the main window.
                    //    ScrcpyProcess.CloseMainWindow();
                    //    Debug.Write("CloseMainWindow");
                    //}
                    //else
                    //{
                    //    //Process was not responding; force the process to close.
                    ScrcpyProcess.Kill();
                //}
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
                RaisePropertyChanged("ScrcpyProcess2");
            }
        }
        private bool _record2 = true;

        public bool Record2
        {
            get => _record2; set
            {
                _record2 = value;
                RaisePropertyChanged("Record2");
            }
        }

        private string _output2;

        public string Output2
        {
            get { return _output2; }
            set
            {
                _output2 = value;
                RaisePropertyChanged("Output2");
            }
        }

        private bool _isRunning2;
        private Process _scrcpyProcess2;

        public bool IsRunning2
        {
            get => _isRunning2; set
            {
                _isRunning2 = value;
                RaisePropertyChanged("_isRunning2");
            }
        }

        private ICommand startScrcpyCommand2;
        public ICommand StartScrcpyCommand2
        {
            get { return (this.startScrcpyCommand2) ?? (this.startScrcpyCommand2 = new DelegateCommand(StartScrcpy2)); }
        }

        bool isProcRunSuccess2;
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

            isProcRunSuccess2 = false;

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
                if (isProcRunSuccess2)
                {
                    ScrcpyProcess2 = process;
                    if (ScrcpyProcess2 == null)
                    {
                        Debug.WriteLine("Error : ScrcpyProcess2 is null");
                    }
                    else
                    {
                        RaisePropertyChanged("CanStopScrcpy2");
                        RaisePropertyChanged("CanStartScrcpy2");
                    }
                }
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
                if (isProcRunSuccess2)
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

            RaisePropertyChanged("CanStopScrcpy2");
            RaisePropertyChanged("CanStartScrcpy2");
        }

        private void ScrcpyProcessOnOutputDataReceived2(object sender, DataReceivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Output += e.Data + "\r\n";
            });
            if (e.Data != null && !e.Data.Contains("error"))
            {
                isProcRunSuccess2 = true;

                RaisePropertyChanged("CanStopScrcpy2");
                RaisePropertyChanged("CanStartScrcpy2");
            }
        }

        private ICommand stopScrcpyCommand2;
        public ICommand StopScrcpyCommand2
        {
            get { return (this.stopScrcpyCommand2) ?? (this.stopScrcpyCommand2 = new DelegateCommand(StopScrcpy2)); }
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
                    //if (ScrcpyProcess2.Responding)
                    //{
                    //    //Process was responding; close the main window.
                    //    ScrcpyProcess2.CloseMainWindow();
                    //    Debug.Write("CloseMainWindow");
                    //}
                    //else
                    //{
                    //Process was not responding; force the process to close.
                    ScrcpyProcess2.Kill();
                //}
            }
        }

        public bool CanStopScrcpy2 => !ScrcpyProcess2?.HasExited ?? true;
        public bool CanStartScrcpy2 => ScrcpyProcess2?.HasExited ?? true;
    }

    #region DelegateCommand Class
    public class DelegateCommand : ICommand
    {

        private readonly Func<bool> canExecute;
        private readonly Action execute;

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class.
        /// </summary>
        /// <param name="execute">indicate an execute function</param>
        public DelegateCommand(Action execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class.
        /// </summary>
        /// <param name="execute">execute function </param>
        /// <param name="canExecute">can execute function</param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        /// <summary>
        /// can executes event handler
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// implement of icommand can execute method
        /// </summary>
        /// <param name="o">parameter by default of icomand interface</param>
        /// <returns>can execute or not</returns>
        public bool CanExecute(object o)
        {
            if (this.canExecute == null)
            {
                return true;
            }
            return this.canExecute();
        }

        /// <summary>
        /// implement of icommand interface execute method
        /// </summary>
        /// <param name="o">parameter by default of icomand interface</param>
        public void Execute(object o)
        {
            this.execute();
        }

        /// <summary>
        /// raise ca excute changed when property changed
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
    #endregion
}

