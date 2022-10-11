using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleAppJson
{
    public class Student
    {
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "course")]
        public int course { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double rating { get; set; }
        public List<double> ratings { get; set; }
    }

    public class StudentList
    {
        public List<Student> studentList { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            #region Можливість №1. Зчитування/запис даних у вигляді типізованих/анонімних класів 

            // Приклад зчитування даних масиву, який подано у текстовому форматі JSON
            var jsonArray = "[1,2,3]";
            // Для того щоб перетіорити рядкові дані JSON у змінну масив використовуємо JsonConvert.DeserializeObject
            var array = JsonConvert.DeserializeObject<int[]>(jsonArray);
            // Після перетворення дані обробляються як масив у C#
            foreach (var value in array)
            {
                Console.WriteLine(value);
            }

            // Приклад перетворення складного обєкту
            var stringJsonData = @"
            {
                'studentList': [
                    {
                      'fullName': 'Petrenko Petro',
                      'course': 2,
                      'rating': 90,
                      'ratings': [67, 78, 89]
                    },
                    {
                      'fullName': 'Ivanenko Ivan',
                      'course': 1,
                      'rating': 75,
                      'ratings': [67, 78, 89],
                      'books': [{'bookTitle':'Title 1', 'autor': 'autor'}]
                    }
                ]
            }";
            var students = JsonConvert.DeserializeObject<StudentList>(stringJsonData);

            // Після перетворення дані обробляються як відповідний обєкт у C#
            foreach(var student in students.studentList)
            {
                Console.WriteLine(string.Concat("Hi ", student.FullName, " " + student.course));
            }

            // Також можна конвертувати JSON-дані безпосередньо із файлу
            var studentsFromFile = JsonConvert.DeserializeObject<StudentList>(File.ReadAllText(@"C:\Users\antosp\source\repos\ConsoleAppJson\ConsoleAppJson\JsonFile1.json"));

            // При конвертуванні JSON-даних можна використовувати динамічний тип як тип результату конвертування
            var dynamicStudens = JsonConvert.DeserializeObject<dynamic>(stringJsonData);
            foreach (var student in dynamicStudens.studentList)
            {
                Console.WriteLine(string.Concat("Hi dynamic ", student.fullName, " " + student.course));
            }

            // запис даних у форматі json
            var serializedOcject = JsonConvert.SerializeObject(students);
            Console.WriteLine(serializedOcject);

            // зберегти в файл
            // serialize JSON to a string and then write string to a file
            File.WriteAllText(@"C:\Users\antosp\source\repos\ConsoleAppJson\ConsoleAppJson\JsonOutputFile.json", serializedOcject);
            #endregion

            // Можливість №2. Зчитування/запис даних у вигляді словників
            var simpleStrObject = "{firstName: 'Ivan', lastName: 'Ivanenko'}";
            var jObject = JObject.Parse(simpleStrObject);
            Console.WriteLine("firstName : {0}", jObject["firstName"]);
            Console.WriteLine("lastName : {0}", jObject["lastName"]);

            var jObjectStudentList = JObject.Parse(stringJsonData);
            var studentListValue = jObjectStudentList["studentList"];
            var list = studentListValue.Select(t => new 
            {
                FullName = t["fullName"].ToString(),
                course = Convert.ToInt32(t["course"]),
                rating = Convert.ToDouble(t["rating"]),
                ratings = t["ratings"].Select(r => Convert.ToDouble(r)),
                books = t["books"]?.Select(book => new
                {
                    title = Convert.ToString(book["title"]),
                    autor = Convert.ToString(book["autor"]),
                })
                .ToList()
            });

            foreach(var student in list)
            {
                Console.WriteLine(string.Join(",", student.FullName));
            }

            Console.ReadKey();
        }
    }
}
