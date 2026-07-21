using Godot;

public partial class WeebSocket : Node
{
	private WebSocketPeer _socket = new();

	[Export]
	public string ServerAddress = "ws://192.168.10.47/ws";
	private bool IsConected = false;

	public override void _Ready()
	{
		GD.Print($"WebSocket Initialized at Inst. ID {GetInstanceId()}");
		GD.Print($"WebSocket._socket Initialized at Inst. ID {_socket.GetInstanceId()}");
	}

	public void Connect()
	{
		if (_socket.GetReadyState() != WebSocketPeer.State.Closed)
			return;

		Error err = _socket.ConnectToUrl(ServerAddress);
		if(err == Error.Ok)
		{
			IsConected = true;
			GD.Print("Connection returns ok");
		}
	}

	public override void _Process(double delta)
	{
		if(!IsConected)
		{
			return;
		}
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
	}

	public void SendText(string text)
	{
		if (!Connected)
			return;

		_socket.SendText(text);
	}

	public string Connection_Status()
{
	return $"{_socket.GetReadyState()} @ {ServerAddress}\n connected returns {Connected} and _socket id is {_socket.GetInstanceId()}";
}
}
