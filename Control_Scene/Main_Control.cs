using Godot;
using System;

//I have to reqrite all of my shit in here in another way, this truly is hell :sob:
public partial class Main_Control : Control
{

	WeebSocket WebSocket;
	private LineEdit IPA;
	private RichTextLabel IPS;
	private bool ready = false;
	private Matrix_Control Matriks_Control;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		WebSocket = GetNodeOrNull<WeebSocket>("/root/WeebSocket");
		IPA = GetNodeOrNull<LineEdit>("VBoxContainer/IPAI");
		IPS = GetNodeOrNull<RichTextLabel>("IPCS");
		Matriks_Control = GetNodeOrNull<Matrix_Control>("Matrix_Control");

		ready = true;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//idk what to do here
	}

	private void _ConnectToIP()
	{
		WebSocket.ServerAddress = IPA.Text;
		WebSocket.Connect();
		GD.Print("Trying to connect...");
	}
	private byte[] GetFrameBytes()
	{
		byte[] frame = new byte[8];

		for (int y = 0; y < 8; y++)
		{
			byte row = 0;

			for (int x = 0; x < 8; x++)
			{
				if (Matrix_Control.pixels[y, x])
				{
					row |= (byte)(1 << (7 - x));
	 		   }
			}

			frame[y] = row;
		}

		return frame;
	}

	private void _timeout()
	{
		IPS.Text = WebSocket.Connection_Status();
		WebSocket.SendFrame(GetFrameBytes());
	}

}
