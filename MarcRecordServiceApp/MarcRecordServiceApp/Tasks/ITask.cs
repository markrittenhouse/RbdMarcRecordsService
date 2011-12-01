using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarcRecordServiceApp.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Tasks
{
	public interface ITask
	{
		TaskResult TaskResult { get; }

		void Run();

		void Cleanup();
	}
}
