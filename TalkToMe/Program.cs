namespace TalkToMe
{
	using System;
	using ChatSharp;

	class Program
	{
		private static void Main(string[] args)
		{
			var app = new App();
			var input = new AppInput(app);
			app.Start();
			input.Start();
		}
	}
}