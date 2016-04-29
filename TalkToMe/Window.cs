namespace TalkToMe
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using ChatSharp;

	// One window per server?
	// Possibly more per server I guess, although that
	// won't work with the same user, so you'd need several
	// accounts.
	public class Window
	{
		private IrcClient client;
		private bool connected;
		private readonly Dictionary<string, List<IMessage>> history;
		private string activeChannel;
		private List<string> channels;

		public Window()
		{
			this.history = new Dictionary<string, List<IMessage>>();
			this.channels = new List<string>();
			this.connected = false;
		}

		public void ConnectTo(string server, IrcUser user)
		{
			this.client = new IrcClient(server, user);

			this.client.MOTDRecieved += (s, e) =>
			{
				Console.Write(e.MOTD);
			};

			this.client.ChannelMessageRecieved += (s, e) =>
			{
				var msg = new PmWrapper(e.PrivateMessage);
				this.Log(msg);
				this.Print(msg);
			};

			this.client.RawMessageSent += (s, e) =>
			{
			};

			this.client.ConnectionComplete += (s, e) =>
			{
				Console.WriteLine("Connected.");
				this.connected = true;
			};

			Console.WriteLine($"Connecting to {server} as {user.Nick}...");
			this.client.ConnectAsync();
		}

		public void JoinChannel(string channel)
		{
			this.activeChannel = channel;
			this.channels.Add(channel);
			this.client.JoinChannel(channel);
			Console.WriteLine($"Joined {channel}.");
		}

		public void SendMessage(string message)
		{
			this.client.SendMessage(message, this.activeChannel);

			var msg = new FakePm(this.activeChannel, this.client.User.Nick, message);
			this.Log(msg);
			this.Print(msg);
		}

		public void WaitForConnection()
		{
			while (!connected)
			{
				Thread.Sleep(100);
			}
		}

		private void Log(IMessage message)
		{
			var channel = this.client.Channels[message.Source];

			if (!this.history.ContainsKey(channel.Name))
			{
				this.history.Add(channel.Name, new List<IMessage>());
			}

			this.history[channel.Name].Add(message);
		}

		private void Print(IMessage message)
		{
			Console.WriteLine($"[{message.Source}] {message.User}: {message.Message}");
		}
	}
}