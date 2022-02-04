# Playout Manager
Caspar CG Client with Scheduling Capabilities

![image](https://user-images.githubusercontent.com/73527278/152359130-13aeff93-137b-4491-881f-be1e4ebdd28d.png)

This project is still currently under developement and is not to be used in production.

Currently crashes happen during adding media if it has a file name with non standard characters, or if something goes wrong whilst trying to retrieve media info.
However it has been tested with running a playlist for 12 hours and worked fine.

Feel free to suggest edits or code on top of this, just send me an email at thomas@creativityfilms.gr

# Quick start guide
## Connecting to the server
When opening the application you will be greeted with the main screen

![image](https://user-images.githubusercontent.com/73527278/152435177-67a861cf-1aa5-4261-b3b3-35a9418d4f8d.png)

Before connecting to a server make sure that you have both the casparcg.exe server running, but also the scanner.exe application running.

In the bottom right corner type in the IP address of the server (or use localhost if the server is on the same machine) and click the Connect icon.
If you've connected sucessfully the icon will go green and the main controls will appear, otherwise the connect icon will turn red and an error will be displayed in the status bar

![image](https://user-images.githubusercontent.com/73527278/152435556-ebb3083d-01cd-4265-be63-91fa363249f6.png)

The main controls are at the bottom of the window, including a channel selection menu.
Playout manager supports up to 3 casparcg channels, one for media, one for CGs and one for previewing.
These can also all be set as one channel and should self-key correctly (all media goes to layer 10, and CGs default to layer 20)
If your preview channel is also set to the playout or cg channels, previewing will interrupt playback if the "Enable Preview" checkbox is checked. This is why this checkbox is unchecked by default

## Adding media items to the rundown
To add a media item or a cg template to the rundown, click on the + icon on the toolbar at the bottom, a pop up will appear:

![image](https://user-images.githubusercontent.com/73527278/152435966-5551390e-6a90-4bb0-a474-db4de4de0536.png)

(more on this guide will be written soon)
