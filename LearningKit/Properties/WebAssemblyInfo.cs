using CMS;

// Ensures that the Kentico API can discover and work with custom classes/components registered in the MVC web project
// Placed into a separate class because AssemblyInfo.cs cannot access code from external libraries in cases where
// the web project is precompiled with outputs merged into a single assembly
[assembly: AssemblyDiscoverable]