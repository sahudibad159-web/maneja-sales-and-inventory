using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Windows.Forms;

public static class ConnectionModule
{
    public static MySqlConnection con = new MySqlConnection(
    "server=localhost;user id=root;password=;database=sales_inventory"
);


    public static void openCon()
    {
        if (con.State == ConnectionState.Closed)
            con.Open();
    }

    public static void closeCon()
    {
        if (con.State == ConnectionState.Open)
            con.Close();
    }

    // ✅ Session class para sa buong app
    public static class Session
    {
        public static string FullName { get; set; }
        public static string Username { get; set; }
        public static string Role { get; set; }
        public static DateTime ShiftStart { get; set; }
    }

    // ✅ Audit Trail method (hindi na kailangan ipasa ang username)
    public static void InsertAuditTrail(string actionType, string module, string description)
    {
        try
        {
            openCon();
            string query = "INSERT INTO AuditTrail (Username, Role, ActionType, ModuleName, Description, ActionDate) " +
                  "VALUES (@Username, @Role, @ActionType, @ModuleName, @Description, NOW())";


            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Username", ConnectionModule.Session.Username ?? "Unknown");
                cmd.Parameters.AddWithValue("@Role", ConnectionModule.Session.Role ?? "Unknown");

                cmd.Parameters.AddWithValue("@ActionType", actionType);
                cmd.Parameters.AddWithValue("@ModuleName", module);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Audit Trail Error: " + ex.Message);
        }
        finally
        {
            closeCon();
        }
    }
}
