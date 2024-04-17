# image_handler.py
import cv2
from cvzone.PoseModule import PoseDetector


class MotionCapture:
    def __init__(self):
        self.img = None
        self.detector = PoseDetector()
        self.cam = cv2.VideoCapture(0)

    def webcam_capture(self):
        success, img = self.cam.read()
        if not success:
            print("Не удалось прочитать кадр. Проверьте подключение веб-камеры.")
            return None

        return self.__handler(img)

    def __handler(self, img):
        self.img = self.detector.findPose(img)
        lmList, boxInfo = self.detector.findPosition(img)

        if boxInfo:
            for lm in lmList:
                lm[1] = img.shape[0] - lm[1]

        self.__print_current_image_on_screen()

        return lmList

    def __print_current_image_on_screen(self):
        cv2.imshow("Image", self.img)
        cv2.waitKey(1)
