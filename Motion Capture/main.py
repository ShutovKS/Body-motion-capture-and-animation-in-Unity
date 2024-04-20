# main.py
import threading
import time
import configparser

import cv2

from socket_server import MotionCaptureServer
from motion_capture import MotionCapture


def main():
    config = configparser.ConfigParser()
    config.read('config.ini')
    host = config.get('Server', 'host')
    port = config.getint('Server', 'port')

    server = MotionCaptureServer(host=host, port=port)

    def on_connection_lost():
        print("Событие потери соединения обработано.")
        while True:
            try:
                server.start_server()
                break
            except Exception as e:
                print(f"Попытка повторного подключения: {e}")
                time.sleep(1)

    server.connection_lost.register(on_connection_lost)

    server_thread = threading.Thread(target=server.start_server)
    server_thread.start()

    motion_capture = MotionCapture()

    try:
        # для захвата из локального видео
        while True:  # Зацикливание воспроизведения
            pose_data_generator = motion_capture.video_file_capture('Hot Anime dance.mp4')
            for pose_data in pose_data_generator:
                server.send_data(pose_data)

        # # для захвата из веб-камеры
        # while True:
        #     pose_data = motion_capture.webcam_capture()
        #     if pose_data is not None:
        #         server.send_data(pose_data)
    except KeyboardInterrupt:
        print("Программа остановлена пользователем.")
    finally:
        server.stop_server()
        server_thread.join()
        motion_capture.cam.release()
        cv2.destroyAllWindows()


if __name__ == "__main__":
    main()
