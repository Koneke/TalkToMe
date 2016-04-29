using System.Collections.Generic;

namespace TalkToMe
{
	using System;

	class AppInput
	{
		private enum State
		{
			AwaitingCommand,
			Command
		};

		private static Dictionary<char, Type> bindings = new Dictionary<char, Type>
		{
			{ 'j', typeof(JoinCommand) },
			{ 'q', typeof(QuitCommand) },
			{ 't', typeof(TestCommand) },
		};

		private App app;
		private IUiCommand current;
		private State state;

		public AppInput(App app)
		{
			this.app = app;
			this.state = State.AwaitingCommand;
		}

		public void Start()
		{
			while (!app.IsQuitting)
			{
				ConsoleKeyInfo key;
				if (Console.KeyAvailable)
				{
					key = Console.ReadKey(true);
				}
				else
				{
					continue;
				}

				if (current != null && current.IsFinished())
				{
					this.current = null;
					this.state = State.AwaitingCommand;
				}

				switch (this.state)
				{
					case State.AwaitingCommand:
						if (bindings.ContainsKey(key.KeyChar))
						{
							this.current = Activator.CreateInstance(bindings[key.KeyChar], this.app) as IUiCommand;

							if (this.current == null)
							{
								throw new Exception();
							}

							this.state = State.Command;
							this.current.Start();
						}
						break;

					case State.Command:
						current.TakeKey(key);
						break;

					default:
						throw new Exception();
				}
			}
		}
	}
}