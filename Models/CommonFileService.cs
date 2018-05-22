using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class CommonFileService
    {

        public void ReadAllFiles(IEnumerable<string> paths)
        {
            var files = paths.Select(ReadAllLines);
            Parallel.ForEach(files, file => { });
        }

        public IEnumerable<string> ReadAllLines(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(fileStream))
            {
                while (!streamReader.EndOfStream)
                {
                    yield return streamReader.ReadLine();
                }
            }
        }
    }
}