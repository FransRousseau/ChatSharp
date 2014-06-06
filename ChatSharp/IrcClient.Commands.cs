using System;
using System.Linq;

namespace ChatSharp
{
	public partial class IrcClient
	{
		/// <summary>
		/// Change current used nick
		/// </summary>
		/// <param name="newNick">The new nick.</param>
		public void Nick( String newNick )
		{
			SendRawMessage( "NICK {0}", newNick );
			User.Nick = newNick;
		}

		/// <summary>
		/// Sends a message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="destinations">The destinations.</param>
		/// <exception cref="System.InvalidOperationException">Message must have at least one target.</exception>
		/// <exception cref="System.ArgumentException">Illegal characters are present in message.;message</exception>
		public void SendMessage( String message, params String[] destinations )
		{
			const String illegalCharacters = "\r\n\0";
			if ( !destinations.Any() ) throw new InvalidOperationException( "Message must have at least one target." );
			if ( illegalCharacters.Any( message.Contains ) ) throw new ArgumentException( "Illegal characters are present in message.", "message" );
			String to = String.Join( ",", destinations );
			SendRawMessage( "PRIVMSG {0} :{1}", to, message );
		}

		/// <summary>
		/// Sends an action message, aka /me command
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="destinations">The destinations.</param>
		public void SendActionMessage( String message, params String[] destinations )
		{
			SendMessage( "\u0001ACTION " + message + "\u0001", destinations );
		}

		/// <summary>
		/// leave a channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <exception cref="System.InvalidOperationException">Client is not present in channel.</exception>
		public void PartChannel( string channel )
		{
			if ( !Channels.Contains( channel ) )
				throw new InvalidOperationException( "Client is not present in channel." );
			SendRawMessage( "PART {0}", channel );
			Channels.Remove( Channels[ channel ] );
		}

		/// <summary>
		/// leave a channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <param name="reason">The reason.</param>
		/// <exception cref="System.InvalidOperationException">Client is not present in channel.</exception>
		public void PartChannel( string channel, string reason )
		{
			if ( !Channels.Contains( channel ) )
				throw new InvalidOperationException( "Client is not present in channel." );
			SendRawMessage( "PART {0} :{1}", channel, reason );
			Channels.Remove( Channels[ channel ] );
		}

		/// <summary>
		/// Joins the channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <exception cref="System.InvalidOperationException">Client is not already present in channel.</exception>
		public void JoinChannel( string channel )
		{
			if ( Channels.Contains( channel ) )
				throw new InvalidOperationException( "Client is not already present in channel." );
			SendRawMessage( "JOIN {0}", channel );
		}

		/// <summary>
		/// Sets the topic of a channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <param name="topic">The topic.</param>
		/// <exception cref="System.InvalidOperationException">Client is not present in channel.</exception>
		public void SetTopic( string channel, string topic )
		{
			if ( !Channels.Contains( channel ) )
				throw new InvalidOperationException( "Client is not present in channel." );
			SendRawMessage( "TOPIC {0} :{1}", channel, topic );
		}

		/// <summary>
		/// Kicks the user from a channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <param name="user">The user.</param>
		public void KickUser( string channel, string user )
		{
			SendRawMessage( "KICK {0} {1} :{1}", channel, user );
		}

		/// <summary>
		/// Kicks the user from a channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <param name="user">The user.</param>
		/// <param name="reason">The reason.</param>
		public void KickUser( string channel, string user, string reason )
		{
			SendRawMessage( "KICK {0} {1} :{2}", channel, user, reason );
		}

		/// <summary>
		/// Invites the user to a channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <param name="user">The user.</param>
		public void InviteUser( string channel, string user )
		{
			SendRawMessage( "INVITE {1} {0}", channel, user );
		}

		public void WhoIs( string nick )
		{
			WhoIs( nick, null );
		}

		public void WhoIs( string nick, Action<WhoIs> callback )
		{
			var whois = new WhoIs();
			RequestManager.QueueOperation( "WHOIS " + nick, new RequestOperation( whois, ro =>
				{
					if ( callback != null )
						callback( ( WhoIs )ro.State );
				} ) );
			SendRawMessage( "WHOIS {0}", nick );
		}

		public void GetMode( string channel )
		{
			GetMode( channel, null );
		}

		public void GetMode( string channel, Action<IrcChannel> callback )
		{
			RequestManager.QueueOperation( "MODE " + channel, new RequestOperation( channel, ro =>
				{
					var c = Channels[ ( string )ro.State ];
					if ( callback != null )
						callback( c );
				} ) );
			SendRawMessage( "MODE {0}", channel );
		}

		public void ChangeMode( string channel, string change )
		{
			SendRawMessage( "MODE {0} {1}", channel, change );
		}

		public void GetModeList( string channel, char mode, Action<MaskCollection> callback )
		{
			RequestManager.QueueOperation( "GETMODE " + mode + " " + channel, new RequestOperation( new MaskCollection(), ro =>
				{
					var c = ( MaskCollection )ro.State;
					if ( callback != null )
						callback( c );
				} ) );
			SendRawMessage( "MODE {0} {1}", channel, mode );
		}
	}
}