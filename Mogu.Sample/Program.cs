using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Mogu.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var proc = Process.Start("notepad.exe");
            var pid = (uint)proc.Id;

            Debug.WriteLine($"target pid:{pid:X}");
            var injector = new Injector();
            using (var connection = await injector.InjectAsync(pid, c => EntryPoint(c)))
            {
                while (connection.IsConnected)
                {
                    byte[] buffer = new byte[1024];
                    var count = await connection.Pipe.ReadAsync(buffer, 0, buffer.Length, CancellationToken.None);
                    var str = Encoding.UTF8.GetString(buffer, 0, count);
                    Debug.WriteLine($"recv:{str}");
                }
            }

            MessageBox.Show("Process exited");
        }

        private static bool hooked;

        public static ValueTask EntryPoint(Connection connection)
        {
            connection.PreventAutoClose();
            MainWindow.connection = connection;

            if (hooked) return default;
            hooked = true;

            var t = new Thread(() =>
            {
                try
                {
                    var app = new App();
                    app.InitializeComponent();
                    app.Run();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            return default;
        }
    }
}
