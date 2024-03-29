using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Renci.SshNet;

namespace BezettingsmonitorFtpUpload.api
{
  public static class FtpCsvWriter
  {
    public static void WriteLines(IEnumerable<string> lines) {
      var host = Environment.GetEnvironmentVariable("FTP_HOST")?.Trim();
      var port = int.Parse(Environment.GetEnvironmentVariable("FTP_PORT"));
      var username = Environment.GetEnvironmentVariable("FTP_USERNAME")?.Trim();
      var password = Environment.GetEnvironmentVariable("FTP_PASSWORD")?.Trim();

      var connectionInfo =
        new ConnectionInfo(host, port, username, new PasswordAuthenticationMethod(username, password));

      using (var sftp = new SftpClient(connectionInfo)) {
        sftp.Connect();

        if (sftp.IsConnected) {
          Console.WriteLine("Connected to sftp.");
          var text = string.Join("\n", lines);
          var buffer = Encoding.UTF8.GetBytes(text);
          sftp.UploadFile(new MemoryStream(buffer), "flexwhere-meetings.csv", true);
          sftp.Disconnect();
        }
      }

      Console.WriteLine("Writing to sftp was successful.");
    }
  }
}