# socket_server.py
import socket


def start_server(image_handler):
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind(('localhost', 12345))
    server_socket.listen(1)
    print("Сервер запущен, ожидание подключения...")

    while True:
        try:
            client_socket, addr = server_socket.accept()
            print(f"Подключение от {addr}")

            while True:  # Цикл для обработки и отправки данных
                data = image_handler.process_frame()
                if data is not None:
                    client_socket.send(str(data).encode())
                    print(str(data))
                else:
                    print("Нет данных для отправки.")

        except Exception as e:
            print(f"Соединение прервано: {e}")
            print("Попытка восстановить соединение...")
            continue

    server_socket.close()
