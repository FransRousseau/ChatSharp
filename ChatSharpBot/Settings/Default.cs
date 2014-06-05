using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace ChatSharpBot.Settings
{
	internal static class Default
	{
		public static String[] Owners { get; private set; }

		public static String[] Channels { get; private set; }

		public static String Nick { get; private set; }

		public static String LogPath { get; private set; }

		public static String ServerAddress { get; private set; }

		private static NameValueCollection _settings = null;

		static Default()
		{
			_settings = ( NameValueCollection )ConfigurationManager.GetSection( "Bot.Settings" );

			Owners = _settings[ "Owners" ].Split( "|,;".ToArray(), StringSplitOptions.RemoveEmptyEntries );
			Channels = _settings[ "Channels" ].Split( "|,;".ToArray(), StringSplitOptions.RemoveEmptyEntries );
			Nick = String.IsNullOrEmpty( _settings[ "Nick" ] ) ? "ChatBot" : _settings[ "Nick" ].Trim();
			LogPath = String.IsNullOrEmpty( _settings[ "LogPath" ] ) ? "." : _settings[ "LogPath" ].Trim();
			ServerAddress = _settings[ "ServerAddress" ].Trim();
		}
	}
}