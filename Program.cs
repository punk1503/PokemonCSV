using System;
using System.ComponentModel.Design;

namespace PokemonCSV
{
    class Program
    {
        static string[][] _data = { };
        static string[][] _changedData = { };
        static string _command = "";
        static private string[] _commands = {
            "0. Выход",
            "1. Загрузить файл",
            "2. Вывести информацию о всех ядовитых покемонах(по полю Type 1)",
            "3. Вывести покемонов по типам(Type 1) с сортировкой по атаке",
            "4. Вывести выборку покемонов без дополнительных особенностей и их средний показатель здоровья",
            "5. Общая сводка"
        };

        /// <summary>
        /// Выводит меню с командами.
        /// </summary>
        static void PrintMenu()
        {
            Console.WriteLine("Выберите действие(введите только число):");
            foreach(var command in _commands)
            {
                Console.WriteLine(command);
            }
        }

        /// <summary>
        /// Выводит сообщение о пустоте текущего массива данных.
        /// </summary>
        static void PrintNoDataAlert()
        {
            Console.WriteLine("На данный момент файл не загружен");
        }

        /// <summary>
        /// Спрашивает у пользователя желание и путь для сохранения текущей выборки.
        /// </summary>
        static void SavingDialog()
        {
            Console.WriteLine("Желаете ли сохранить выборку? 1 - Да, иначе - Нет");
            if (Console.ReadLine() == "1")
            {
                try
                {
                    Console.Write("Введите имя файла:");
                    CSVRW.WriteCSVFile(Console.ReadLine(), _changedData);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Некорректное имя файла");
                }
            }
        }

        /// <summary>
        /// Точка входа :)
        /// </summary>
        static void Main()
        {

            while(true)
            {
                PrintMenu();
                _command = Console.ReadLine();
                switch (_command)
                {
                    case "1":
                        Console.Write("Введите имя файла:");
                        try
                        {
                            _data = CSVRW.ReadCSVData(Console.ReadLine());
                        }
                        catch (FileNotFoundException e)
                        {
                            Console.WriteLine("Файл не найден");
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine("Некорректное имя файла");
                        }
                        catch(Exception e)
                        {
                            if(e.Message == "Incorrect file structure")
                            {
                                Console.WriteLine("Некорректная структура файла");
                            }
                        }
                        break;
                    case "2":
                        if (_data.Length != 0)
                        {
                            _changedData = PokemonProcessor.GetAllFirstTypeXPokemons(_data, "Poison");
                            CSVRW.PrintCSVData(_changedData);
                            SavingDialog();
                        }
                        else
                        {
                            PrintNoDataAlert();
                        }
                        break;
                    case "3":
                        if (_data.Length != 0)
                        {
                            _changedData = PokemonProcessor.GetRearrangedList(_data);
                            Console.WriteLine($"Дельта атаки покемонов типа Dark:{PokemonProcessor.GetDeltaATK(PokemonProcessor.GetAllFirstTypeXPokemons(_data, "Dark"))}");
                            CSVRW.PrintCSVData(_changedData);
                            SavingDialog();
                        }
                        else
                        {
                            PrintNoDataAlert();
                        }
                        break;
                    case "4":
                        if (_data.Length != 0)
                        {
                            _changedData = PokemonProcessor.GetEmptyType2(_data);
                            Console.WriteLine($"Среднее значение здоровья покемонов без второго типа: {PokemonProcessor.GetAverageHP(_changedData)}");
                            CSVRW.PrintCSVData(_changedData);
                            SavingDialog();
                        }
                        else
                        {
                            PrintNoDataAlert();
                        }
                        break;
                    case "5":
                        if (_data.Length != 0)
                        { 
                            Console.WriteLine($"Всего покемонов: {PokemonProcessor.GetDataSetLength(_data)}");
                            Console.WriteLine($"Всего типов: {PokemonProcessor.GetGroupCount(_data)}");
                            Console.WriteLine("Группа: Количество покемонов");
                            foreach (var (key, value) in PokemonProcessor.GetPokemonCountByGroup(_data))
                            {
                                Console.WriteLine($"{key}: {value}");
                            }
                            Console.WriteLine("Группа: Количество покемонов типа Ghost");
                            foreach (var (key, value) in PokemonProcessor.GetGhostsByGroups(_data))
                            {
                                Console.WriteLine($"{key}: {value}");
                            }
                            Console.WriteLine($"Количество летающих покемонов: {PokemonProcessor.GetFlyingDragons(_data)}");
                        }
                        else
                        {
                            PrintNoDataAlert();
                        }
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Некорректная команда");
                        break;
                }
            }
        }
    }

}
