# socket_server.py
import socket
import threading
from event import Event


class MotionCaptureServer:
    def __init__(self, host='localhost', port=50237):
        self.host = host
        self.port = port
        self.server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.server_socket.bind((self.host, self.port))
        self.server_socket.listen(1)
        self.running = False
        self.client_socket = None
        self.connection_lost = Event()

    def start_server(self):
        self.running = True
        threading.Thread(target=self.connected).start()

    def connected(self):
        while self.running:
            try:
                self.client_socket, addr = self.server_socket.accept()
                print(f"Подключение от {addr}")
                self.send_data("Привет, клиент!")
            except Exception as e:
                print(f"Соединение прервано: {e}")
                self.on_connection_lost()
                break

    def send_data(self, data):
        if self.client_socket:
            try:
                self.client_socket.send(str(data).encode())
                print(f"Данные отправлены: {data}")
            except Exception as e:
                print(f"Ошибка при отправке данных: {e}")
                self.on_connection_lost()

    def on_connection_lost(self):
        print("Соединение разорвано.")
        if self.client_socket:
            self.client_socket.close()
            self.client_socket = None
        self.connection_lost.trigger()

    def stop_server(self):
        self.running = False
        self.server_socket.close()
        print("Сервер остановлен.")
