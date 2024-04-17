# motion_capture.py
import cv2
from cvzone.PoseModule import PoseDetector


class MotionCapture:
    def __init__(self):
        self.img = None
        self.detector = PoseDetector()
        self.cam = cv2.VideoCapture(0)

    def webcam_capture(self):
        # Стриминг видео с вебкамеры
        success, img = self.cam.read()
        if not success:
            print("Не удалось прочитать кадр. Проверьте подключение веб-камеры.")
            return None

        return self.__handler(img)

    def video_file_capture(self, video_path):
        # Обработка локального видео
        self.cam = cv2.VideoCapture(video_path)
        success, img = self.cam.read()
        if not success:
            print(f"Не удалось прочитать кадр из файла {video_path}. Проверьте путь к файлу.")
            return None

        return self.__handler(img)

    def image_file_capture(self, image_path):
        # Обработка локального изображения
        img = cv2.imread(image_path)
        if img is None:
            print(f"Не удалось прочитать изображение из файла {image_path}. Проверьте путь к файлу.")
            return None

        return self.__handler(img)

    def video_stream_capture(self, video_url):
        # Стриминг видео с URL
        self.cam = cv2.VideoCapture(video_url)
        success, img = self.cam.read()
        if not success:
            print(f"Не удалось прочитать кадр из потока {video_url}. Проверьте URL.")
            return None

        return self.__handler(img)

    def ip_camera_capture(self, ip_address):
        # Использование камеры с IP-адресом
        self.cam = cv2.VideoCapture(f"rtsp://{ip_address}")
        success, img = self.cam.read()
        if not success:
            print(f"Не удалось прочитать кадр с камеры IP {ip_address}. Проверьте IP-адрес.")
            return None

        return self.__handler(img)

    def rtsp_stream_capture(self, rtsp_url):
        # Обработка видео с RTSP-потока
        self.cam = cv2.VideoCapture(rtsp_url)
        success, img = self.cam.read()
        if not success:
            print(f"Не удалось прочитать кадр из RTSP потока {rtsp_url}. Проверьте URL.")
            return None

        return self.__handler(img)

    def usb_camera_capture(self, device_index=0):
        # Использование камеры с поддержкой USB
        self.cam = cv2.VideoCapture(device_index)
        success, img = self.cam.read()
        if not success:
            print(f"Не удалось прочитать кадр с USB камеры. Проверьте подключение камеры.")
            return None

        return self.__handler(img)

    def __handler(self, img):
        try:
            self.img = self.detector.findPose(img)
            lmList, boxInfo = self.detector.findPosition(img)

            if boxInfo:
                for lm in lmList:
                    lm[1] = img.shape[0] - lm[1]

            self.__print_current_image_on_screen()

            return lmList
        except Exception as e:
            print(f"Ошибка при обработке изображения: {e}")
            return None

    def __print_current_image_on_screen(self):
        cv2.imshow("Image", self.img)
        cv2.waitKey(1)
