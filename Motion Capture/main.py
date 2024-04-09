# main.py
from socket_server import start_server
from image_handler import ImageHandler


def main():
    image_handler = ImageHandler()
    start_server(image_handler)


if __name__ == "__main__":
    main()
