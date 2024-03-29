﻿# HattoriMeowzo
This project is a .NET Core web app intended to run on a Raspberry Pi 3b (4 untested personally, but probably fine) operating as the control center for a robot cat toy with a servo, motion detector and a Raspberry Pi camera attached. 

Short Description of Project:

A servo connected to rack and pinion gears makes an object extend and retract from an enclosure, emulating the motion of a mouse leaving and retreating from its hole. 

When the motion detector senses motion, the camera takes a picture and then the app uses OpenCvSharp to check if it can recognize a cat's face in the photo. 

If it does recognize a cat's face, it takes another picture and a short video while it runs a predefined routine of movements. If a cat's face is not recognized in the photo taken after movement was detected, the toy returns to checking for movement. 

Meanwhile, the photos and videos taken by the toy can be viewed on a web app running locally on the Raspberry Pi. Ngrok or other solutions may make viewing the cat toy's media more convenient.

To run this project, you'll need to install/compile a few things on your Raspberry Pi: 

.NET 5 SDK 
OpenCv 
OpenCvSharp 

On my website, I wrote a tutorial covering the different stages of this project, and in the third post, I mention the resources I used to complete those three steps above: https://grantdesselle.com/uncategorized/hattori-meowzo-raspberry-pi-cat-toy-building-the-web-app-and-object-recognition-using-opencvsharp/

Additionally, make sure your camera is enabled in your Raspberry Pi configuration settings. 

Important: After you download/clone the project, you'll need to publish it for the Raspberry Pi, but before you do that, check the paths listed at the top of the Camera class. Those magic strings could break the app. I wrote the paths assuming that the app's root folder will be renamed to CatToyWebApp and installed on the desktop of a Raspberry Pi with the default username of pi.

To run the app:
Build it, and then in the command line for the root directory, enter (without quotes) 'dotnet publish -r linux-arm'. The command prompt should print out a publish folder at the end. Transfer this folder to the Raspberry Pi.

Once that folder has transferred, rename it to CatToyWebApp. Next, go into the wwwroot folders and make sure any placeholder images and dummy txt files have been removed. If the app is published with empty folders inside the wwwroot folder, those folders may not get copied over. That's the reason for those dummy files.

To ensure object recognition functions, go into the directory where you compiled OpenCvSharp on your Raspberry Pi. You want to take a copy of the 'libOpenCvSharpExtern.so' file and paste it into the root directory of the web app. On my Raspberry Pi, this file is located at /home/pi/Desktop/opencvsharp-4.5.3.20210821/opencvsharp-4.5.3.20210821/src/build/OpenCvSharpExtern/libOpenCvSharpExtern.so . 

Once you've copied over that file from the OpenCvSharp directory, open a terminal window and navigate to the CatToyWebApp root folder. Without quotes, enter 'chmod +x CatToyWebApp' to give the folder execute permissions, and on the next line enter './CatToyWebApp', to run it. 

Once the app is running, the terminal window will tell you the port where you can visit the web app and view the photos and videos it has taken. 

This project is the starting point for what I hope I can flesh out into something much more robust and valueable. Thanks for checking it out. 

I also made a simple console app, CatToyTestApp, which may help you more easily/quickly adjust your camera and motion detection settings as well as ensure everything is wired correctly. You need .NET 5 SDK installed on the Pi.

In addition to the web app's boiler plate, a large portion of my code was taken from other repositories. Mostly, these:
.NET IoT: https://github.com/dotnet/iot
MMALSharp: https://github.com/techyian/MMALSharp 
OpenCvSharp: https://github.com/shimat/opencvsharp
