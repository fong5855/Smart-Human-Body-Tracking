# EMSK ARC 3D Remote Communication and Interaction

## Introduction

## Required Hardware
* DesignWare ARC EM Starter Kit(EMSK)
* ESP8266 WiFi module
* FTDI FT2232 usb to UART module
* WiFi Hotspot
* Depth camera module (PC + Kinect) 
* client (HoloLens, PC, Smart phone, etc.)


## Require Software
* Metaware or ARC GNU Toolset
* OpenNI
* Robot OS (ROS)
* Client display

## Hardware Connection
![Connection](imgs/arc_connection.jpg)  

## Architecture
The communication system is driven by EMSK ARC, which is a low power always on processor, waiting until one make a call and then power up the depth camera system.  

We can get human skeleton by **depth camera module**. (In our case we use kinect and PC and get skeleton by OpenNI skeleton algorithm.)  
Then we using **ROS** to integrate to environments (camera informations, skeleton informations and ftdi transition) and sent the detected skeleton to ARC through **FTDI FT2232** chip.  

![](imgs/tracking.png)

The **EMSK ARC**, which serve as the low power server, collect the detected skeleton and process the skeleton nodes let the client can easliy handle these informations. The connection between the client and ARC is the socket portocal and ARC will send the infomations to client by **ESP8266 WiFi module**.  

The client (At least PC, we use extra device to make more usage scenarios) recieve the infomations and apply the human body pose to the 3D hologram with few computations and we will able to interact with the hologram!  

# User Manual
1. Start the EMSK ARC to wait for call.
![init](imgs/arc_init.PNG)
2. User make a call to EMSK, then the EMSK ARC will poewer up the depth camera module and sent the detect human body infomations back.
3. The user can interact with the pose infomations.

