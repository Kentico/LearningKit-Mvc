**🛈 This repository is intended as a read-only source of information, and contributions by the general public are not expected.**

# LearningKit

 LearningKit is a functional website for learning purposes. It demonstrates how to implement various Kentico features on MVC websites in the form of code snippets, which you can run if you connect the website to a Kentico database.
 
 ### Instructions for running the LearningKit project

1. [Install](https://docs.kentico.com/x/o5mUB) Kentico (you can select the Portal Engine model in the installer, without a sample site).
2. [Create a new site](https://docs.kentico.com/x/756UB) in the **Sites** application based on the **MVC Blank Site** web template.
3. Set the **Presentation URL** of the new site to the URL where you plan to run the LearningKit project.
4. [Enable web farms](https://docs.kentico.com/k12/configuring-kentico/setting-up-web-farms/configuring-web-farm-servers) in automatic mode.
5. Rename the `samples\LearningKit\ConnectionStrings.config.template` file to `ConnectionStrings.config`.
6. Rename the `samples\LearningKit\AppSettings.config.template` file to `AppSettings.config`.
7. Copy the `CMSConnectionString` connection string from the Kentico `web.config` file to the `samples\LearningKit\ConnectionStrings.config` file.
8. Copy the `CMSHashStringSalt` app setting from the Kentico `web.config` file to the `samples\LearningKit\AppSettings.config` file.
9. Open the `LearningKit.sln` solution in Visual Studio and run the LearningKit web application.
