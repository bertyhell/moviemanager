using System.Diagnostics;

namespace Tmc.SystemFrameworks.Common
{
    public static class CommandHelper
    {
        /// <summary>
        /// Executes a shell command synchronously.
        /// </summary>
        /// <param name="command">string command</param>
        /// <param name="arguments"> </param>
        /// <returns>string, as output of the command.</returns>
        public static Process ExecuteCommandSync(string command, string arguments)
        {
            // create the ProcessStartInfo using "cmd" as the program to be run,
            // and "/c " as the parameters.
            // Incidentally, /c tells cmd that we want it to execute the command that follows,
            // and then exit.
            ProcessStartInfo ProcStartInfo = new ProcessStartInfo(command, arguments)
                                                 {
                                                     RedirectStandardOutput = false,
                                                     UseShellExecute = false,
                                                     CreateNoWindow = true
                                                 };

            // The following commands are needed to redirect the standard output.
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            // Do not create the black window.
            // Now we create a process, assign its ProcessStartInfo and start it
            Process Proc = new Process { StartInfo = ProcStartInfo, EnableRaisingEvents = true };
            Proc.Start();
            return Proc;
        }

    }
}
