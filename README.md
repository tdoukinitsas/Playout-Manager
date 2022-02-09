# Playout Manager
![PLAYOUTMANAGER-ID_1_1](https://user-images.githubusercontent.com/73527278/153185981-6e4e6d6a-10e2-4880-aedb-533a50afc327.gif)

Caspar CG Client with Scheduling Capabilities

![image](https://user-images.githubusercontent.com/73527278/152359130-13aeff93-137b-4491-881f-be1e4ebdd28d.png)

This project is still currently under developement and is not to be used in production.

Currently crashes happen during adding media if it has a file name with non standard characters, or if something goes wrong whilst trying to retrieve media info.
However it has been tested with running a playlist for 12 hours and worked fine.

Feel free to suggest edits or code on top of this, just send me an email at thomas@creativityfilms.gr

# Issues
There are still some bugs and features that don't work, for further reference visit the issues page:
https://github.com/tdoukinitsas/Playout-Manager/issues

# Quick start guide

## Downloading
Playout Manager currently does not need to be installed and can run as a portable app.

Download and from the pre-built binaries found here:
https://github.com/tdoukinitsas/Playout-Manager/releases

Alternatively you can build your own version using Visual Studio 2019 with C# and WPF modules installed

You will need the following NuGet packages to open the project in Visual Studio:
- MaterialDesignThemes (UI Theme)
- MaterialDesignColors (UI Theme)
- gong-wpf-dragdrop (for enabling drag and drop functionality on the rundown)
- Microsoft.Office.Interop.Excel (this is for planned future integration with spreadsheets)

## Start the server
You'll need a build of CasparCG server:
https://github.com/CasparCG/server

This has been tested with v2.3.3 LTS
https://github.com/CasparCG/server/releases/tag/v2.3.3-lts-stable

Before connecting to a server make sure that you have both the casparcg.exe server running, but also the scanner.exe application running.

## Connecting to the server
When opening the application you will be greeted with the main screen

![image](https://user-images.githubusercontent.com/73527278/152435177-67a861cf-1aa5-4261-b3b3-35a9418d4f8d.png)

In the bottom right corner type in the IP address of the server (or use localhost if the server is on the same machine) and click the Connect icon.
If you've connected sucessfully the icon will go green and the main controls will appear, otherwise the connect icon will turn red and an error will be displayed in the status bar

![image](https://user-images.githubusercontent.com/73527278/152435556-ebb3083d-01cd-4265-be63-91fa363249f6.png)

The main controls are at the bottom of the window, including a channel selection menu.
Playout manager supports up to 3 casparcg channels, one for media, one for CGs and one for previewing.
These can also all be set as one channel and should self-key correctly (all media goes to layer 10, and CGs default to layer 20)
If your preview channel is also set to the playout or cg channels, previewing will interrupt playback if the "Enable Preview" checkbox is checked. This is why this checkbox is unchecked by default

## Adding media items to the rundown
To add a media item or a cg template to the rundown, click on the + icon on the toolbar at the bottom, a pop up will appear:

![image](https://user-images.githubusercontent.com/73527278/152822390-cc1002cf-49cc-467f-bee0-3a04f34a0087.png)

- Enable Preview: This will show a preview of the clip you select on the Preview Channel. It's off by default as sometimes the preview channel may also be the playout channel. It also previews the in and out points when you scrub using the slider.
- Refresh Clips: This will fetch a list of all the clips in the CasparCG media folder.
- Clip Name: Here you can choose the media item to add to the rundown. If Enable Preview is checked the clip will load in to the preview channel. Leave this blank if you don't want a media clip to be triggered.
- PLay: Plays the clip selected on the preview channel.
- Pause: Pauses the clip playing on the preview channel.
- Start Date / Start Time: Sets the automatic scheduled start time for the clip. If you don't want the item to be triggered, just set it to a time in the past.
- In / Out sliders: Sets in and out points for the clip. If you have Enable Preview checked, these will also "scrub" the clip on the preview channel
- Once clip has finished playing: Sets the action to be performed at the end of the clip. Hold will freeze the last frame, Black will stop the clip at the end, Loop will loop the clip until another one is triggered.
- Refresh Templates: This will fetch a list of all the CG Templates in the CasparCG template folder.
- Templates List: Here you can choose the CG template to be triggered at the same time as the media clip. Leave this blank if you don't want a CG to be triggered.
- Play / Update / Stop CG: This will preview the selected action on the preview channel.
- Field 0 / 1: Type some text to be sent to the template as f0 and f1. These are sent as JSON.
- Layer: Chooses the compositing layer that the CG should appear on.
- Delay: The cg's delay in seconds.
- Extra AMCP Commands: Type in AMCP commands that should be triggered when the rundown item is triggered. Some useful command shortcuts are below the text box.
- Cancel: Closes the pop-up
- Update Selected: If a rundown item is selected, it will be overwritten with the current information in the pop-up dialogue.
- Add to rundown: The information in the pop-up dialogue will be added as a new item, at the end of the rundown list.

## Editing Items / Editing Start Time / Deleting Items

![image](https://user-images.githubusercontent.com/73527278/152827214-c8ff5df3-7a84-44d8-9aee-8151b4d1d945.png)

To edit a rundown item, click the edit button. This will take the currently selected item and populate the pop-up dialogue.
Then edit the information and click on Update Selected when finished.

To edit the start time of an item, click the edit start time button

![image](https://user-images.githubusercontent.com/73527278/152827433-f4efd67f-de72-4e84-b4e5-675bcdb7161d.png)

This will open a new pop up where you can change the start time of the item.
By clicking on "Auto-calculate new start times for all items" this will calculate new start times for everything after the selected item, based on each clip's duration, creating an automatic playlist.

To delete an item, select it and click the delete button

## Media playback bar

![image](https://user-images.githubusercontent.com/73527278/152827753-259485e4-274f-4091-8c83-8defd922d7f6.png)

- Play: Plays the selected rundown item
- Auto-calculate new start times for all items: This will calculate new start times for everything after the selected item, based on each clip's duration, creating an automatic playlist.
- Pause: Pauses the currently playing clip.
- Resume: Plays the currently paused clip.
- Stop: Stops the media clip whilst keeping the CG active.
- Play Next: Plays the next item from the currently selected item.
- Panic: Stops all items on all channels, meaning everything goes black.

## Channel selection bar

![image](https://user-images.githubusercontent.com/73527278/152828556-c702a2a6-fb22-4746-b8e7-4cb2eae37837.png)

As mentioned before, here you can select up to 3 individual channels, one for media, one for CGs and one for preview. These however can all also be the same channel.

## Status bar

![image](https://user-images.githubusercontent.com/73527278/152828687-691d0830-cba9-425a-8939-ae40dd3cbf27.png)

Any logged events appear here

## Menu

![image](https://user-images.githubusercontent.com/73527278/152828777-ec3b797a-4423-4c75-948d-7aed4629b610.png)

-File > New Rundown: Clears the rundown.
-File > Save Rundown: Saves the current rundown as a .pmr file.
-File > Load Rundown: Loads a .pmr file with a rundown.
-File > Exit: Quits the application
-View > Toggle Fullscreen: Does what it says on the tin.
-Help > About: Shows an about page with some information.

## Staus Indicators

![image](https://user-images.githubusercontent.com/73527278/152829166-50dbb3c5-4dc3-42be-8405-a3e053cae881.png)

Here you can see the CasparCG Server status, the currently playing clip, the time till the clip ends and a clock.
When a clip is about to finish, the colours change to indicate that a media item is about to finish.

# Contributing
## Prerequisites

You will need the following NuGet packages to open the project in Visual Studio:
- MaterialDesignThemes (UI Theme)
- MaterialDesignColors (UI Theme)
- gong-wpf-dragdrop (for enabling drag and drop functionality on the rundown)
- Microsoft.Office.Interop.Excel (this is for planned future integration with spreadsheets)
