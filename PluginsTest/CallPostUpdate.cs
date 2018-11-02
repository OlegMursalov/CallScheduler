using Microsoft.Xrm.Sdk;
using System;
using Microsoft.Win32.TaskScheduler;
using System.Windows.Forms;

namespace PluginsTest
{
    public class CallPostUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = PluginHelper.GetContext(serviceProvider);
            var call = PluginHelper.GetTarget<new_call>(context);
            if (call != null && call.new_StartDialing.HasValue)
            {
                try
                {
                    var sd = call.new_StartDialing.Value;
                    var ld = sd.ToLocalTime();
                    using (var ts = new TaskService())
                    {
                        var callId = call.Id.ToString();
                        var nameTask = $"Call[{callId}]-[{ld.Day}.{ld.Month}.{ld.Year}_{ld.Hour}.{ld.Minute}.{ld.Second}]";
                        var td = ts.NewTask();
                        td.RegistrationInfo.Description = nameTask;
                        td.Triggers.Add(new TimeTrigger
                        {
                            StartBoundary = sd
                        });
                        td.Actions.Add(pathToStarterCallFromTaskScheduler, $"{nameTask}:{callId}");
                        ts.RootFolder.RegisterTaskDefinition(nameTask, td);
                        // MessageBox.Show($"Задача {nameTask} успешно зарегестрирована в планировщике Windows");
                    }
                }
                catch (Exception ex)
                {
                    var message = $"Произошла ошибка при попытке зарегестрировать задачу в планировщике Windows: [{ex.Message}]";
                    throw new InvalidPluginExecutionException(message);
                }
            }
        }

        private const string pathToStarterCallFromTaskScheduler = @"C:\Test\StarterCallFromTaskScheduler.exe";
    }
}