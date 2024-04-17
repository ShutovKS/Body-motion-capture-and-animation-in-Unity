# main.py
import socket_server
from motion_capture import MotionCapture


def main():
    motion_capture = MotionCapture()
    socket_server.start_server(motion_capture.webcam_capture)
    # while True:
    #     motion_capture.webcam_capture()


if __name__ == "__main__":
    main()
