**🛈 This repository is intended as a read-only source of information, and contributions by the general public are not expected.**

# LearningKit

 LearningKit is a functional website for learning purposes. It demonstrates how to implement various Kentico Xperience features on MVC websites in the form of code snippets, which you can run if you connect the website to a Kentico Xperience database. See [Kentico Xperience sample sites](https://devnet.kentico.com/articles/kentico-xperience-sample-sites-and-their-differences) for a detailed description of this and other Xperience sample sites.

## Instructions for running the LearningKit project

1. [Install](https://docs.xperience.io/x/bAmRBg) Kentico Xperience in the ASP.NET MVC 5 development model. When choosing the installation type, select *New site*.
1. Set the **Presentation URL** of the new site to the URL where you plan to run the LearningKit project.
1. [Enable web farms](https://docs.xperience.io/x/Mw_RBg) in automatic mode.
1. Rename the `LearningKit\ConnectionStrings.config.template` file to `ConnectionStrings.config`.
1. Rename the `LearningKit\AppSettings.config.template` file to `AppSettings.config`.
1. Copy the `CMSConnectionString` connection string from the Xperience administration application's `web.config` file to the `LearningKit\ConnectionStrings.config` file.
1. Copy the `CMSHashStringSalt` app setting from the Xperience administration application's `web.config` file to the `LearningKit\AppSettings.config` file.
1. Open the `LearningKit.sln` solution in Visual Studio and run the LearningKit web application.

## Accessing older version of the repository

You can find older versions of the LearningKit project under the [Releases section](https://github.com/KenticoInternal/LearningKit-Mvc/releases) (open the corresponding commit for the required version and click **Browse files**).

Quick links:

- [LearningKit for Kentico 12 Service Pack](https://github.com/KenticoInternal/LearningKit-Mvc/releases/tag/2.0.0)
- [LearningKit for Kentico 12](https://github.com/KenticoInternal/LearningKit-Mvc/releases/tag/v1.0.0)
