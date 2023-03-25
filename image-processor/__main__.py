import socket
import imutils
import cv2
import numpy as np


def rescale_frame(frame, percent=75):
    width = int(frame.shape[1] * percent/ 100)
    height = int(frame.shape[0] * percent/ 100)
    dim = (width, height)
    return cv2.resize(frame, dim, interpolation =cv2.INTER_AREA)

def client():
    host = "127.0.0.1"
    port = 5710

    client_socket = socket.socket()
    client_socket.connect((host, port))
    
    print("Connected to simulator!")
    
    width  = 1280
    height = 720

    vid = cv2.VideoCapture()
    vid.open(0 + cv2.CAP_DSHOW)
    vid.set(cv2.CAP_PROP_FRAME_WIDTH, width)
    vid.set(cv2.CAP_PROP_FRAME_HEIGHT, height)
    vid.set(cv2.CAP_PROP_FPS, 60)    
    
    lower_red = np.array([0, 70, 50])
    upper_red = np.array([10, 255, 255])

    box_size = 400
    box_start = (50, int(height / 2 - box_size / 2))
    box_end = (width - 50, int(height / 2 + box_size / 2) )
    
    while(True):

        ret, frame = vid.read()
        hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
        
        mask = cv2.inRange(hsv, lower_red, upper_red)
        _, contours = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

        for j, contour in enumerate(contours):
            bbox = cv2.boundingRect(contour)
            contour_mask = np.zeros_like(mask)
            cv2.drawContours(contour_mask, contours, j, 255, -1)

        frame = cv2.rectangle(frame, box_start, box_end, (255, 0, 0), 3)
        

        output = rescale_frame(mask, percent=50)
        frame_small = rescale_frame(frame, percent=50)

        cv2.imshow('output', output)
        cv2.imshow('frame', frame_small)

        if cv2.waitKey(5) & 0xFF == ord('q'):
            break

    
    # After the loop release the cap object
    vid.release()
    # Destroy all the windows
    cv2.destroyAllWindows()

    client_socket.close()


if __name__ == '__main__':
    client()