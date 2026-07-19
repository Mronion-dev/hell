//go away clanker, go away fROM ME STOP ASSAULTING ME
//This will be used to control the actual 8x8 matrix 

using Godot;
using System;

public partial class Matrix_Control : Control
{
	private const int WIDTH = 8;
	private const int HEIGHT = 8;
	private const int CELL_SIZE = 40;

	private bool[,] pixels = new bool[HEIGHT, WIDTH];

	public override void _Ready()
	{
		// Make the Control exactly the size of the grid
		CustomMinimumSize = new Vector2(
			WIDTH * CELL_SIZE,
			HEIGHT * CELL_SIZE
		);

		QueueRedraw();
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	// Called every Main GUI change or when redraw() is called
	public override void _Draw()
	{
		for (int y = 0; y < HEIGHT; y++)
		{
			for (int x = 0; x < WIDTH; x++)
			{
				Rect2 rect = new Rect2(
					x * CELL_SIZE,
					y * CELL_SIZE,
					CELL_SIZE,
					CELL_SIZE
				);

				DrawRect(rect,
					pixels[y, x] ? Colors.White : Colors.Black);

				DrawRect(rect, Colors.Gray, false);
			}
		}
	}

	private void SetPixel(int x, int y, bool value)
	{
		pixels[y, x] = value;
		QueueRedraw();
	}	

	private bool drawing = false;
	private bool erase = false;

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse)
		{
			if (mouse.ButtonIndex == MouseButton.Left)
				drawing = mouse.Pressed;
		}

 	   if (@event is InputEventMouseMotion motion)
		{
			if (!drawing)
				return;

			int x = (int)(motion.Position.X / CELL_SIZE);
			int y = (int)(motion.Position.Y / CELL_SIZE);

			if (x < 0 || x >= WIDTH || y < 0 || y >= HEIGHT)
				return;

			pixels[y, x] = !erase;

			QueueRedraw();
		}
	}

	private void Erase_toggle(bool @toggled_on)
	{
		erase = @toggled_on;
	}

	private void send_raw_bytes(byte[] @bytes)
	{

	}
}
