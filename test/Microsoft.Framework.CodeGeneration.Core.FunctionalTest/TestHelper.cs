﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.DependencyInjection.Fallback;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Infrastructure;

namespace Microsoft.Framework.CodeGeneration.Core.FunctionalTest
{
    public static class TestHelper
    {
        public static IServiceProvider CreateServices(string testAppName)
        {
            var originalProvider = CallContextServiceLocator.Locator.ServiceProvider;
            var appEnvironment = originalProvider.GetService<IApplicationEnvironment>();

            //When the tests are run the appEnvironment points to test project.
            //Change the app environment to point to the test application to be used
            //by test.
            var originalAppBase = appEnvironment.ApplicationBasePath; ////Microsoft.Framework.CodeGeneration.Core.FunctionalTest
            var testAppPath = Path.GetFullPath(Path.Combine(originalAppBase, "..", "TestApps", testAppName));

            var services = new ServiceCollection();
            services.AddInstance(
                typeof(IApplicationEnvironment),
                new TestApplicationEnvironment(appEnvironment, testAppPath, testAppName));

            return services.BuildServiceProvider(originalProvider);
        }
    }
}