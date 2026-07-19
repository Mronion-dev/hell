using Godot;

public partial class WeebSocket : Node
{
	private readonly WebSocketPeer _socket = new();

	[Export]
	public string ServerAddress = "ws://localhost:8080";

	public void Connect()
	{
		Error err = _socket.ConnectToUrl(ServerAddress);
		GD.Print($"Connect returned: {err}");
		return;
	}

	public override void _Process(double delta)
	{
		_socket.Poll();

		switch (_socket.GetReadyState())
		{
			case WebSocketPeer.State.Connecting:
				break;

			case WebSocketPeer.State.Open:
				while (_socket.GetAvailablePacketCount() > 0)
				{
					var packet = _socket.GetPacket();

					if (_socket.WasStringPacket())
						GD.Print(packet.GetStringFromUtf8());
					else
						GD.Print($"Received {packet.Length} bytes");
				}
				break;

			case WebSocketPeer.State.Closing:
				break;

			case WebSocketPeer.State.Closed:
				break;
		}
	}

	public bool Connected =>
		_socket.GetReadyState() == WebSocketPeer.State.Open;

	public void SendFrame(byte[] @frame)
	{
		if (!Connected)
		{
			return("not connected");
		}

		_socket.Send(frame);
	}

	public void SendText(string text)
	{
		if (!Connected)
			return;

		_socket.SendText(text);
	}
}
