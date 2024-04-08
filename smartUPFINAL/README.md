Smart House Control Panel
Overview
The Smart House Control Panel is a Windows Presentation Foundation (WPF) application developed in C# that serves as a centralized control hub for managing various aspects of a smart home environment. Leveraging the power of C# and WPF, the application provides users with an intuitive and feature-rich interface for controlling devices, monitoring temperature, and accessing multimedia content.

Features
Device Control: Turn on/off lights, open/close gates, activate/deactivate cameras, and control other smart devices remotely.
Temperature Monitoring: Display current outdoor temperature and recommend indoor temperature based on weather conditions.
Video Playback: Stream live video feeds from surveillance cameras directly within the application.
Real-time Updates: Receive instant updates on device status changes and temperature fluctuations.
Installation
Clone or download the repository to your local machine.
Open the solution file (SmartHouseControlPanel.sln) in Visual Studio.
Build the solution to restore dependencies and compile the application.
Run the application from Visual Studio or navigate to the output directory and launch the executable file (SmartHouseControlPanel.exe).
Usage
Device Control: Use the provided buttons in the UI to control various devices in your smart home. Clicking on a button triggers the corresponding action, updating the device status in real-time.
Temperature Monitoring: The outdoor temperature is fetched from an external API (e.g., OpenWeatherMap) and displayed in the UI. The recommended indoor temperature is calculated based on the outdoor temperature and displayed alongside.
Video Playback: Click the "Turn On Cameras" button to activate video surveillance. The application streams live video feeds from connected cameras, providing users with real-time monitoring capabilities.
Dependencies
C#: The application is developed using the C# programming language.
WPF: Windows Presentation Foundation (WPF) is used for building the graphical user interface.
Entity Framework: Entity Framework is utilized for interacting with the SQL database.
Newtonsoft.Json: Newtonsoft.Json is used for parsing JSON data from external APIs.
HttpClient: The HttpClient class is used for making HTTP requests to fetch weather data.
Contributing
Contributions to the Smart House Control Panel project are welcome! If you have ideas for new features, improvements, or bug fixes, feel free to open an issue or submit a pull request.

License
This project is licensed under the MIT License.

"# C-smart-house" 
