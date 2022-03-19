using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using TestInterfaces;
using TestUtils;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Tests.EditMode {
    public class Test07_GitUnityVisualStudio : TestSuite {
        [TestCase("2021.2.7f1")]
        public void A02a_CheckUnityVersion(string unityVersion) {
            Assert.AreEqual(unityVersion, Application.unityVersion);
        }
        [TestCase("./.git")]
        public void A02b_GitInitialized(string path) {
            var directory = new DirectoryInfo(path);
            Assert.IsTrue(directory.Exists, $"Directory '{directory.FullName}' not found. Your Unity project folders must reside in the root of your repository.");
        }
        [TestCase("./.gitignore")]
        [TestCase("./.editorconfig")]
        public void A02c_ProjectFileExists(string path) {
            var file = new FileInfo(path);
            Assert.IsTrue(file.Exists, $"File '{file.FullName}' not found. Did you copy them from FlappyBird?");
        }
        [Test]
        public void A02d_CompanyNameIsEmailAddress() {
            Assert.AreNotEqual("DefaultCompany", Application.companyName, $"Change Company Name in Project Settings > Player!");
            StringAssert.IsMatch(Assets.emailPattern, Application.companyName, $"Company Name must be a valid e-mail address, but was '{Application.companyName}'");
        }
        [Test]
        public void A02e_CompanyNameIsELearningAddress() {
            StringAssert.IsMatch(Assets.elearningPattern, Application.companyName, $"Company Name must be your e-learning address! E-Learning addresses start with either 's' or 'bt'.");
        }
        [TestCase("com.unity.inputsystem", "1.2.0")]
        [TestCase("com.unity.textmeshpro", "3.0.6")]
        [TestCase("com.unity.render-pipelines.universal", "12.1.2")]
        public async void A02f_PackageIsVersion(string package, string version) {
            var search = Client.List(true, false);

            do {
                await Task.Delay(100);
            } while (!search.IsCompleted);

            Assert.IsNotNull(search.Result, $"Failed to list packages!");
            foreach (var info in search.Result) {
                if (info.name == package) {
                    Assert.GreaterOrEqual(Version.Parse(info.version), Version.Parse(version), $"Gotta update package '{package}' to version '{version}'!");
                    return;
                }
            }
            Assert.Fail($"Gotta install package '{package}' in version '{version}'!");
        }
    }
}