# image_handler.py
import cv2
from cvzone.PoseModule import PoseDetector


class ImageHandler:
    def __init__(self):
        self.img = None
        self.detector = PoseDetector()
        self.cap = cv2.VideoCapture(0)

    def process_frame(self):
        success, img = self.cap.read()
        if not success:
            print("Не удалось прочитать кадр. Проверьте подключение веб-камеры.")
            return None

        self.img = self.detector.findPose(img)
        lmList, bboxInfo = self.detector.findPosition(img)

        if bboxInfo:
            for lm in lmList:
                lm[1] = img.shape[0] - lm[1]

        self.print_curent_image_from_camera()

        return lmList

    def print_curent_image_from_camera(self):
        cv2.imshow("Image", self.img)
        cv2.waitKey(1)
