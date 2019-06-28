**🛈 This repository is intended as a read-only source of information, and contributions by the general public are not expected.**

# LearningKit

 LearningKit is a functional website for learning purposes. It demonstrates how to implement various Kentico features on MVC websites in the form of code snippets, which you can run if you connect the website to a Kentico database.
 
 ### Instructions for running the LearningKit project

1. [Install](https://docs.kentico.com/x/IIO-BQ) Kentico (you can select the Portal Engine model in the installer, without a sample site).
2. [Create a new site](https://docs.kentico.com/x/Doq-BQ) in the **Sites** application based on the **MVC Blank Site** web template.
3. Set the **Presentation URL** of the new site to the URL where you plan to run the LearningKit project.
4. [Enable web farms](https://docs.kentico.com/x/7oi-BQ) in automatic mode.
5. Rename the `LearningKit\ConnectionStrings.config.template` file to `ConnectionStrings.config`.
6. Rename the `LearningKit\AppSettings.config.template` file to `AppSettings.config`.
7. Copy the `CMSConnectionString` connection string from the Kentico `web.config` file to the `LearningKit\ConnectionStrings.config` file.
8. Copy the `CMSHashStringSalt` app setting from the Kentico `web.config` file to the `LearningKit\AppSettings.config` file.
9. Open the `LearningKit.sln` solution in Visual Studio and run the LearningKit web application.

### Accessing older version of the repository

You can find older versions of the LearningKit project under the [Releases section](https://github.com/KenticoInternal/LearningKit-Mvc/releases) (open the corresponding commit for the required version and click **Browse files**).

Quick links:

- [Kentico 12 (without the Service Pack)](https://github.com/KenticoInternal/LearningKit-Mvc/tree/453a6dcae3f4e512d5a2a2450ba7f0ccaad77305)
