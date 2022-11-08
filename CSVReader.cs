using System.Data;
using System.Formats.Asn1;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace PokemonCSV
{
    internal class CSVRW
    {
        // Заголовок CSV таблицы с названиями параметров.
        private static string _CSVHeader = "#,Name,Type 1,Type 2,Total,HP,Attack,Defense,Sp. Atk,Sp. Def,Speed,Generation,Legendary";
        
        /// <summary>
        /// Функция, выводящая заголовок файла без разделителей.
        /// </summary>
        public static void PrintHeader()
        {
            Console.WriteLine(_CSVHeader.Replace(',', ' '));
        }

        /// <summary>
        /// Функция, считывающая данные из таблицы формата CSV и записывающая их в список.
        /// </summary>
        /// <param name="path">Путь к CSV файлу.</param>
        /// <returns>Список - таблицу значений.</returns>
        public static string[][] ReadCSVData(string path)
        {
            try
            {
                var lines = File.ReadAllText(path.Trim('"')).Replace(';', ',').Split('\n');
                lines = lines.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                if (CheckFileStructure(lines))
                {
                    var csvData = new List<string[]>();
                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (lines[i].Contains(','))
                        {
                            csvData.Add(lines[i].Split(','));
                        }
                    }
                    return csvData.ToArray();
                }
                else
                {
                    throw new Exception("Incorrect file structure");
                }
            }
            catch(FileNotFoundException e) {
                throw e;
            }
            catch(ArgumentException e)
            {
                throw e;
            }
            
        }

        /// <summary>
        /// Выводит в консоль данные выборки.
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        public static void PrintCSVData(string[][] data)
        {
            PrintHeader();
            foreach (var line in data)
            {
                foreach (var i in line)
                {
                    Console.Write(i + ' ');
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Записывает выборку в CSV файл.
        /// </summary>
        /// <param name="path">Путь к новому файлу.</param>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        public static void WriteCSVFile(string path, string[][] data)
        {
            string[] linesFromData = new string[data.Length+1];
            linesFromData[0] = _CSVHeader;
            for(int i=1; i<data.Length; i++)
            {
                linesFromData[i] = string.Join(",", data[i]);
            }
            File.WriteAllLines(path, linesFromData);
        }

        /// <summary>
        /// Проверяет файловую структуру.
        /// </summary>
        /// <param name="lines">Список строк исходного файла.</param>
        /// <returns>Булевое значение: корректна ли файловая структура.</returns>
        private static bool CheckFileStructure(string[] lines)
        {
            bool isCorrect = true;
            if (lines[0].Replace(';', ',') != _CSVHeader)
                isCorrect = false;
            foreach (var line in lines)
            {
                Console.WriteLine(line);
                if(!(new Regex(@",").Matches(line).Count == 12 || new Regex(@";").Matches(line).Count == 12))
                {
                    isCorrect = false;
                }
            }
            return isCorrect;
        }

    }
}
