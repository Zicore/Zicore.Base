using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zicore.Base.Json;

namespace Zicore.Base.Test
{
    [TestClass]
    public class JsonSerializableTest
    {
        readonly JsonSerialzableTestValues _testValues = new JsonSerialzableTestValues
        {
            DecimalValue = 5005.5m,
            IntegerValue = -951284,
            StringValue = "String Value Test",
            DoubleValue = 951238d
        };

        [TestMethod]
        public void TestSaveAndLoadAppDataFolder()
        {
            const string AppName = "ZicoreBaseTest";
            const string FileName = "Test.json";

            var filePath = JsonSerializable.GetAppDataFilePath(FileName, AppName);

            // Ensure to delete before test
            File.Delete(filePath);

            JsonSerialzableTestValues test = new JsonSerialzableTestValues();
            try
            {
                // Assign Test Values
                AssignTestValues(test, _testValues);

                // Save to file
                test.SaveToAppData(FileName, AppName);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            test = new JsonSerialzableTestValues(); // Create new class with different values
            try
            {
                test.LoadFromAppData(FileName,AppName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            
            // Test against test values
            CompareValues(test,_testValues);

            // cleanup test file
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        [TestMethod]
        public void TestSaveAndLoadApplicationFolderWithSubDirectory()
        {
            const string SubDir = "config";
            const string FileName = "Test.json";

            var filePath = JsonSerializable.GetApplicationDirectoryFilePath(FileName,SubDir);
            FileInfo fi = new FileInfo(filePath);

            if (fi.DirectoryName != null) Directory.CreateDirectory(fi.DirectoryName);

            // Ensure to delete before test
            try
            {
                File.Delete(filePath);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            JsonSerialzableTestValues test = new JsonSerialzableTestValues();
            try
            {
                // Assign Test Values
                AssignTestValues(test, _testValues);

                // Save to file
                test.SaveToApplicationDirectory(FileName,SubDir);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            test = new JsonSerialzableTestValues(); // Create new class with different values
            try
            {
                test.LoadFromApplicationDirectory(FileName,SubDir);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // Test against test values
            CompareValues(test, _testValues);

            // cleanup test file
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        [TestMethod]
        public void TestSaveAndLoadApplicationFolder()
        {
            const string SubDir = "";
            const string FileName = "Test.json";

            // Empty subdirectory (should point to the applications folder by the default behaviour of Path.Combine(a,b,c) when b is empty)
            var filePath = JsonSerializable.GetApplicationDirectoryFilePath(FileName, SubDir);
            FileInfo fi = new FileInfo(filePath);

            if (fi.DirectoryName != null) Directory.CreateDirectory(fi.DirectoryName);

            // Ensure to delete before test
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            JsonSerialzableTestValues test = new JsonSerialzableTestValues();
            try
            {
                // Assign Test Values
                AssignTestValues(test, _testValues);

                // Save to file
                test.SaveToApplicationDirectory(FileName, SubDir);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            test = new JsonSerialzableTestValues(); // Create new class with different values
            try
            {
                test.LoadFromApplicationDirectory(FileName, SubDir);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // Test against test values
            CompareValues(test, _testValues);

            // cleanup test file
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Compare with my Test scenario
        /// </summary>
        /// <param name="test"></param>
        /// <param name="testValues"></param>
        private void CompareValues(JsonSerialzableTestValues test, JsonSerialzableTestValues testValues)
        {
            Assert.AreEqual(test.IntegerValue, testValues.IntegerValue);
            Assert.AreEqual(test.DecimalValue, testValues.DecimalValue);
            Assert.AreEqual(test.DoubleValue, testValues.DoubleValue);
            Assert.AreEqual(test.StringValue, testValues.StringValue);
        }

        private void AssignTestValues(JsonSerialzableTestValues test, JsonSerialzableTestValues testValues)
        {
            test.IntegerValue = testValues.IntegerValue;
            test.DecimalValue = testValues.DecimalValue;
            test.StringValue = testValues.StringValue;
            test.DoubleValue = testValues.DoubleValue;
        }
    }
}
