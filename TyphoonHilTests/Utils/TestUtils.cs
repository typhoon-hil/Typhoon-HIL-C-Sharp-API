using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyphoonHilTests.Utils
{
    public static class TestUtils
    {
        public static void ClearDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                foreach (var file in Directory.GetFiles(directoryPath)) File.Delete(file);

                foreach (var subDirectory in Directory.GetDirectories(directoryPath)) ClearDirectory(subDirectory);
            }
            else
            {
                throw new DirectoryNotFoundException($"Directory '{directoryPath}' not found.");
            }
        }
    }
}
