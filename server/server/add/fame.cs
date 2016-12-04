using db;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
namespace server.add
{ 
       class fame : RequestHandler
        {
            protected override void HandleRequest()
            {
                string status = "403";
                using (Database db = new Database())
                {
                    NameValueCollection query = HttpUtility.ParseQueryString(Context.Request.Url.Query);
                    MySqlCommand cmd = db.CreateQuery();
                    cmd.CommandText = "SELECT id FROM accounts WHERE uuid=@uuid";
                    cmd.Parameters.AddWithValue("@uuid", query["guid"]);
                    object id = cmd.ExecuteScalar();
                    if (id != null)
                    {
                        int amount = int.Parse(query["aid"]);
                        cmd = db.CreateQuery();
                        cmd.CommandText = "UPDATE stats SET fame = fame + @amount WHERE accId=@accId";
                        cmd.Parameters.AddWithValue("@accId", id);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                            status = "400";
                        else
                            status = "500";
                    }
                    else
                        status = "404";
                }
                byte[] res = Encoding.UTF8.GetBytes(
                    status);
                Context.Response.OutputStream.Write(res, 0, res.Length);
            }
        }
    }
