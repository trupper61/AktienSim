using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace aktiensim
{
    public class MySqlManager : IDisposable
    {
        private readonly int maxRetries = 3;
        private readonly int delayMilliseconds = 2000;
        private bool disposed = false;

        public MySqlConnection Connection;
        public string connectionString = "server=localhost;database=aktiensimdb;uid=root;password=";
        public DepotVerwaltung Depot { get; }
        public TransaktionVerwaltung Transaktion { get; }
        public AktienVerwaltung Aktien { get; }
        public Benutzerverwaltung Benutzer { get; }
        public EreignisVerwaltung Ereignis { get; }
        public MySqlManager() 
        {
            Connection = CreateConnectionWithRetry();
            Depot = new DepotVerwaltung(Connection);
            Transaktion = new TransaktionVerwaltung(Connection);
            Aktien = new AktienVerwaltung(Connection);
            Benutzer = new Benutzerverwaltung(Connection);
            Ereignis = new EreignisVerwaltung(Connection);
        }
        private MySqlConnection CreateConnectionWithRetry()
        {
            int retries = 0;
            while (true)
            {
                try
                {
                    var conn = new MySqlConnection(connectionString);
                    conn.Open();
                    return conn;
                }
                catch (MySqlException ex) when (ex.Number == 1040)
                {
                    retries++;
                    if (retries >= maxRetries)
                        throw new Exception("Verbindungsaufbau fehlgeschlagen nach mehreren Versuchen.", ex);

                    Thread.Sleep(delayMilliseconds);
                }
            }
        }
        public void Dispose()
        {
            Connection?.Close();
            Connection?.Dispose();
        }
    }
}
