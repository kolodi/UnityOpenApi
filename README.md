# UnityOpenApi

Open API parser and client generator for Unity

## Concept diagram

![Concept diagram](https://docs.google.com/drawings/d/1c1kjdK5TJ_ZHc8onSXXMQAmeI5XNVedIPpmzbtxMUb0/edit?usp=sharing)

## Purpose

Parsing OpenAPI documents and creating reusable Unity assets for easy API consuming in your Unity projects. Designed to work on any target platform.

## Getting Started

Use **Api Parser** ScriptableObject to parse OpenAPI description from file or url. You will be prompted to select the folder where to put the generated API assets. You can select the same folder next time to update assets, this will aslo assure to preserve all existing references to these assets within the project.

There are 2 types of assets generated: 
* The main API Asset containing all common data and settings
* The path assets for each individual path in the API, these assets are then references in different places of your project to consume the API.

### Prerequisites

Unity 2018.3 was used, but it should be compatible with older versions as well.

### Installing

Just clone the repository and open the project in Unity.
All meta files are preserved.
During the first launch, Unity can take a bit to generate caches.

## Running the tests

There are no automated tests for now

## Third party libraries and packages

* [Editor Coroutines](https://docs.unity3d.com/Packages/com.unity.editorcoroutines@0.0/manual/index.html) - allows you to test web requests in editor without hitting play button
* [JSON.NET for Unity](https://www.parentelement.com/assets/json_net_unity) - parsing your data
* [OpenAPI.NET](https://github.com/Microsoft/OpenAPI.NET) - parsing Swagger and OpenAPI3 documents (JSON and YAML)

## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.

## Authors

* **Kolodi** - *Initial work* - [kolodi](https://github.com/kolodi)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Inspired by [OpenAPI Tools](https://github.com/OpenAPITools)
