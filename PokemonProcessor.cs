namespace PokemonCSV
{
    internal class PokemonProcessor
    {
        /// <summary>
        /// Находит все уникальные группы по столбцу Type 1/
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        /// <returns>Хэшсет со всеми группами столбца Type 1/</returns>
        public static HashSet<string> GetPokemonTypes(string[][] data)
        {
            HashSet<string> firstTypes = new HashSet<string> { };
            foreach (var line in data)
            {
                firstTypes.Add(line[2]);
            }
            return firstTypes;
        }
        /// <summary>
        /// Функция, возвращающая выборку покемонов по значению поля Type 1.
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        /// <param name="firstTypeValue">Значение поля Type 1</param>
        /// <returns>Итоговая выборка по полю Type 1.</returns>
        public static string[][] GetAllFirstTypeXPokemons(string[][] data, string firstTypeValue)
        {
            List<string[]> poisonousList = new List<string[]>();
            foreach(string[] item in data)
            {
                if (item[2] == firstTypeValue)
                {
                    poisonousList.Add(item);
                }
            }
            return poisonousList.ToArray();
        }

        /// <summary>
        /// Сортирует таблицу покемонов по возрастанию атаки
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        /// <returns>Отсортированный по возрастанию атаки массив массивов.</returns>
        public static string[][] GetSortedByATK(string[][] data)
        {
            for (int i = 0; i < data.Length - 1; i++)
            {
                for (int j = 0; j < data.Length - i - 1; j++)
                {
                    if (int.Parse(data[j][6]) > int.Parse(data[j + 1][6]))
                    {
                        string[] swapTmp = data[j];
                        data[j] = data[j + 1];
                        data[j + 1] = swapTmp;
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Переорганизовывает список покемонов, объединяя их по типам. Внутри групп производится сортировка
        /// по возрастанию атаки методом PokemonProcessor.GetSortedByATK();
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        /// <returns>Список покемонов, сгрупированных по группам с сортировкой по атаке.</returns>
        public static string[][] GetRearrangedList(string[][] data)
        {
            string[][] newLines = { };
            var firstTypes = GetPokemonTypes(data);
            foreach(var type in firstTypes)
            {
                 newLines = newLines.Concat(GetSortedByATK(GetAllFirstTypeXPokemons(data, type))).ToArray();
            }
            return newLines;
        }

        /// <summary>
        /// Вычисляет разницу между максимальным и минимальным значениями атаки в списке покемонов.
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        /// <returns>Число - разница в атаке.</returns>
        public static int GetDeltaATK(string[][] data)
        {
            int[] ATKValues = new int[data.Length];
            for(int i = 0; i < ATKValues.Length; i++)
            {
                ATKValues[i] = int.Parse(data[i][6]);
            }
            return ATKValues.Max() - ATKValues.Min();
        }

        /// <summary>
        /// Вычисляет среднее значение здоровья в списке покемонов.
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        /// <returns>Число с плавающей точкой - среднее значение здоровья.</returns>
        public static float GetAverageHP(string[][] data)
        {
            float sum = 0;
            int counter = 0;
            foreach(var line in data)
            {
                sum += float.Parse(line[5]);
                counter++;
            }
            return sum / counter;
        }

        /// <summary>
        /// Находит всех покемонов с пустым значение типа 2.
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        /// <returns>Список покемонов с пустым вторым типом.</returns>
        public static string[][] GetEmptyType2(string[][] data)
        {
            List <string[]> emptyType2List = new List<string[]> ();
            foreach(var line in data)
            {
                if(line[3] == "")
                {
                    emptyType2List.Add(line);
                }
            }
            return emptyType2List.ToArray();
        }

        /// <summary>
        /// Вычисляет количнство покемонов.
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        /// <returns>Количество покемонов.</returns>
        public static int GetDataSetLength(string[][] data) => data.Length;

        /// <summary>
        /// Вычисляет количество групп по столбцу Type 1.
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        /// <returns>Количество групп.</returns>
        public static int GetGroupCount(string[][] data) => GetPokemonTypes(data).Count;

        /// <summary>
        /// Вычисляет количество покемонов в каждой группе по полю Type 1.
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        /// <returns>Словарь вида (название группы: количество покемонов в группе).</returns>
        public static Dictionary<string, int> GetPokemonCountByGroup(string[][] data)
        {
            Dictionary<string, int> typeToCount = new Dictionary<string, int>();
            var firstTypes = GetPokemonTypes(data);
            foreach (var type in firstTypes)
            {
                typeToCount[type] = 0;
            }
            foreach (var line in data)
            {
                typeToCount[line[2]]++;
            }
            return typeToCount;
        }

        /// <summary>
        /// Вычисляет количество призраков по столбцу Type 2 в рахных группах Type 1.
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        /// <returns>Словарь вида (название группы: количество покемонов-призраков в группе).</returns>
        public static Dictionary<string, int> GetGhostsByGroups(string[][] data)
        {
            var firstTypes = GetPokemonTypes(data);
            Dictionary<string, int> typeToGhostCount = new Dictionary<string, int>();
            foreach (var type in firstTypes)
            {
                typeToGhostCount[type] = 0;
            }
            foreach (var line in data)
            {
                if (line[3] == "Ghost")
                {
                    typeToGhostCount[line[2]]++;
                }
            }
            return typeToGhostCount;
        }

        /// <summary>
        /// Вычисляет количество летающих (по Type 1) драконов (по Type 2).
        /// </summary>
        /// <param name="data">Массив массивов с данными CSV файла.</param>
        /// <returns>Количество летающих драконов.</returns>
        public static int GetFlyingDragons(string[][] data)
        {
            int count = 0;
            foreach(var line in data)
            {
                if (line[2] == "Flying" && line[3] == "Dragon")
                {
                    count++;
                }
            }
            return count;
        }
    }
}
