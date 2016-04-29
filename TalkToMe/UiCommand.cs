namespace TalkToMe
{
	using System;

	public enum UiCommandType
	{
		Join
	}

	public interface IUiCommand
	{
		void Start();
		void TakeKey(ConsoleKeyInfo key);
		bool IsFinished();
	}

	public abstract class UiCommandBase : IUiCommand
	{
		protected bool finished;
		protected App app;

		protected UiCommandBase(App app)
		{
			this.finished = false;
			this.app = app;
		}

		public abstract void Start();
		public abstract void TakeKey(ConsoleKeyInfo key);
		public virtual bool IsFinished()
		{
			return this.finished;
		}

		protected virtual void Finish()
		{
			this.finished = true;
		}
	}

	public class QuitCommand : UiCommandBase
	{
		public QuitCommand(App app) : base(app)
		{
			finished = true;
		}

		public override void Start()
		{
			this.app.Quit();
		}

		public override void TakeKey(ConsoleKeyInfo key)
		{
			throw new NotImplementedException();
		}
	}

	public class TestCommand : UiCommandBase
	{
		public TestCommand(App app) : base(app)
		{
		}

		public override void Start()
		{
			this.app.SendMessage("test message please ignore");
			this.finished = true;
		}

		public override void TakeKey(ConsoleKeyInfo key)
		{
			throw new NotImplementedException();
		}
	}

	public abstract class TextCommand : UiCommandBase
	{
		protected string current;

		protected TextCommand(App app) : base(app)
		{
			this.current = "";
		}

		protected abstract void Execute();

		public override void TakeKey(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.Enter:
					Console.WriteLine();
					this.Execute();
					this.Finish();
					break;

				case ConsoleKey.Escape:
					Console.WriteLine(" {Cancelled command}");
					this.Finish();
					break;

				case ConsoleKey.Backspace:
					if (this.current.Length <= 0)
					{
						break;
					}

					this.current = this.current.Substring(0, this.current.Length - 1);
					Console.CursorLeft -= 1;
					Console.Write(' ');
					Console.CursorLeft -= 1;
					break;

				default:
					if (ValidInput(key))
					{
						var c = (key.Modifiers & ConsoleModifiers.Shift) == ConsoleModifiers.Shift
							? char.ToUpper(key.KeyChar)
							: char.ToLower(key.KeyChar);

						current += c;
						Console.Write(c);
					}
					break;
			}
		}

		private bool ValidInput(ConsoleKeyInfo key)
		{
			if (key.Key >= ConsoleKey.A && key.Key <= ConsoleKey.Z)
			{
				return true;
			}

			if (key.Key >= ConsoleKey.D0 && key.Key <= ConsoleKey.D9)
			{
				return true;
			}

			if (key.Key == ConsoleKey.OemPeriod)
			{
				return true;
			}

			return false;
		}
	}

	public class JoinCommand : TextCommand 
	{
		public JoinCommand(App app) : base(app)
		{
		}

		public override void Start()
		{
			Console.Write("Join: ");
		}

		protected override void Execute()
		{
			this.app.Join(this.current);
		}
	}
}