# Jabra SDK4 .NET - Device Settings and Properties Sample

This is a .NET sample app demonstrating how to read, write and observe select settings and properties of Jabra devices using the Jabra SDK4 .NET

> 💡 For an introduction and overview of Jabra device integration posibilities, please visit [developer.jabra.com](https://developer.jabra.com).

## How to run

### Run in Visual Studio

1. Clone this repository into a project in Visual Studio (select "Clone Repository..." in the file menu or splash screen).
1. Connect one of the Jabra devices mentioned in `Program.cs` or adjust the code to target a Jabra device you have at hand.
1. Run the project. E.g. by pressing f5 in Visual Studio

## Select device properties

This sample demonstrates interactions with commonly used properties on select devices. If you're looking to interact with other settings, properties or devices, please contact Jabra as described under "Help and issues".

### Jabra Panacast 50 (select properties)

| Property               | Description                                    | Values                                           | Operations    | Triggers device reboot |
| ---------------------- | ---------------------------------------------- | ------------------------------------------------ | ------------- | ---------------------- |
| `zoomMode2`            | Zoom mode.                                     | `fullScreen`, `intelligentZoom`, `activeSpeaker` | read, write   | no                     |
| `roomCapacity`         | Threshold for triggering capacity alerts       | integer                                          | read, write   | no                     |
| `firmwareVersion`      | Version of firmware on device.                 | string                                           | read          | no                     |
| `peopleCount`          | Number of people in field of view.             | integer                                          | read, observe | no                     |
| `roomCapacityExceeded` | Alert when people count exceeds room capacity. | boolean                                          | observe       | no                     |

### Engage 40, Engage 50 and Engage 50 II (select properties)

| Property				 | Description                                         | Values  | Operations  |
| ---------------------- | --------------------------------------------------- | ------- | ----------- |
| `smartRingerEnabled`   | Smart Ringer in Engage 50 II control unit enabled   | boolean | read, write |
| `backgroundNoiseLevel` | Background noise level in dB (in room around agent) | integer | observe     |
| `firmwareVersion`	     | Version of firmware on device.                      | string  | read        |
| `agentSpeaking`        | Agent speaking indicator.                           | boolean | observe     |
| `customerSpeaking`     | Customer speaking indicator.                        | boolean | observe     |
| `audioExposure`        | Sound pressure in dB experienced by headset wearer  | integer | observe     |

## Help and issues

If you have questions or find a bug, please reach out via the support form at [developer.jabra.com](https://developer.jabra.com)
