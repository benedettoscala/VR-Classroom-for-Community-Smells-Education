# VR Classroom for Community Smells Education

## Overview 🎓

**VR Classroom for Community Smells Education** is a Unity-based project that leverages **VRChat** to create an interactive classroom environment. This virtual classroom is designed for experiments detailed in "_The Good and the Bad of Teaching Social Debt and Community Smells Using Virtual Reality_" paper, enhancing education through **3D avatars**, **animations**, and **community smell visualizations**. 🖥️

## Data Availability
All the materials related to the experiment — including the datasets for each treatment, the statistical analyses, and the ethical approval — are located in the ```EmpiricalExperimentMaterials```folder.

## Features

### Teacher Controls 🎤

- **Manage avatar animations** to depict various community smells.
- **Switch environments** between an office and a classroom.
- **Control slide presentations** on a virtual board.
- **Engage with students** via voice chat.

### Student Interactions 🧑‍🎓

- **Sit in designated seats** to experience the lecture.
- **Communicate via voice chat** with the teacher and peers.

### Community Smells Visualized 🎭

- **Organizational Silo** – Isolated subgroups limiting collaboration.
- **Lone Wolf** – Individuals working independently, detached from the team.
- **Black Cloud Effect** – A negative influence affecting the entire group.

## Requirements

You need to have an installation of **VRChat** (either through **Steam** or the **Meta platform**).

## Ready-to-Use Classroom 🎮

The virtual classroom is already available on VRChat servers! You can access it directly via the following link:

📞 [VRChat Classroom](https://vrchat.com/home/world/wrld_357a500a-2867-45d1-9c9f-f1b08d6be309/info)

## How to Contribute 🚀 🛠️

1. Clone the repository:
   ```sh
   git clone https://github.com/benedettoscala/VR-Classroom-for-Community-Smells-Education.git
   ```
2. Open the project in **Unity 2022.3.22f1** (recommended).
3. If prompted with import issues, select **Ignore**.
4. Install any missing dependencies when prompted.

All modifiable elements (scripts, animations, and more) are located in the **Assets** folder.

### Running in Unity

1. If the scene doesn't load, navigate to the **scene directory** and open `CommunitySmellsLesson.unity`.
2. Click the **Play** button in Unity to start the project in debug mode.

### Deploying in VRChat

1. Open **VRChat SDK** and go to **Show Control Panel**.
2. Log in with your **VRChat account** (required for uploading content).
3. Navigate to the **Builder** tab and select **Build & Test New Build**.
4. To publish the project online, ensure your VRChat account meets the necessary permissions. More details are available in the [VRChat documentation](https://docs.vrchat.com/).

## Project Structure 📁

```
CommunitySmellsLesson/
├── Assets/                              # Unity assets (slides, scripts, audio, animations, prefabs)
├── EmpiricalExperimentMaterial/         # Experimental data used for the experiment
├  ├── StatisticalAnalysis/
├  ├── Ethical Approval/ 
├── Packages/                            # Package dependencies and external libraries
├── ProjectSettings/                     # Unity project settings and configurations
├── README.md                            # Project documentation
├── .gitignore                           # Git configuration
```

## Contributing 🤝

We welcome contributions! Feel free to open an **issue** or submit a **pull request** to enhance the project.

## License 🐟

This project is licensed under the **MIT License**.

## Contact ✉️

For inquiries or collaboration opportunities, please reach out via [your contact information].
