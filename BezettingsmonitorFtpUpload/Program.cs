using BezettingsmonitorFtpUpload.api;

namespace BezettingsmonitorFtpUpload
{
  class Program
  {
    static void Main(string[] args)
    {
      while (true)
      {
      }

      var fetcher = new PlanonDataFetcher();
      var lines = fetcher.Query();
      FtpCsvWriter.WriteLines(lines);
    }
  }
}