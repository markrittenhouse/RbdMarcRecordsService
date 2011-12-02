using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Tasks;
using MarcRecordServiceApp.Tasks.MarcRecords;
using log4net;

namespace MarcRecordServiceApp
{
	public class Program
	{
		private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
            log4net.Config.XmlConfigurator.Configure();
            Log.Debug("Main() >>>");

            Log.DebugFormat("RittenhouseMarcDb: {0}", Settings.Default.RittenhouseMarcDb);

            // list properties
            SortedList sortedProperties = new SortedList();
            foreach (SettingsProperty property in Settings.Default.Properties)
            {
                sortedProperties.Add(property.Name, property.DefaultValue.ToString());
            }
            foreach (string key in sortedProperties.Keys)
            {
                if (sortedProperties[key].ToString() == Settings.Default.GetType().GetProperty(key).GetValue(Settings.Default, null).ToString())
                {
                    Log.DebugFormat("Name: {0}, DefaultValue: {1}", key, sortedProperties[key]);
                }
                else
                {
                    Log.WarnFormat("Name: {0}, DefaultValue: {1}, Value: {2}", key, sortedProperties[key],
                                   Settings.Default.GetType().GetProperty(key).GetValue(Settings.Default, null));
                }
            }
            
            Console.WriteLine("");
            Console.WriteLine("Rittenhouse - MARC Record Service");

			try
            {
                string arg;
                if (args.Length == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("01 = CreateNlmMarcRecords");
                    Console.WriteLine("02 = CreateLcMarcRecords ");
                    Console.WriteLine("03 = CreateRbdMarcRecords");
					Console.WriteLine("");
					Console.Write("Please enter code: ");
                    arg = Console.ReadLine();
                }
                else
                {
                    arg = args[0];
                }


                Log.Info("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
                Log.InfoFormat("arg: {0}", arg);
                Log.Info("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
                Console.WriteLine();

                ITask task = null;
                switch (arg)
                {
                    case "-CreateNlmMarcRecords":
                    case "01":
                        task = new MarcRecordsTask(MarcRecordProvider.Nlm);
                        break;

                    case "-CreateLcMarcRecords":
                    case "02":
                        task = new MarcRecordsTask(MarcRecordProvider.Lc);
                        break;

                    case "-CreateRbdMarcRecords":
                    case "03":
                        task = new MarcRecordsTask(MarcRecordProvider.Rbd);
                        break;

                    default:
                        Console.WriteLine("INVALID AURGUMENT");
                        break;
                }

                if (task != null)
                {
                    try
                    {
                        task.Run();
                        task.TaskResult.CompletedSuccessfully = true;
                        task.TaskResult.RunComments = "Successfully Completed.";

                        foreach (TaskResultStep step in task.TaskResult.Steps)
                        {
                            if (!step.CompletedSuccessfully)
                            {
                                task.TaskResult.CompletedSuccessfully = false;
                                task.TaskResult.RunComments = string.Format("Step {0} failed.", step.Id);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        task.TaskResult.RunComments = string.Format("EXCEPTION: {0}", ex.Message);
                        task.TaskResult.CompletedSuccessfully = false;
                        Log.Error(ex.Message, ex);
                    }
                    finally
                    {
                        task.Cleanup();
                    }

                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(ex.Message, ex);
            }

            Console.WriteLine();
            Log.Debug("Main() <<<");
        }
	}
}
