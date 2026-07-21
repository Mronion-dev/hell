import asyncio
import websockets
import time

HOST = "0.0.0.0"
PORT = 8019


async def handler(websocket):
    print(f"\n=== Client Connected ===")
    print(f"Remote: {websocket.remote_address}")

    start = time.perf_counter()
    packets = 0
    bytes_received = 0

    try:
        async for message in websocket:

            packets += 1

            if isinstance(message, bytes):
                bytes_received += len(message)

                print("\nBinary Packet")
                print(f"Length : {len(message)} bytes")
                print("HEX    :", message.hex(" ").upper())

                if len(message) == 8:
                    print("\nMatrix:")
                    for b in message:
                        row = format(b, "08b")
                        print(row.replace("0", ".").replace("1", "#"))

            else:
                bytes_received += len(message)

                print("\nText Packet")
                print(message)

            elapsed = time.perf_counter() - start

            if elapsed >= 1.0:
                print("\n----------------------")
                print(f"FPS: {packets/elapsed:.2f}")
                print(f"Bandwidth: {bytes_received/elapsed:.1f} B/s")
                print("----------------------")

                packets = 0
                bytes_received = 0
                start = time.perf_counter()

    except websockets.ConnectionClosed as e:
        print("\nConnection closed")
        print(e)


async def main():
    print(f"Hosting websocket on ws://localhost:{PORT}/")

    async with websockets.serve(handler, HOST, PORT):
        await asyncio.Future()


asyncio.run(main())