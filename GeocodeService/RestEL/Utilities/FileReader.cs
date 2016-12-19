using System;
using System.IO;
using System.Text.RegularExpressions;

namespace FileScan
{
    public class FileReader
    {
        public static string ReadFileIntoString(string jsonFileLocation)
        {
            //validate filepath string
            CheckIllegalFilepathCharacters(jsonFileLocation);
  
            StreamReader _file = null;
            string stringifiedFile="";
            var formattedjsonFileLocation = " [FilePath: " + jsonFileLocation + "]";

            try
            {
                using (_file = new StreamReader(jsonFileLocation))
                {
                    stringifiedFile = _file.ReadToEnd();
                }
            }
            catch (FileNotFoundException )
            {
                Console.WriteLine("File cannot be found:" + formattedjsonFileLocation);
            }
            catch (DirectoryNotFoundException )
            {
                Console.WriteLine("Directory cannot be found: " + formattedjsonFileLocation);
            }                
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Unspecified error on {0}: {1}", formattedjsonFileLocation, e));
            }
            finally
            {
                if (_file != null)
                {
                    _file.Close();
                }
            }
            return stringifiedFile;
        }

        private static void CheckIllegalFilepathCharacters(string jsonFileLocation)
        {
            char[] listOfBadFileCharacters = Path.GetInvalidFileNameChars();
            string badFileCharacters = new string(listOfBadFileCharacters);

            Regex r = new Regex("[" + badFileCharacters + "]");
            //valid legal characters in file path
            if (r.IsMatch(jsonFileLocation))
            {
                throw new Exception("Illegal characters found in this string: " + jsonFileLocation);
            }
        }
    }


}
