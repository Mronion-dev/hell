using Godot;
using System;

public partial class Container : VBoxContainer
{

	private WeebSocket WebSocket;
	private LineEdit IPA;
	private Label IPS;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IPA = GetNode<LineEdit>("IPAI");
		IPS = GetNode<Label>("IPSN")

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void Connect()
	{
		WebSocket.ServerAddress = IPA.Text;
		WebSocket.Connect();
		return;
	}
}
