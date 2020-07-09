using DiscordRPC;
using VoltstroEngine.Core.Logging;

namespace DiscordRPCExample
{
	public sealed class DiscordManager
	{
		private DiscordRpcClient client;

		public string ApplicationId = "730349973610430521";

		public void Init()
		{
			client = new DiscordRpcClient(ApplicationId, -1, new VEDiscordLogger());

			client.OnError += (sender, args) => Logger.Error("Error with Discord RPC: {@Code}:{@Message}", args.Code, args.Message);
			client.OnReady += (sender, args) => Logger.Info("Client ready: " + args.User.Username);
			client.OnConnectionFailed += (sender, args) =>
			{
				Logger.Error("Error communicating with Discord: Pipe: `{@FailedPipe}`. Is Discord running?", args.FailedPipe);
				client.Deinitialize();
			};
			client.OnClose += (sender, args) => Logger.Info($"Discord RPC was closed: {args.Code}:{args.Reason}");

			client.Initialize();
		}

		public void Shutdown()
		{
			client.Dispose();
		}

		public void OnUpdate()
		{
			client.Invoke();
		}

		public void SetRichPresence(RichPresence presence)
		{
			if(!client.IsInitialized) return;

			client.SetPresence(presence);
		}
	}
}