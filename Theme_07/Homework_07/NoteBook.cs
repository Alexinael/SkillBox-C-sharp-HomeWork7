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
        static int countRecord = 0;
        private string title;

        /// <summary>
        /// Путь к сохраняемым файлам
        /// </summary>
        public string path { get; set; }

        public static List<Option> options;
        private int currentIndexMenu;

        /// <summary>
        /// Возврат строки lorem ipsum
        /// </summary>
        /// <param name="minWords">минимум слов</param>
        /// <param name="maxWords">максимум слов</param>
        /// <param name="minSentences"></param>
        /// <param name="maxSentences"></param>
        /// <param name="numParagraphs"> число параграфов</param>
        /// <returns></returns>
        static string LoremIpsum(int minWords, int maxWords,
            int minSentences, int maxSentences,
            int numParagraphs)
        {

            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
            "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
            "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                + minSentences + 1;
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            StringBuilder result = new StringBuilder();

            for (int p = 0; p < numParagraphs; p++)
            {
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { result.Append(" "); }
                        result.Append(words[rand.Next(words.Length)]);
                    }
                    result.Append(". ");
                }
                
            }

            return result.ToString();
        }

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

                if (index == 0)
                {
                    line = line + " [" + this.allRecords.Count + " шт.]";
                }

                Console.WriteLine(line);
                index++;
            }
            this.waitKey();
            return;
        }

        public void updateRecord()
        {
            countRecord = this.allRecords.Count;
        }

        public void showSubMainMenu( List<Option> subMenu = null)
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
                if (option.Selected == null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Name = option.Name.ToUpper();
                }

                line += Name;
                Console.WriteLine(line);
                index++;
            }
            this.showTitle("Выбери нужное");
            line = "";
            index = 0;
            foreach (Option option in subMenu)
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
                if (option.Selected == null)
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
            int cnt = 1;
            foreach (NoteBookRecord record in allRecords)
            {
                this.showTitle($"Запись №{cnt}");
                Console.WriteLine($"{record.getRecord()}\n");
                cnt++;
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
        private void loadFromFile(string date = null, string author = null, string message = null)
        {
            this.load(this.path, date, author, message);
        }

        /// <summary>
        /// Показывает в консоли заголовок
        /// </summary>
        /// <param name="title">заголовок</param>
        public void showTitle(string title)
        {
            Console.WriteLine($"==========={title}===========");
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

                new Option($"Записи", null ),
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
                new Option("\t Добавить случайную запись",() => {
                    local.addRecordRandom();
                    local.showMainMenu();
                    }),
                new Option("\t Удалить запись", () => {
                    Console.CursorVisible = true;

                    local.showTitle("Действия с записями");

                    local.showMainMenu();
                    }),
                new Option("\t Удалить записи (условие)", null),

                new Option("\t\t по автору",() => {
                    Console.CursorVisible = true;

                    local.showTitle("Действия с записями");

                    List<Option> subMenu = new List<Option>();
                    List<String> authors = new List<string>();
                    foreach(NoteBookRecord noteBookRecord in local.allRecords)
                    {
                        if (authors.IndexOf(noteBookRecord.author)==-1 )
                        {
                            authors.Add(noteBookRecord.author);
                            subMenu.Add(new Option($"{noteBookRecord.author}",()=>{
                                }));
                        }
                    }

                    local.showSubMainMenu(subMenu);
                    }),
                new Option("\t\t по дате",() => {
                    Console.CursorVisible = true;

                    local.showTitle("Действия с записями");

                    local.showMainMenu();
                    }),
                new Option("\t\t по тексту",() => {
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
                new Option("\tЗагрузить все", () => 
                    {
                        string _path = local.path + "out.json";
                        local.load(_path);
                    }),
                new Option("\tЗагрузить с условием", null),
                new Option("\t\tПо автору", () => 
                    {
                        Console.WriteLine("Введи отбор по автору >");
                        string _path = local.path + "out.json";
                        string author = Console.ReadLine().Trim();
                        local.load(_path,null, author );
                    }),
                new Option("\t\tПо дате", () =>
                    {
                        Console.WriteLine("Введи отбор по дате >");
                        string _path = local.path + "out.json";
                        string date = Console.ReadLine().Trim();
                        local.load(_path,date);
                    }),

                new Option("\t\tПо вхождению слова", () =>
                    {
                        Console.WriteLine("Введи отбор (сообщение)>");
                        string msg = Console.ReadLine().Trim();
                        string _path = local.path + "out.json";
                        local.load(_path,null, null, msg);
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

        

        private void addRecordRandom()
        {
            showTitle("Добавляю случайную запись");
            NoteBookRecord noteBookRecord = new NoteBookRecord(LoremIpsum(3, 4, 3, 4, 1), LoremIpsum(5, 6, 7, 7, 2), allRecords.Count());

            allRecords.Add(noteBookRecord);
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

        /*
        /// <summary>
        /// Загрузка данных из файла
        /// </summary>
        /// <param name="Path">Путь к файлу</param>
        public void load(string Path //, string date = null, string author = null, string message = null)
            )
        {
            load(Path);
        }

        */

        public List<NoteBookRecord> filter(List<NoteBookRecord> innerListRecord , string date = null, string author = null, string message  =null)
        {
            List<NoteBookRecord> tmpRecord = innerListRecord;

            if (author != null)
            {

                List<NoteBookRecord> forDelete = new List<NoteBookRecord>();
                foreach (NoteBookRecord record in tmpRecord)
                {
                    if (!record.author.Contains(author))
                    {
                        forDelete.Add(record);
                    }
                }
                foreach (NoteBookRecord forDel in forDelete)
                {
                    tmpRecord.Remove(forDel);
                }

            }
            if (message != null)
            {
                List<NoteBookRecord> forDelete = new List<NoteBookRecord>();
                foreach (NoteBookRecord record in tmpRecord)
                {
                    if (!record.comment.Contains(message) && !record.title.Contains(message))
                    {
                        forDelete.Add(record);
                    }
                }
                foreach (NoteBookRecord forDel in forDelete)
                {
                    tmpRecord.Remove(forDel);
                }

            }
            if (date != null)
            {
                DateTime dt = DateTime.Parse(date);
                tmpRecord = (List<NoteBookRecord>)tmpRecord.Where(x => DateTime.Parse(x.createDate) > dt);
                List<NoteBookRecord> forDelete = new List<NoteBookRecord>();
                foreach (NoteBookRecord record in tmpRecord)
                {
                    if (DateTime.Parse(record.createDate) >= dt)
                    {
                        forDelete.Add(record);
                    }
                }
                foreach (NoteBookRecord forDel in forDelete)
                {
                    tmpRecord.Remove(forDel);
                }
            }
            return tmpRecord;
        }

        /// <summary>
        /// Загрузка данных из файла по диапазону дат
        /// </summary>
        /// <param name="Path">Путь к файлу</param>
        /// <param name="Date">Дата диапазона (строка)</param>
        /// <param name="author">Автор</param>
        /// <param name="message">сообщение для поиска</param>
        public void load (string Path, string date = null, string author = null, string message = null)
        {
            if (!File.Exists(Path))
            {
                Console.WriteLine($"Файла <{Path}> не существует");
                Console.ReadKey();
                return;
            }
            StreamReader streamReader = new StreamReader(Path);
            string line = "";
            List<NoteBookRecord> tmpRecord = new List<NoteBookRecord>();
            do
            {
                line = streamReader.ReadLine();
                if (line == null || line.Trim() == "")  continue;
                tmpRecord = JsonConvert.DeserializeObject<List<NoteBookRecord>>(line);
                //Console.WriteLine($"line : {line}");
            }
            while (line != null);
            streamReader.Close();

            

            this.allRecords = filter(tmpRecord, date, author, message);

            this.pause($"Загрузка завершена! Загружено : {this.allRecords.Count}") ;
            this.showMainMenu();
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

