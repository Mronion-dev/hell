using Godot;
using System;

public partial class Container : VBoxContainer
{

	WeebSocket WebSocket = new WeebSocket();
	private LineEdit IPA;
	private Label IPS;
	private bool ready = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IPA = GetNodeOrNull<LineEdit>("IPAI");
		IPS = GetNodeOrNull<Label>("IPSN");
		ready = true;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(ready == false)
		{
			return;
		}
		if(IPS != null)
		{
			IPS.Text = WebSocket.Connection_Status();
		}
	}

	public void ConnectToIP()
	{
		WebSocket.ServerAddress = IPA.Text;
		WebSocket.Connect();
	}

}
