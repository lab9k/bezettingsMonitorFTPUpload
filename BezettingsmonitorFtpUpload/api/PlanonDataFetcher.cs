using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Data.SqlClient;

namespace BezettingsmonitorFtpUpload.api
{
  public class PlanonDataFetcher
  {
    private string[] Script { get; }

    public PlanonDataFetcher() {
      var parent = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
      var path = Path.Combine(parent,
        "src/BezettingsmonitorFtpUpload/queries/query.sql");
      this.Script = File.ReadAllLines(path);
    }

    public IEnumerable<string> Query() {
      var ret = new List<string> {
        "MEETING_ROOM_ID;MEETING_TITLE;MEETING_DESCRIPTION;MEETING_CAPACITY;MEETING_ACTOR_ID;MEETING_START;MEETING_END;MEETING_CREATED;MEETING_MODIFIED;;"
      };
      try {
        var dataSource = Environment.GetEnvironmentVariable("INPUT_DB_HOST");
        var userId = Environment.GetEnvironmentVariable("INPUT_DB_USER");
        var password = Environment.GetEnvironmentVariable("INPUT_DB_PASSWORD");
        var initialCatalog = Environment.GetEnvironmentVariable("INPUT_DB_NAME");

        var builder = new SqlConnectionStringBuilder {
          DataSource = dataSource,
          UserID = userId,
          Password = password,
          InitialCatalog = initialCatalog
        };
        using (var connection = new SqlConnection(builder.ConnectionString)) {
          Console.WriteLine("\nQuerying data:");
          Console.WriteLine("=========================================\n");
          connection.Open();
          var sb = new StringBuilder();
          foreach (var s in Script) {
            sb.Append("\n");
            sb.Append(s);
          }

          var sql = sb.ToString();
          using (var command = new SqlCommand(sql, connection)) {
            using (var reader = command.ExecuteReader()) {
              while (reader.Read()) {
                var meetingRoomId = reader.IsDBNull(0) ? "" : reader.GetString(0);
                var meetingTitle = reader.IsDBNull(1) ? "" : reader.GetString(1);
                var meetingDescription = reader.IsDBNull(2) ? "" : reader.GetString(2);
                var meetingCapacity = reader.IsDBNull(3) ? "" : $"{reader.GetInt32(3)}";
                var meetingActorId = reader.IsDBNull(4) ? "" : reader.GetString(4);
                var meetingStart = reader.IsDBNull(5) ? "" : $"{reader.GetString(5)}";
                var meetingEnd = reader.IsDBNull(6) ? "" : $"{reader.GetString(6)}";
                var meetingCreated = reader.IsDBNull(7) ? "" : $"{reader.GetString(7)}";
                var meetingModified = reader.IsDBNull(8) ? "" : $"{reader.GetString(8)}";

                var line =
                  $"\"{meetingRoomId}\";\"{meetingTitle}\";\"{meetingDescription}\";\"{meetingCapacity}\";\"{meetingActorId}\";\"{meetingStart}\";\"{meetingEnd}\";\"{meetingCreated}\";\"{meetingModified}\";;";
                ret.Add(line);
              }
            }
          }
        }
      }
      catch (SqlException e) {
        Console.WriteLine(e.ToString());
      }

      return ret;
    }
  }
}