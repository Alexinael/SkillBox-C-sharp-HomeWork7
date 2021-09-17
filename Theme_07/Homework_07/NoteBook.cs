using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace Homework_07
{

    public class WeatherForecast
    {
        public DateTimeOffset Date { get; set; }
        public int TemperatureCelsius { get; set; }
        public string Summary { get; set; }
    }

    public struct NoteBook
    {
        /// <summary>
        /// Переменная принимающая в себя все записи еждневника 
        /// TODO возможно в дальнейшем сделать динамическую подгрузку
        /// </summary>
        private List<NoteBookRecord> allRecords ;

        private string title;

        /// <summary>
        /// Путь к сохраняемым файлам
        /// </summary>
        public string path { get; set; }

        public static List<Option> options;
        private int currentIndexMenu;
        

        /// <summary>
        /// Показать главное меню
        /// </summary>
        public void showMainMenu(bool needClear = true)
        {
            Console.Clear();
            
            string line = "";
            byte index = 0;
            foreach (Option option in options)
            {
                line = "    ";
                Console.ForegroundColor = ConsoleColor.White;
                string Name = option.Name;
                if (index == currentIndexMenu)
                {
                    if (option.Selected != null)
                        line = "   >";
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (option.Selected == null )
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Name = option.Name.ToUpper();
                }

                line += Name;
                Console.WriteLine(line);
                index++;
            }
            this.waitKey();
            return;
        }


        internal void Start()
        {
            foreach (Option option in options)
            {
                if (option.Selected != null)
                {
                    this.currentIndexMenu++;
                    break;
                }
            }
            this.showMainMenu();
            
        }

        private void waitKey()
        { 
            ConsoleKeyInfo consoleKey;
            consoleKey = Console.ReadKey();
            
            if (consoleKey.Key == ConsoleKey.DownArrow)
            {
                do // прыгаем вниз пока не найлем метод у опции 
                {
                    this.currentIndexMenu++;
                    if (currentIndexMenu > options.Count - 1)
                    {
                        this.currentIndexMenu = 0;
                    }
                }
                while (options[this.currentIndexMenu].Selected == null);

            }
            if (consoleKey.Key == ConsoleKey.UpArrow)
            {

                do // прыгаем вверх пока не найлем метод у опции 
                {
                    
                    this.currentIndexMenu--;
                    if (currentIndexMenu < 0)
                    {
                        this.currentIndexMenu = options.Count - 1;
                    }
                }
                while (options[this.currentIndexMenu].Selected == null);

               
            }
            if (consoleKey.Key == ConsoleKey.Enter)
            {
                if (options[this.currentIndexMenu].Selected != null)
                    options[this.currentIndexMenu].Selected();

                // выполняем процедуру из option
                
            }
            showMainMenu(); // ждем выбор
        }

        public void selectMenu()
        {
       
        }

        /// <summary>
        /// Показать все записи
        /// </summary>
        private void showAllRecord()
        {
            Console.WriteLine("Показываю все записи...");
            foreach (NoteBookRecord record in allRecords)
            {
                Console.WriteLine($"{record.getRecord()}\n");
            }
            Console.WriteLine("\n");
        }

        /// <summary>
        /// сохранение в файл
        /// </summary>
        private void saveToFile()
        {
            this.save(this.path);
        }

        /// <summary>
        /// Загрузка из файла
        /// </summary>
        private void loadFromFile()
        {
            this.load(this.path);
        }

        /// <summary>
        /// Показывает в консоли заголовок
        /// </summary>
        /// <param name="title">заголовок</param>
        public void showTitle(string title)
        {
            Console.WriteLine($"==========={title}===========");
        }

        public int countRecord()
        {
            return allRecords.Count;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public NoteBook(string title = "Без названия", string path = "")
        {
            this.title = title;
            this.path = path;
            this.allRecords = new List<NoteBookRecord>();
            this.currentIndexMenu = 0;
            var local = this;
            options = new List<Option>
            {
                
                new Option("Записи", null ),
                new Option($"\t Показать все записи ", () => {
                    Console.CursorVisible = true;
                    
                    local.showTitle("Действия с записями");
                    local.showAllRecord();
                    local.pause();
                    local.showMainMenu();
                    }),
                new Option("\t Добавить запись", () => {
                    Console.CursorVisible = true;

                    local.addRecord();

                    
                    local.showMainMenu();
                    }),
                new Option("\t Удалить запись", () => {
                    Console.CursorVisible = true;

                    local.showTitle("Действия с записями");

                    local.showMainMenu();
                    }),
                new Option("\t Удалить записи с определенным условием", () => {
                    Console.CursorVisible = true;

                    local.showTitle("Действия с записями");

                    local.showMainMenu();
                    }),

                new Option("Файл", null),
                new Option("\tЗаписать",()=> 
                    {
                        string _path = local.path + "out.json";
                        local.save(_path);
                        local.showMainMenu();
                    }),
                new Option("\tЗагрузить", () => 
                    {
                        string _path = local.path + "out.json";
                        local.load(_path);
                    }),
                new Option("\tЗагрузить с условием", () => 
                    {

                    }),
                new Option("Выход", () =>
                {
                    System.Environment.Exit(0);
                })
            };

        }

        private void pause(string text = "")
        {
            if (text.Trim() != "")
            {
                Console.WriteLine($"{text}");
            }

            Console.WriteLine("Для возврата в выбор нажмите любую клавшиу");
            Console.ReadKey();
        }

        private void addRecord()
        {
            showTitle("Действия с записями");

            Console.WriteLine("Введи заголовок:\n");
            string recordTitle = Console.ReadLine();
            Console.WriteLine("Введи текст записи:\n");
            string recordText = Console.ReadLine();

            NoteBookRecord noteBookRecord = new NoteBookRecord(recordTitle, recordText, allRecords.Count());
            allRecords.Add(noteBookRecord);
        }

        /// <summary>
        /// Загрузка данных из файла
        /// </summary>
        /// <param name="Path">Путь к файлу</param>
        public void load(string Path)
        {
            load(Path, null);
        }

        /// <summary>
        /// Загрузка данных из файла по диапазону дат
        /// </summary>
        /// <param name="Path">Путь к файлу</param>
        /// <param name="Date">Дата диапазона (строка)</param>
        public void load (string Path, string Date = null)
        {
            if (!File.Exists(Path))
            {
                Console.WriteLine($"Файла <{Path}> не существует");
            }
            StreamReader streamReader = new StreamReader(Path);
            string line = "";
            do
            {
                line = streamReader.ReadLine();
                Console.WriteLine($"line : {line}");
            }
            while (line != null);
            Console.ReadKey();
        }
        public string toJson()
        {
            List<string> forReturn = new List<string>();

            foreach(NoteBookRecord items in this.allRecords)
            {
               // string s = $"\{'\}";
            }
            WeatherForecast weatherForecast = new WeatherForecast();
            weatherForecast.Date = DateTime.Now;

            //string jsonString = JsonSerializer.Serialize(weatherForecast);

            return "";
        }


        /// <summary>
        /// Запись данных в файл
        /// </summary>
        /// <param name="Path">Путь к файлу (без расширения)</param>
        public void save(string Path)
        {
            if (File.Exists(Path))
            {
                Console.WriteLine("Файл существует, перезаписать? Да/нет (Y/n)");
                string ans = Console.ReadLine();
                String[] s = new string[] {
                    "1","д","y","","да"
                };


                if (!s.Contains( ans.ToLower())) {
                    return;
                }

            }
            try
            {
                TextWriter tx;//= new TextWriter(Path);
                //StreamWriter streamWriter = new StreamWriter(Path);
                string _json; //Serialize<List<NoteBookRecord>>(this.allRecords);

                //Newtonsoft.Json.JsonSerializer jsonSerializer = new Newtonsoft.Json.JsonSerializer();//.SerializeObject(this.allRecords);

                //_json = Newtonsoft.Json.JsonConvert.SerializeObject( this.allRecords.ToArray());
                /*
                List<NoteBookRecord> k = new List<NoteBookRecord>();
                NoteBookRecord n = new NoteBookRecord("TEST TITLE", "TEST TEXT", 1);
                NoteBookRecord nn = new NoteBookRecord("TEST TITLE", "TEST TEXT", 1);
                k.Add(n);

                var ser = JsonSerializer.CreateDefault(new JsonSerializerSettings()
                {
                    
                });
                */
               // ser.Serialize(tx, k);
                
               // _json = JsonConvert.SerializeObject(nn);
                //_json = JsonConvert.SerializeObject(k);
                //_json = JsonSerializer.Serialize(tx, k );
                _json = JsonConvert.SerializeObject(this.allRecords);
                
               // _json = toJson();
                File.WriteAllText(Path, _json);

                //streamWriter.Write(_json);
                 
                //streamWriter.Close();
                this.pause("Запись завершена");
            }
            catch (Exception error) {
                this.pause($"Ошибка записи: {error}");
            }
        }

    }
}

