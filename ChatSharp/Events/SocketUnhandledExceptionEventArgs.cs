using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatSharp.Events
{
	public class SocketUnhandledExceptionEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the exception object.
		/// </summary>
		/// <value>
		/// The exception object.
		/// </value>
		public Exception ExceptionObject { get; private set; }


		public SocketUnhandledExceptionEventArgs( Exception exception )
		{
			this.ExceptionObject = exception;
		}

	}
}
