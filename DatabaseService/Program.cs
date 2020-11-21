using DatabaseService.DB;
using System;
using Topshelf;

namespace DatabaseService
{
    class Program
    {
        /// <summary>
        /// This program creates a service that can be installed in the system, which handles
        /// every request sent via RabbitMQ with petitions to access the database.
        /// </summary>
        static void Main()
        {
            var rc = HostFactory.Run(x =>
            {
                x.Service<DatabaseListener>(s =>
                {
                    s.ConstructUsing(name => new DatabaseListener(new DBAccess()));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Meter database listener");
                x.SetDisplayName("Meter database listener");
                x.SetServiceName("MDL");
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
