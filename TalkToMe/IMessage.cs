namespace TalkToMe
{
	using ChatSharp;

	interface IMessage
	{
		string Source { get; }
		string User { get; }
		string Message { get; }
	}

	public class PmWrapper : IMessage
	{
		public PmWrapper(PrivateMessage message)
		{
			this.Source = message.Source;
			this.User = message.User.Nick;
			this.Message = message.Message;
		}

		public string Source { get; }
		public string User { get; }
		public string Message { get; }
	}

	public class FakePm : IMessage
	{
		public FakePm(string source, string user, string message)
		{
			this.Source = source;
			this.User = user;
			this.Message = message;
		}

		public string Source { get; }
		public string User { get; }
		public string Message { get; }
	}
}