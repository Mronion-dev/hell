using Godot;

public partial class WeebSocket : Node
{
	private readonly WebSocketPeer _socket = new();

	[Export]
	public string ServerAddress = "ws://192.168.10.47/ws";

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
			return;
		}

		_socket.Send(frame);
		GD.Print("frame sent");
	}

	public void SendText(string text)
	{
		if (!Connected)
			return;

		_socket.SendText(text);
	}

	public string Connection_Status()
	{
		if (!Connected)
		{
			return($"Null @{ServerAddress}");
		}
		else
		{
			return($"True @{ServerAddress}");
		}
		
	}
}
