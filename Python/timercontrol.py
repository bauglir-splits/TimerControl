import time
import keyboard
import requests

# Constants
TIMER_START_KEY = 'f2'
TIMER_STOP_KEY = 'f3'
SERVER_ADDRESS = "https://httpbin.org"
SERVER_PORT = "443"
ENDPOINT = "/post"

ENDPOINT_URL = SERVER_ADDRESS + ":" + SERVER_PORT + ENDPOINT
# End Constants

def TimerStart():
    print("Starting Timer")
    try:
        response = requests.post(ENDPOINT_URL, json={"action": "start"})
        print("Response:", response.status_code)
        time.sleep(1) # Sleep to protect against accidental double press
    except requests.exceptions.RequestException as errex:
        print("Request Exception")
        print(errex.args[0])

def TimerStop():
    print("Stopping Timer")
    try:
        response = requests.post(ENDPOINT_URL, json={"action": "stop"})
        print("Response:", response.status_code)
        time.sleep(1) # Sleep to protect against accidental double press
    except requests.exceptions.RequestException as errex:
        print("Request Exception")
        print(errex.args[0])

keyboard.add_hotkey(TIMER_START_KEY, TimerStart)
keyboard.add_hotkey(TIMER_STOP_KEY, TimerStop)

keyboard.wait('Ctrl+Esc')

keyboard.remove_hotkey(TIMER_START_KEY)
keyboard.remove_hotkey(TIMER_STOP_KEY)
