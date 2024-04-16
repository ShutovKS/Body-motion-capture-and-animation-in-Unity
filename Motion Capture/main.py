# main.py
import socket_server
from image_handler import ImageHandler


def main():
    image_handler = ImageHandler()
    socket_server.start_server(image_handler)


if __name__ == "__main__":
    main()
