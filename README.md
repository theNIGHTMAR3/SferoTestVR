# SferoTestVR
By Michał Kuprianowicz and Jakub Wierzba. <br>
Started the Engineer project and continued with the Master's thesis.

## Overview
**SferoTestVR** is a Unity-based application that integrates with the VirtuSphere spherical locomotion simulator to provide an
immersive VR walking experience. It enables real-world sphere motion control, procedural environment generation, and real-time motor adjustments. 
Complementing this, **SferoTestGrapher** is a web-based tool for visualising and analysing logged movement data.

### Core Application: SferoTestVR
- **Gameplay**: 'Dungeon-like' game, where the player controls a rolling sphere. The goal is to get through the dungeon consiting of rooms and corridor with varying levels of difficulty.
- **Procedural environment**: Dynamically generates a path from predefined rooms and varied corridors with collision detection to create varied walking scenarios.
- **Motor control zones**: Defines virtual areas where the application overrides motor behaviour to enforce player's speed and direction.
- **Data logging**: Records telemetry (position, velocity, motor voltage/current, brake status) in JSON format for each session.
- **User interface**: Simple VR menu for creating paths, viewing session time, and switching modes (HomePC, HomeVR and VirtuSphere).

### Analysis Tool: SferoTestGrapher
- **Web dashboard**: Built with HTML, CSS, and JavaScript for cross-platform accessibility.
- **Data ingestion**: Loads JSON logs to plot trajectories, speed profiles, and motor parameters over time.
- **Info access**: Hovering the cursor over the chart shows all available data for a given position.

## User Studies
This project aims to research how natural walking is in VirtuSphere with custom engines enabled. To achieve this, experiments will be carried out: 
- **Participants**: At least 15 volunteers with varying knowledge in VR.
- **Training phase**: 5‑minute session to learn controls before the main approach.
- **Protocol**: Each participant completes a predefined path, which takes around 25 minutes. After completing it, a survey is conducted:
- **Questionnaires**:
	- General feelings 
	- System Usability Scale (SUS)
	- Simulator sickness questionnaire
	- Immersion questionnaire (Slater-Usoh-Steed)
- **Results**: Compare the obtained results and conclude. It should give a definite answer to the main question in the master's thesis.
  
## Disclamer
The repository is public, but to make the application run properly, it needs a custom .dll library. This library is not shared because it is the intellectual property of Gdańsk University of Technology's VirtuSphere.
It was custom-made for that device, and due to the copyright law, it can not be shared. 
