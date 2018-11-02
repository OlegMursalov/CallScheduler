using Microsoft.Win32.TaskScheduler;
using System;
using System.IO;
using System.Text;

namespace StarterCallFromTaskScheduler
{
    class Program
    {
        static void Main(string[] arguments)
        {
            var messageForLog = new StringBuilder();
            var array = arguments[0].Split(':');
            var data = new { NameTask = array[0], CallId = new Guid(array[1]) };
            try
            {
                messageForLog.AppendLine($"Задача {data.NameTask} запущена планировщиком Windows в {DateTime.Now}");
                
                // Совершается запрос на наш сервис "B", развернутый в IIS

                messageForLog.AppendLine($"Запущен обзвон [{data.CallId}]");
                using (var ts = new TaskService())
                {
                    ts.RootFolder.DeleteTask(data.NameTask);
                    messageForLog.AppendLine($"Задача {data.NameTask} выполнена и удалена из планировщика Windows");
                }
            }
            catch (Exception ex)
            {
                messageForLog.AppendLine($"При выполнении задачи {data.NameTask} возникла ошибка: [{ex.Message}]");
            }
            // В finally не перехватывает, но, возможно, просто доступа нет к файловой системе
            // Может, задачу в планировщике надо ставить от какой-то учетки
            File.AppendAllText("Log.txt", $"{messageForLog.ToString()}{Environment.NewLine}");
        }
    }
}