namespace TalkToMe
{
	using ChatSharp;

	public class App
	{
		private IrcUser user;
		private Window activeWindow;

		public bool IsQuitting { get; private set; }

		public void Start()
		{
			this.IsQuitting = false;
			this.user = new IrcUser("KonekeDev", "KonekeDev");
			this.activeWindow = new Window();
			this.activeWindow.ConnectTo("irc.freenode.net", user);
			this.activeWindow.WaitForConnection(); // spins until we're done.
		}

		public void Join(string channel)
		{
			this.activeWindow.JoinChannel(channel);
		}

		public void SendMessage(string message)
		{
			this.activeWindow.SendMessage(message);
		}

		public void Quit()
		{
			this.IsQuitting = true;
		}
	}
}