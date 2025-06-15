using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class TransaktionVerwaltung
    {
        private readonly MySqlConnection connection;
        public TransaktionVerwaltung(MySqlConnection connection)
        {
            this.connection = connection;
        }

        public void AddTransaktion(int aktieId, string typ, double anzahl, double einzelpreis, Benutzer activeUser)
        {
            string query = "INSERT INTO transaktion (aktie_ID, typ, anzahl, einzelpreis, zeitpunkt, depot_ID) VALUES(@aktie_ID, @typ, @anzahl, @einzelpreis, @zeitpunkt, @depot_ID)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@aktie_ID", aktieId);
                cmd.Parameters.AddWithValue("@typ", typ);
                cmd.Parameters.AddWithValue("@anzahl", anzahl);
                cmd.Parameters.AddWithValue("@einzelpreis", einzelpreis);
                DepotVerwaltung dv = new DepotVerwaltung(connection);
                Depot userDepot = dv.GetUserDepot(Convert.ToInt32(activeUser.benutzerID)).FirstOrDefault();
                cmd.Parameters.AddWithValue("@depot_id", userDepot.ID);
                cmd.Parameters.AddWithValue("@zeitpunkt", DateTime.Now);
                activeUser.kontoStand -= anzahl * einzelpreis;
                cmd.ExecuteNonQuery();
            }
        }
        public void AddÜberweisung(Benutzer send, Benutzer get, double betrag)
        {
            string query = "INSERT INTO ueberweisung (absender_id, empfaenger_id, betrag) VALUES (@absender, @empfaenger, @betrag)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@absender", send.benutzerID);
                cmd.Parameters.AddWithValue("@empfaenger", get.benutzerID);
                cmd.Parameters.AddWithValue("@betrag", betrag);
                cmd.ExecuteNonQuery();
            }
        }
        public List<Überweisung> LadeUeberweisungenFürBenutzer(int benutzerID)
        {
            List<Überweisung> result = new List<Überweisung>();
            string query = "SELECT * FROM ueberweisung WHERE absender_id = @id OR empfaenger_id = @id ORDER BY datum DESC";
            using (var myMan = new MySqlManager())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, myMan.Connection))
                {
                    cmd.Parameters.AddWithValue("@id", benutzerID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int Id = Convert.ToInt32(reader["id"]);
                            int AbsenderId = Convert.ToInt32(reader["absender_id"]);
                            int EmpfaengerId = Convert.ToInt32(reader["empfaenger_id"]);
                            double Betrag = Convert.ToDouble(reader["betrag"]);
                            DateTime Datum = Convert.ToDateTime(reader["datum"]);
                            result.Add(new Überweisung(Id, AbsenderId, EmpfaengerId, Betrag, Datum));
                }
                    }
                }
            }
            return result;
        }
        public void AktualisiereTransaktion(Transaktion t)
        {
            string query = "UPDATE transaktion SET anzahl = @anzahl WHERE id = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@anzahl", t.anzahl);
                cmd.Parameters.AddWithValue("@id", t.id);
                cmd.ExecuteNonQuery();
            }
        }
        public void LöscheTransaktion(int transId)
        {
            string query = "DELETE FROM transaktion WHERE id = @transId";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@transId", transId);
                cmd.ExecuteNonQuery();
            }
        }
        public List<Transaktion> LadeTransaktionenFürDepot(int depotId)
        {
            List<Transaktion> transaktionen = new List<Transaktion>();
            string query = @"SELECT id, aktie_ID, typ, anzahl, einzelpreis, zeitpunkt 
                        FROM transaktion
                        WHERE depot_ID = @depotId 
                        ORDER BY zeitpunkt DESC";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@depotId", depotId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        int aktieId = reader.GetInt32("aktie_ID");
                        string typ = reader.GetString("typ");
                        double anzahl = reader.GetDouble("anzahl");
                        decimal einzelpreis = reader.GetDecimal("einzelpreis");
                        DateTime zeitpunkt = reader.GetDateTime("zeitpunkt");

                        Transaktion transaktion = new Transaktion(id, aktieId, anzahl, einzelpreis, typ, zeitpunkt);
                        if (transaktion.anzahl > 0)
                            transaktionen.Add(transaktion);
                    }
                }
            }
            return transaktionen;
        }
    }
}
